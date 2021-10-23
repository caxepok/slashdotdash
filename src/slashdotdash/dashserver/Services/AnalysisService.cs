using dashserver.Enums;
using dashserver.Infrastructure;
using dashserver.Models.API;
using dashserver.Models.DB;
using dashserver.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace dashserver.Services
{
    /// <inheritdoc cref="IAnalysisService"/>
    public class AnalysisService : IAnalysisService
    {
        private readonly ILogger _logger;
        private readonly DashDBContext _dashDBContext;

        public AnalysisService(ILogger<AnalysisService> logger, DashDBContext dashDBContext)
        {
            _logger = logger;
            _dashDBContext = dashDBContext;
        }

        public IEnumerable<Models.API.KPI> GetKPIInfo()
        {
            List<Models.API.KPI> kpiInfos = new();
            foreach (var kpi in _dashDBContext.KPIs.ToList())
            {
                // просто вытаскиваем заранее сохранённые КПЭ из базы
                var values = _dashDBContext.KPIRecords
                    .Where(_ => _.KPIId == kpi.Id)
                    .OrderBy(_ => _.Date)
                    .Select(_ => new ValueOnDate(_.Date, _.Value)).ToArray();

                var apiKPI = new Models.API.KPI()
                {
                    Id = kpi.Id,
                    Name = kpi.Name,
                    Type = kpi.KPIType,
                    Threshold = kpi.Threshold,
                    Values = values,
                    TodayValue = values[^1].Value,
                    YesterdayValue = values[^2].Value    // todo: проверка на длину массива
                };
                kpiInfos.Add(apiKPI);

                IEnumerable<KPIRecomendation> recomendations = GetKPIRecommendationsAsync(kpi);
                if (recomendations.Any())
                    apiKPI.HaveRecomendations = true;
            }

            return kpiInfos;
        }


        public IEnumerable<ShopAverageOccupiedPercent> GetShopAnalysis(Plan plan)
        {
            var shops = _dashDBContext.Shops.Include(_ => _.ResourceGroups).ToList();
            List<ShopAverageOccupiedPercent> shopAverageOccupiedPercents = new(shops.Count);
            foreach (Shop shop in shops)
            {
                var days = _dashDBContext.PlanDays
                    .Where(_ => _.PlanId == plan.Id && _.Day <= 14)
                    .OrderBy(_ => _.Day).ToList();
                // ToList т.к. следующее выражение не кладётся EF`ом на SQL
                var averageOccupied = days.Where(_ => shop.ResourceGroups.Any(rg => rg.Id == _.ResourceGroupId))
                    .Select(_ => _.OccupiedPercent)
                    .Average();

                var perDay = days.GroupBy(_ => _.Day)
                    .Select(g => new ValueOnDate(plan.Date.AddDays(g.Key), g.Average(a => a.OccupiedPercent)));

                shopAverageOccupiedPercents.Add(new ShopAverageOccupiedPercent()
                {
                    Name = shop.Name,
                    ShopId = shop.Id,
                    Value = Math.Round(averageOccupied, 2)
                });
            }

            return shopAverageOccupiedPercents;
        }

        public IEnumerable<ResourceGroupOccupiedPercent> GetResourceGroupRecomendations(Plan plan, int shopId)
        {
            var shop = _dashDBContext.Shops.Include(_ => _.ResourceGroups).SingleOrDefault(_ => _.Id == shopId);
            List<ResourceGroupOccupiedPercent> resourceGroupOccupiedPercents = new(shop.ResourceGroups.Count);
            foreach (var resourceGroup in shop.ResourceGroups)
            {
                // выбираем все дни плана для агрегатов группы
                var days = _dashDBContext.PlanDays
                    .Where(_ => _.ResourceGroupId == resourceGroup.Id && _.PlanId == plan.Id && _.Day <= 14)
                    .OrderBy(_ => _.Day)
                    .ToList();

                // средняя загрузка всей группы агрегатов
                decimal averageOccupied = days.Average(_ => _.OccupiedPercent);
                List<ValueOnDate> valuesPerDate = new();
                var rgop = new ResourceGroupOccupiedPercent()
                {
                    ResourceGroupId = resourceGroup.Id,
                    AverageOccupation = averageOccupied,
                    PerDay = valuesPerDate
                };

                // считаем загрузку на каждый день по группе агрегатов
                // тут же проверяем склады\цепочки складов
                var perDayGroups = days.GroupBy(_ => _.Day);
                foreach (var perDayGroup in perDayGroups)
                {
                    var date = plan.Date.AddDays(perDayGroup.Key);
                    var vad = new ValueOnDate(date, perDayGroup.Average(a => a.OccupiedPercent));
                    valuesPerDate.Add(vad);

                    if (vad.Value > 100)  // загрузка группы агрегатов не может быть больше 100%
                        vad.Warning = "Оборудование не справится с объёмом";

                    else if (vad.Value < rgop.Threshold)    // агрегаты недозагружены, проанализируем почему
                        vad.Warning = "анализ цепочки складов"; // AnalyzeStockLinks(resourceGroup, date);
                }

                // схлопываем варнинги
                int warningsCount = valuesPerDate.Count(_ => _.Warning != null);
                if (warningsCount > 0)
                    rgop.Warning = $"Есть проблемы загрузкой оборудования: {warningsCount}";

                resourceGroupOccupiedPercents.Add(rgop);
            }

            return resourceGroupOccupiedPercents;
        }

        public IEnumerable<ResourceOccupiedPercent> GetResourceOccupiedPercent(Plan plan, int resourceGroupId)
        {
            var resourceGroup = _dashDBContext.ResourceGroups.Include(_ => _.Resources).SingleOrDefault(_ => _.Id == resourceGroupId);
            List<ResourceOccupiedPercent> resourceOccupiedPercents = new(resourceGroup.Resources.Count);
            foreach (var resource in resourceGroup.Resources)
            {
                var days = _dashDBContext.PlanDays
                    .Where(_ => _.ResourceId == resource.Id && _.PlanId == plan.Id && _.Day <= 14)
                    .OrderBy(_ => _.Day)
                    .ToList();

                decimal averageOccupied = days.Average(_ => _.OccupiedPercent);
                var perDay = days.GroupBy(_ => _.Day)
                    .Select(g => new ValueOnDate(plan.Date.AddDays(g.Key), g.Average(a => a.OccupiedPercent)));

                var rop = new ResourceOccupiedPercent()
                {
                    ResourceId = resource.Id,
                    AverageOccupation = averageOccupied,
                    PerDay = perDay
                };
                resourceOccupiedPercents.Add(rop);
            }

            return resourceOccupiedPercents;
        }

        public IEnumerable<PlanSummaryNode> GetPlanSummary(Plan plan)
        {
            List<PlanSummaryNode> nodesSh = new();
            var days = _dashDBContext.PlanDays
                    .Where(_ => _.PlanId == plan.Id && _.Day <= 14)
                    .OrderBy(_ => _.Day).ToList();
            var shops = _dashDBContext.Shops.Include(_ => _.ResourceGroups).ToList();
            // считаем по цехам
            foreach (var shop in shops)
            {
                List<PlanSummaryNode> nodesRg = new();
                // среднее по цеху
                var averageSh = days.Where(_ => shop.ResourceGroups.Any(rg => rg.Id == _.ResourceGroupId))
                    .Select(_ => _.OccupiedPercent)
                    .Average();
                // среднее по цеху по дням
                var perDaySh = days.Where(_ => shop.ResourceGroups.Any(rg => rg.Id == _.ResourceGroupId)).GroupBy(_ => _.Day)
                    .Select(g => new ValueOnDate(plan.Date.AddDays(g.Key), g.Average(a => a.OccupiedPercent))).ToList();
                // считаем для групп
                foreach (var resourceGroup in shop.ResourceGroups)
                {
                    List<PlanSummaryNode> nodesRes = new();
                    // среднее для группы
                    var averageRg = days.Where(_ => shop.ResourceGroups.Any(rg => rg.Id == _.ResourceGroupId))
                                    .Select(_ => _.OccupiedPercent)
                                    .Average();
                    // по дням для группы
                    var perDayRg = days.GroupBy(_ => _.Day)
                        .Select(g => new ValueOnDate(plan.Date.AddDays(g.Key), g.Average(a => a.OccupiedPercent))).ToList();
                    // за каждый день для группы мы можем проанализировать цепочку складов
                    foreach (var pdrg in perDayRg)
                    {
                        if (pdrg.Value < 80)
                        {
                            pdrg.Warning = AnalyzeResourceGroup(resourceGroup.Id, pdrg.Date, pdrg.Value);
                        }
                        else if (pdrg.Value > 100)
                        {
                            pdrg.Warning = "Группа агрегатов не справится с объёмом";
                        }
                    }
                    // считаем для агрегатов
                    foreach (var resource in _dashDBContext.Resources.Where(_ => _.ResourceGroupId == resourceGroup.Id))
                    {
                        // среднее по агрегату
                        decimal averageRes = days.Where(_ => _.ResourceId == resource.Id).Average(_ => _.OccupiedPercent);
                        // по длням по агрегату
                        var perDayRes = days.Where(_ => _.ResourceId == resource.Id).GroupBy(_ => _.Day)
                            .Select(g => new ValueOnDate(plan.Date.AddDays(g.Key), g.Average(a => a.OccupiedPercent))).ToList();
                        // анализ нагрузки на агрегат
                        foreach (var pdres in perDayRes)
                        {
                            if (pdres.Value > 100)
                                pdres.Warning = "Агрегат не справится с объёмом";
                        }
                        PlanSummaryNode nodeRes = new PlanSummaryNode(resource.Id, resource.Name, String.Empty, perDayRes);
                        nodesRes.Add(nodeRes);
                    }
                    PlanSummaryNode nodeRg = new PlanSummaryNode(resourceGroup.Id, resourceGroup.Name, String.Empty, perDayRg);
                    nodeRg.Childs = nodesRes;
                    nodesRg.Add(nodeRg);
                }
                PlanSummaryNode nodeSh = new PlanSummaryNode(shop.Id, shop.Name, String.Empty, perDaySh);
                nodeSh.Childs = nodesRg;
                nodesSh.Add(nodeSh);
            }
            return nodesSh;
        }

        /// <summary>
        /// Анализ цепочек складов ресурсной группы
        /// </summary>
        /// <param name="pdrg"></param>
        private string AnalyzeResourceGroup(int resourceGroupId, DateTimeOffset date, decimal value)
        {
            List<string> warnings = new();
            var stockLinks = _dashDBContext.StockLinks.Include(_ => _.Stock).ToList();  // подтянем связки складов
            foreach (var stockLink in stockLinks.Where(_ => _.ResourceGroupId == resourceGroupId))
            {
                // баланс склада на нужную дату
                StockBalance sb = _dashDBContext.StockBalances.SingleOrDefault(_ => _.Date == date && _.StockId == stockLink.StockId);
                if (sb == null)
                    continue;

                if (stockLink.Type == StockLinkType.In && sb.PlannedBalance == 0)
                {
                    warnings.Add($"На входящем складе ({stockLink.Stock.Name}) нет материалов.");
                    // проверим цепочку
                    var chainCheckResult = TestStockChain(stockLinks, stockLink, date);
                    if (chainCheckResult != null)
                        warnings.Add(chainCheckResult);
                }
                if(stockLink.Type == StockLinkType.Out && sb.PlannedBalance >= sb.MaxBalance && sb.AllowedOverload)
                {
                    warnings.Add($"Исходящий склад ({stockLink.Stock.Name}) заполнен, нет места.");
                    // проверим цепочку
                    var chainCheckResult = TestStockChain(stockLinks, stockLink, date);
                    if (chainCheckResult != null)
                        warnings.Add(chainCheckResult);
                }
            }
            return String.Join("\r\n", warnings);
        }

        /// <summary>
        /// Рекурсивный проход по агрегатам входящего склада
        /// </summary>
        public string TestStockChain(IEnumerable<StockLink> stockLinks, StockLink stockLink, DateTimeOffset date)
        {
            return null;
            // ресурсные группы, которые связанны с "проблемной" ресурсной группой
            var linkedResourceGroups = stockLinks.Where(_
                => _.ResourceGroupId == stockLink.ResourceGroupId &&
                   _.Id != stockLink.Id &&
                   _.Type == GetInvertedLinkType(stockLink.Type));

            // для каждой связки проверим склад
            foreach (var linkedResourceGroup in linkedResourceGroups)
            {
                var stockBalance = _dashDBContext.StockBalances.SingleOrDefault(_ => _.Date == date && _.StockId == linkedResourceGroup.StockId);
                if (stockBalance == null)
                    return null;

                if (stockLink.Type == StockLinkType.In) // у нас кончились запасы - проверим нагрузку группы агрегатов, которые в этот склад отгружают
                {

                }
                else
                {

                }
            }

            static StockLinkType GetInvertedLinkType(StockLinkType stockLinkType) =>
                stockLinkType == StockLinkType.In ? StockLinkType.Out : StockLinkType.In;
        }

        /// <summary>
        /// Генерит рекомендации по КПЭ
        /// </summary>
        /// <param name="kpi">КПЭ</param>
        /// <returns>рекомендации</returns>
        private IEnumerable<KPIRecomendation> GetKPIRecommendationsAsync(Models.DB.KPI kpi)
        {
            // todo: выдаём рекомендации только по цехам\агрегатам\складам, т.к. других данных у нас нет
            if (kpi.KPIType != KPIType.ResourceWorkload)
                return Enumerable.Empty<KPIRecomendation>();

            return new KPIRecomendation[] {
                new KPIRecomendation() { Text = "Часть оборудования простаивает, можно запланировать производство" } };
        }

    }
}

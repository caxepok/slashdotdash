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
        private const string ColorRed = "#EB3216";
        private const string ColorOrange = "#FF9966";
        private const string ColorGreen = "#67AE19";

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

                // посчитаем процент отклонения от порогового значения
                // и в зависимости от этого покрасим кружок КПЭ
                decimal todayValue = values[^1].Value;
                string color;
                if (kpi.ThresholdDirection)
                {
                    if (todayValue >= kpi.Threshold)
                        color = ColorGreen;
                    else
                        color = ColorByDeviation(todayValue, kpi.Threshold);
                }
                else
                {
                    if (todayValue <= kpi.Threshold)
                        color = ColorGreen;
                    else
                        color = ColorByDeviation(todayValue, kpi.Threshold);
                }

                var apiKPI = new Models.API.KPI()
                {
                    Id = kpi.Id,
                    Name = kpi.Name,
                    Type = kpi.KPIType,
                    Threshold = kpi.Threshold,
                    Color = color,
                    Values = values,
                    TodayValue = todayValue,
                    YesterdayValue = values[^2].Value    // todo: проверка на длину массива, без данных тут всё свалится
                };
                kpiInfos.Add(apiKPI);

                IEnumerable<KPIRecomendation> recomendations = GetKPIRecommendationsAsync(kpi);
                if (recomendations.Any())
                    apiKPI.HaveRecomendations = true;
            }
            return kpiInfos;

            // возвращает цвет КПЭ в зависимости от величины отклонения от порогового значения
            static string ColorByDeviation(decimal value, decimal threshold)
            {
                decimal deviation = Math.Abs(((value / threshold) - 1) * 100);
                if (deviation <= 5)
                    return ColorOrange;
                else
                    return ColorRed;
            }
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
                        vad.Warning = ""; // todo: проверка цепочки и тут, но по-другому
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
                // считаем по группам агрегатов
                foreach (var resourceGroup in shop.ResourceGroups)
                {
                    List<PlanSummaryNode> nodesRes = new();
                    // среднее для группы
                    var averageRg = days.Where(_ => _.ResourceGroupId == resourceGroup.Id)
                                    .Select(_ => _.OccupiedPercent)
                                    .Average();
                    // по дням для группы
                    var perDayRg = days.Where(_ => _.ResourceGroupId == resourceGroup.Id).GroupBy(_ => _.Day)
                        .Select(g => new ValueOnDate(plan.Date.AddDays(g.Key), g.Average(a => a.OccupiedPercent))).ToList();
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
                        PlanSummaryNode nodeRes = new(resource.Id, resource.Name, String.Empty, perDayRes);
                        nodesRes.Add(nodeRes);
                    }
                    PlanSummaryNode nodeRg = new(resourceGroup.Id, resourceGroup.Name, String.Empty, perDayRg);
                    nodeRg.Childs = nodesRes;
                    nodesRg.Add(nodeRg);
                }
                PlanSummaryNode nodeSh = new(shop.Id, shop.Name, String.Empty, perDaySh);
                nodeSh.Childs = nodesRg;
                nodesSh.Add(nodeSh);
            }
            // после того как мы посчитали всё, мы можем проанализировать связки со складами
            var nodesRgFull = nodesSh.SelectMany(_ => _.Childs).ToList();
            foreach (var nodeRg in nodesRgFull)
            {
                foreach (var pdrg in nodeRg.Values)
                {
                    // за каждый день для группы мы можем проанализировать цепочку складов
                    if (pdrg.Value < 85)
                    {
                        pdrg.Warning = AnalyzeResourceGroup(nodeRg.Id, pdrg.Date, nodesRgFull);
                    }
                    if (pdrg.Value > 100)
                    {
                        pdrg.Warning = "Группа агрегатов не справится с объёмом";
                    }
                }
            }
            return nodesSh;
        }

        public IEnumerable<PlanCompareNode> GetPlanCompare(Plan srcPlan, Plan dstPlan)
        {
            var planSrc = GetPlanSummary(srcPlan);
            var planDst = GetPlanSummary(dstPlan);

            List<PlanCompareNode> shopCompares = new();

            // перебираем цеха
            foreach (var shopSrc in planSrc)
            {
                List<PlanCompareNode> rgCompares = new();
                List<ValueOnDateWithCompare> shopCompareValues = new();
                var shopDst = planDst.SingleOrDefault(_ => _.Name == shopSrc.Name);
                // дни по цехам
                foreach (var shopSrcValue in shopSrc.Values)
                {
                    ValueOnDateWithCompare shopCompareValue = new ValueOnDateWithCompare(shopSrcValue.Date, shopSrcValue.Value);
                    shopCompareValues.Add(shopCompareValue);
                    // находим тот же день в другом плане по цеху
                    var shopDstValue = shopDst.Values.SingleOrDefault(_ => _.Date == shopSrcValue.Date);
                    if (shopDstValue != null)
                        shopCompareValue.SetDestValue(shopDstValue.Value);
                }                    // перебираем группы агрегатов
                foreach (var rgSrc in shopSrc.Childs)
                {
                    List<PlanCompareNode> resCompares = new();
                    List<ValueOnDateWithCompare> rgCompareValues = new();
                    var rgDst = shopDst.Childs.SingleOrDefault(_ => _.Name == rgSrc.Name);
                    // дни по группам агрегатов
                    foreach (var rgSrcValue in rgSrc.Values)
                    {
                        ValueOnDateWithCompare rgCompareValue = new ValueOnDateWithCompare(rgSrcValue.Date, rgSrcValue.Value);
                        rgCompareValues.Add(rgCompareValue);
                        // находим тот же день в группе агрегатов в цеху
                        var rgDstValue = rgDst.Values.SingleOrDefault(_ => _.Date == rgSrcValue.Date);
                        if (rgDstValue != null)
                            rgCompareValue.SetDestValue(rgDstValue.Value);
                    }
                    // перебираем агрегаты
                    foreach (var resSrc in rgSrc.Childs)
                    {
                        List<ValueOnDateWithCompare> resCompareValues = new();
                        var resDst = rgDst.Childs.SingleOrDefault(_ => _.Name == resSrc.Name);
                        // дни по агрегатам
                        foreach (var resSrcValue in resSrc.Values)
                        {
                            ValueOnDateWithCompare resCompareValue = new ValueOnDateWithCompare(resSrcValue.Date, resSrcValue.Value);
                            resCompareValues.Add(resCompareValue);
                            // находим тот же день в агрегатах
                            var resDstValue = resDst.Values.SingleOrDefault(_ => _.Date == resSrcValue.Date);
                            if (resDstValue != null)
                                resCompareValue.SetDestValue(resDstValue.Value);
                        }
                        var resCompare = new PlanCompareNode(resSrc.Id, resSrc.Name, resCompareValues);
                        resCompares.Add(resCompare);

                    }
                    var rgCompare = new PlanCompareNode(rgSrc.Id, rgSrc.Name, rgCompareValues);
                    rgCompare.Childs = resCompares;
                    rgCompares.Add(rgCompare);
                }
                var shopCompare = new PlanCompareNode(shopSrc.Id, shopSrc.Name, shopCompareValues);
                shopCompare.Childs = rgCompares;
                shopCompares.Add(shopCompare);
            }

            return shopCompares;
        }

        /// <summary>
        /// Анализ цепочек складов ресурсной группы
        /// </summary>
        private string AnalyzeResourceGroup(int resourceGroupId, DateTimeOffset date, List<PlanSummaryNode> nodesRg)
        {
            List<string> warnings = new();
            // проверим склады
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
                    var chainCheckResult = TestStockChain(stockLinks, stockLink, date, nodesRg);
                    if (chainCheckResult != null)
                        warnings.Add(chainCheckResult);
                }
                if (stockLink.Type == StockLinkType.Out && sb.PlannedBalance >= sb.MaxBalance && sb.AllowedOverload)
                {
                    warnings.Add($"Исходящий склад ({stockLink.Stock.Name}) заполнен, нет места.");
                    // проверим цепочку
                    var chainCheckResult = TestStockChain(stockLinks, stockLink, date, nodesRg);
                    if (chainCheckResult != null)
                        warnings.Add(chainCheckResult);
                }
            }
            // todo: проверим равномерность загрузки агрегатов


            if (!warnings.Any())
                return null;

            return String.Join("\r\n", warnings);
        }

        /// <summary>
        /// Проход по агрегатам связанного склада
        /// </summary>
        private string TestStockChain(IEnumerable<StockLink> stockLinks, StockLink stockLink, DateTimeOffset date, List<PlanSummaryNode> nodesRg)
        {
            // ресурсные группы, которые связанны с "проблемной" ресурсной группой
            var linkedResourceGroups = stockLinks.Where(_
                => _.StockId == stockLink.StockId &&
                   _.Id != stockLink.Id &&
                   _.Type == GetInvertedLinkType(stockLink.Type));
            var stock = _dashDBContext.Stocks.Single(_ => _.Id == stockLink.StockId);
            // для каждой связки проверим склад
            foreach (var linkedResourceGroup in linkedResourceGroups)
            {
                var stockBalance = _dashDBContext.StockBalances.SingleOrDefault(_ => _.Date == date && _.StockId == linkedResourceGroup.StockId);
                if (stockBalance == null)
                    continue;

                if (stockLink.Type == StockLinkType.Out) // у нас кончились запасы - проверим нагрузку группы агрегатов, которые в этот склад отгружают
                {
                    var nodeRg = nodesRg.SingleOrDefault(_ => _.Id == stockLink.ResourceGroupId);
                    if (nodeRg == null)
                        continue;
                    if (nodeRg.Values.Single(_ => _.Date == date).Value > 99)    // агрегат, отгружающий на склад не справляется
                        return $"Агрегаты ({nodeRg.Name}) не успевают отгружать на склад  ({stock.Name}), они полностью загружены";
                }
                else // входящий склад перегружен, следущее оборудование не справляется
                {
                    var nodeRg = nodesRg.SingleOrDefault(_ => _.Id == stockLink.ResourceGroupId);
                    if (nodeRg == null)
                        continue;
                    if (nodeRg.Values.Single(_ => _.Date == date).Value > 99)    // агрегат, отгружающий на склад не справляется
                        return $"Агрегаты ({nodeRg.Name}) не успевают обрабатывать исходящую продукцию со склада ({stock.Name})";
                }
            }

            return null;

            static StockLinkType GetInvertedLinkType(StockLinkType stockLinkType) =>
                stockLinkType == StockLinkType.In ? StockLinkType.Out : StockLinkType.In;
        }

        /// <summary>
        /// Генерит верхнеуровневые рекомендации по КПЭ
        /// </summary>
        /// <param name="kpi">КПЭ</param>
        /// <returns>рекомендации</returns>
        private static IEnumerable<KPIRecomendation> GetKPIRecommendationsAsync(Models.DB.KPI kpi)
        {
            // todo: tbd
            // todo: выдаём рекомендации только по цехам\агрегатам\складам, т.к. других данных у нас нет
            if (kpi.KPIType == KPIType.ResourceWorkload)
                return Enumerable.Empty<KPIRecomendation>();

            return new KPIRecomendation[] {
                new KPIRecomendation() { Text = "Часть оборудования простаивает, можно запланировать производство" } };
        }
    }
}

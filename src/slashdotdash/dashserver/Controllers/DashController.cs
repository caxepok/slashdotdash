using dashserver.Infrastructure;
using dashserver.Models;
using dashserver.Models.API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace dashserver.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DashController : ControllerBase
    {
        private const int DepthDays = 14;    // количество дней, на сколько нужно проводить анализ

        private readonly ILogger _logger;
        private readonly DashDBContext _dashDBContext;

        public DashController(ILogger<DashController> logger, DashDBContext dashDBContext)
        {
            _logger = logger;
            _dashDBContext = dashDBContext;
        }

        /// <summary>
        /// Возвращает основные КПЭ
        /// </summary>
        [HttpGet("kpi")]
        public IActionResult GetKPIAsync()
        {
            List<Models.API.KPI> kpiInfos = new();
            foreach (var kpi in _dashDBContext.KPIs.ToList())
            {
                // просто вытаскиваем заранее сохранённые КПЭ из базы
                var values = _dashDBContext.KPIRecords
                    .Where(_ => _.KPIId == kpi.Id)
                    .OrderBy(_ => _.Date)
                    .Select(_ => new ValueOnDate()
                    {
                        Date = _.Date,
                        Value = _.Value
                    }).ToArray();
                
                var apiKPI = new Models.API.KPI()
                {
                    Id = kpi.Id,
                    Type = kpi.KPIType,
                    Threshold = kpi.Threshold,
                    Values = values,
                    TodayValue = values[values.Length - 1].Value,
                    YesterdayValue = values[values.Length - 2].Value    // todo: проверка на длину массива
                };
                kpiInfos.Add(apiKPI);
            }
            return Ok(kpiInfos);
        }

        /// <summary>
        /// Возвращает среднюю нагрузку по цеху
        /// </summary>
        /// <param name="planDate">дата плана</param>
        /// <returns>нагрузки по цехам</returns>
        [HttpGet("shop")]
        public IActionResult GetShopKPI(DateTimeOffset planDate)
        {
            Plan plan = _dashDBContext.Plans.SingleOrDefault(_ => _.Date == planDate);
            if (plan == null)
                return NotFound($"Plan not found for date: {planDate}");

            var shops = _dashDBContext.Shops.Include(_ => _.ResourceGroups).ToList();
            List<ShopAverageOccupiedPercent> shopAverageOccupiedPercents = new(shops.Count);
            foreach (Shop shop in shops)
            {
                var occupiedPercent = _dashDBContext.PlanDays
                    .Where(_ => _.PlanId == plan.Id && _.Day <= 14)
                    .OrderBy(_ => _.Day).ToList()   // ToList т.к. следующее выражение не ложится EF`ом на SQL
                    .Where(_ => shop.ResourceGroups.Any(rg => rg.Id == _.ResourceGroupId))
                    .Select(_ => _.OccupiedPercent)
                    .Average();
                shopAverageOccupiedPercents.Add(new ShopAverageOccupiedPercent()
                {
                    Name = shop.Name,
                    ShopId = shop.Id,
                    Value = Math.Round(occupiedPercent, 2)
                });
            }

            return Ok(shopAverageOccupiedPercents);
        }

        /// <summary>
        /// Возвращает среднюю нагрузку по ресурсным группам агрегатов
        /// </summary>
        /// <param name="planDate">дата плана</param>
        /// <param name="shopId">идентификатор цеха</param>
        /// <returns>нагрузки по группам агрегатов</returns>
        [HttpGet("shop/resourceGroup")]
        public IActionResult GetResourceGroupKPI(DateTimeOffset planDate, int shopId)
        {
            Plan plan = _dashDBContext.Plans.SingleOrDefault(_ => _.Date == planDate);
            if (plan == null)
                return NotFound($"Plan not found for date: {planDate}");

            var shop = _dashDBContext.Shops.Include(_ => _.ResourceGroups).SingleOrDefault(_ => _.Id == shopId);
            List<ResourceGroupOccupiedPercent> resourceGroupOccupiedPercents = new(shop.ResourceGroups.Count);
            foreach (var resourceGroup in shop.ResourceGroups)
            {
                var days = _dashDBContext.PlanDays
                    .Where(_ => _.ResourceGroupId == resourceGroup.Id && _.PlanId == plan.Id && _.Day <= 14)
                    .OrderBy(_ => _.Day)
                    .ToList();

                decimal averageOccupied = days.Average(_ => _.OccupiedPercent);
                var perDayGroups = days.GroupBy(_ => _.Day);
                List<ValueOnDate> valuesPerDate = new List<ValueOnDate>();
                var rgop = new ResourceGroupOccupiedPercent()
                {
                    ResourceGroupId = resourceGroup.Id,
                    AverageOccupation = averageOccupied,
                    PerDay = valuesPerDate
                };
                foreach (var perDayGroup in perDayGroups)
                {
                    var day = planDate.AddDays(perDayGroup.Key);
                    var vad = new ValueOnDate()
                    {
                        Date = day,
                        Value = perDayGroup.Average(a => a.OccupiedPercent)
                    };
                    valuesPerDate.Add(vad);
                    if (vad.Value < rgop.Threshold)
                    {
                        var stockLinks = _dashDBContext.StockLinks.Include(_ => _.Stock).Where(_ => _.ResourceGroupId == resourceGroup.Id).ToList();
                        foreach (var stockLink in stockLinks)
                        {
                            StockBalance sb = _dashDBContext.StockBalances.SingleOrDefault(_ => _.Date == day && _.StockId == stockLink.StockId);
                            if (sb == null)
                            {
                                _logger.LogWarning($"Stock balance not found on date: {day}");
                                continue;
                            }
                            if (stockLink.Type == Enums.StockLinkType.In)
                            {
                                // проверяем входящий склад
                                if (sb.PlannedBalance <= 0)
                                    vad.Warning = $"На входящем складе ({stockLink.Stock.Name}) нет материалов";
                            }
                            else
                            {
                                // проверяем исходящий склад, что он переполнен и на нём не разрешено переполнение
                                if (sb.PlannedBalance > sb.MaxBalance && !sb.AllowedOverload)
                                    vad.Warning = $"На исходящем складе ({stockLink.Stock.Name}) нет места";
                            }
                        }
                    }

                    else if(vad.Value > rgop.Threshold)
                    {
                        vad.Warning = "Оборудование не справится с объёмом";
                    }
                }
                resourceGroupOccupiedPercents.Add(rgop);
            }
            return Ok(resourceGroupOccupiedPercents);
        }

        /// <summary>
        /// Возвращает среднюю нагрузку по агрегатам
        /// </summary>
        /// <param name="planDate">дата плана</param>
        /// <param name="resourceGroupId">идентификатор группы агрегатов</param>
        /// <returns>нагрузки по цехам</returns>
        [HttpGet("shop/resource")]
        public IActionResult GetResourceKPI(DateTimeOffset planDate, int resourceGroupId)
        {
            Plan plan = _dashDBContext.Plans.SingleOrDefault(_ => _.Date == planDate);
            if (plan == null)
                return NotFound($"Plan not found for date: {planDate}");

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
                    .Select(g => new ValueOnDate()
                    {
                        Date = planDate.AddDays(g.Key),
                        Value = g.Average(a => a.OccupiedPercent)
                    });

                var rop = new ResourceOccupiedPercent()
                {
                    ResourceId = resource.Id,
                    AverageOccupation = averageOccupied,
                    PerDay = perDay
                };
                resourceOccupiedPercents.Add(rop);
            }
            return Ok(resourceOccupiedPercents);
        }

        /// <summary>
        /// Возвращает среднюю нагрузку по агрегатам
        /// </summary>
        /// <param name="planDate">дата плана</param>
        /// <param name="resourceGroupId">идентификатор группы агрегатов</param>
        /// <returns>нагрузки по цехам</returns>
        [HttpGet("stock")]
        public IActionResult GetStockKPI(DateTimeOffset planDate)
        {
            return Ok();
            //Plan plan = _dashDBContext.Plans.SingleOrDefault(_ => _.Date == planDate);
            //if (plan == null)
            //    return NotFound($"Plan not found for date: {planDate}");

            //var stockBalances = _dashDBContext.StockBalances.Where(_ => _.Date <= planDate.AddDays(DepthDays)).OrderBy(_ => _.Date).ToList();
            
            //return Ok(resourceOccupiedPercents);
        }
    }
}

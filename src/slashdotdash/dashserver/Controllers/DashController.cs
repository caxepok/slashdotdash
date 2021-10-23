using dashserver.Infrastructure;
using dashserver.Models.API;
using dashserver.Models.DB;
using dashserver.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dashserver.Controllers
{
    /// <summary>
    /// Контроллер для фронта отдающий данные
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class DashController : ControllerBase
    {
        private const int DepthDays = 14;    // количество дней, на сколько нужно проводить анализ

        private readonly ILogger _logger;
        private readonly DashDBContext _dashDBContext;
        private readonly IAnalysisService _analysisService;

        public DashController(ILogger<DashController> logger, DashDBContext dashDBContext, IAnalysisService analysisService)
        {
            _logger = logger;
            _dashDBContext = dashDBContext;
            _analysisService = analysisService;
        }

        /// <summary>
        /// Возвращает основные КПЭ
        /// </summary>
        [HttpGet("kpi")]
        public async Task<IActionResult> GetKPIAsync()
        {
            var results = _analysisService.GetKPIInfo();
            return Ok(results);
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

            var results = _analysisService.GetShopAnalysis(plan);
            return Ok(results);
        }

        [HttpGet("plan")]
        public IActionResult GetPlanSummary(DateTimeOffset planDate)
        {
            Plan plan = _dashDBContext.Plans.SingleOrDefault(_ => _.Date == planDate);
            if (plan == null)
                return NotFound($"Plan not found for date: {planDate}");

            var results = _analysisService.GetPlanSummary(plan);
            return Ok(results);
        }

        [HttpGet("plan/compare")]
        public IActionResult GetPlanCompare(DateTimeOffset srcPlanDate, DateTimeOffset dstPlanDate)
        {
            Plan srcPlan = _dashDBContext.Plans.SingleOrDefault(_ => _.Date == srcPlanDate);
            if (srcPlan == null)
                return NotFound($"Plan not found for date: {srcPlanDate}");
            Plan dstPlan = _dashDBContext.Plans.SingleOrDefault(_ => _.Date == dstPlanDate);
            if (dstPlan == null)
                return NotFound($"Plan not found for date: {dstPlanDate}");

            var results = _analysisService.GetPlanCompare(srcPlan, dstPlan);
            return Ok(results);
        }

        /// <summary>
        /// Возвращает среднюю нагрузку по ресурсным группам агрегатов
        /// </summary>
        /// <param name="planDate">дата плана</param>
        /// <param name="shopId">идентификатор цеха</param>
        /// <returns>нагрузки по группам агрегатов</returns>
        [HttpGet("shop/resourceGroup")]
        public async Task<IActionResult> GetResourceGroupKPI(DateTimeOffset planDate, int shopId)
        {
            Plan plan = _dashDBContext.Plans.SingleOrDefault(_ => _.Date == planDate);
            if (plan == null)
                return NotFound($"Plan not found for date: {planDate}");

            var results = _analysisService.GetResourceGroupRecomendations(plan, shopId);
            return Ok(results);
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

            var results = _analysisService.GetResourceOccupiedPercent(plan, resourceGroupId);
            return Ok(results);
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
        }
    }
}

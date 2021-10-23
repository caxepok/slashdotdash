using dashserver.Models.API;
using dashserver.Models.DB;
using System.Collections.Generic;

namespace dashserver.Services.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса аналитики
    /// </summary>
    public interface IAnalysisService
    {
        /// <summary>
        /// Возвращает данные КПЭ
        /// </summary>
        IEnumerable<Models.API.KPI> GetKPIInfo();
        /// <summary>
        /// Возвращает данные плана по группе агрегатов
        /// </summary>
        /// <param name="plan">план</param>
        /// <param name="shopId">цех</param>
        IEnumerable<ResourceGroupOccupiedPercent> GetResourceGroupRecomendations(Plan plan, int shopId);
        /// <summary>
        /// Возвращает данные по агрегатам
        /// </summary>
        /// <param name="plan">план</param>
        /// <param name="resourceGroupId">идентификатор группы агрегатов</param>
        IEnumerable<ResourceOccupiedPercent> GetResourceOccupiedPercent(Plan plan, int resourceGroupId);
        /// <summary>
        /// Возвращает данные по цехам
        /// </summary>
        /// <param name="plan">план</param>
        IEnumerable<ShopAverageOccupiedPercent> GetShopAnalysis(Plan plan);
        /// <summary>
        /// Возаращает данные для сводной таблицы плана
        /// </summary>
        /// <param name="plan">план</param>
        /// <returns></returns>
        IEnumerable<PlanSummaryNode> GetPlanSummary(Plan plan);
        /// <summary>
        /// Возвращает сранение двух планов
        /// </summary>
        /// <param name="srcPlan">исходный план</param>
        /// <param name="dstPlan">план для сравнения</param>
        /// <returns></returns>
        IEnumerable<PlanCompareNode> GetPlanCompare(Plan srcPlan, Plan dstPlan);
        /// <summary>
        /// Возвращает данные по загрузке складов
        /// </summary>
        /// <param name="plan">план</param>
        /// <returns></returns>
        IEnumerable<StockSummaryItem> GetStockSummary(Plan plan);
    }
}

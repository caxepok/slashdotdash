using System;

namespace dashserver.Models.DB
{
    /// <summary>
    /// Остатки по складу
    /// </summary>
    public class StockBalance
    {
        public int Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public decimal PlannedBalance { get; set; }
        public decimal MaxBalance { get; set; }
        public bool AllowedOverload { get; set; }

        public int StockId { get; set; }
        public virtual Stock Stock { get; set; }
        public int PlanId { get; set; }
        public virtual Plan Plan { get; set; }

        public StockBalance(int planId, int stockId, DateTimeOffset date, decimal plannedBalance, decimal maxBalance, bool allowedOverload)
        {
            PlanId = planId;
            StockId = stockId;
            Date = date;
            PlannedBalance = plannedBalance;
            MaxBalance = maxBalance;
            AllowedOverload = allowedOverload;
        }
    }
}

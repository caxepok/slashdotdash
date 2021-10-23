using System;

namespace dashserver.Models.DB
{
    /// <summary>
    /// Плановый день
    /// </summary>
    public class PlanDay
    {
        public int Id { get; set; }
        public int PlanId { get; set; }
        public int ResourceId { get; set; }
        public int ResourceGroupId { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public int Day { get; set; }
        public int DurationInDays { get; set; }
        public decimal OccupiedPercent { get; set; }
        public decimal UnavailablePercent { get; set; }

        public PlanDay(int planId, int resourceId, int resourceGroupId, DateTimeOffset startDate, int day, int durationInDays, decimal occupiedPercent, decimal unavailablePercent)
        {
            PlanId = planId;
            ResourceId = resourceId;
            ResourceGroupId = resourceGroupId;
            StartDate = startDate;
            Day = day;
            DurationInDays = durationInDays;
            OccupiedPercent = occupiedPercent;
            UnavailablePercent = unavailablePercent;
        }
    }
}

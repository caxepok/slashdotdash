using System.Collections.Generic;

namespace dashserver.Models.API
{
    public class ResourceGroupOccupiedPercent
    {
        public int ResourceGroupId { get; set; }
        public decimal AverageOccupation { get; set; }
        public decimal Threshold { get; set; } = 90;
        public IEnumerable<ValueOnDate> PerDay { get; set; }
    }
}

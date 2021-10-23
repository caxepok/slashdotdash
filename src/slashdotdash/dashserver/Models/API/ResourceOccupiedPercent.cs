using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dashserver.Models.API
{
    public class ResourceOccupiedPercent
    {
        public int ResourceId { get; set; }
        public decimal AverageOccupation { get; set; }
        public decimal Threshold { get; set; } = 90;
        public IEnumerable<ValueOnDate> PerDay { get; set; }
    }
}

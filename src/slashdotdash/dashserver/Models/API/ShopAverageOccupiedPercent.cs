using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dashserver.Models.API
{
    public class ShopAverageOccupiedPercent
    {
        public int ShopId { get; set; }
        public string Name { get; set; }
        public decimal Threshold { get; set; } = 90;
        public decimal Value { get; set; }
    }
}

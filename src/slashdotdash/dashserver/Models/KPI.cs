using dashserver.Enums;
using System.Collections.Generic;

namespace dashserver.Models
{
    /// <summary>
    /// КПЭ
    /// </summary>
    public class KPI
    {
        public int Id { get; set; }
        public KPIType KPIType { get; set; }
        public string Name { get; set; }
        public decimal Threshold { get; set; }

        public virtual ICollection<KPI> KPIRecords { get; set; }
    }
}

using dashserver.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dashserver.Models
{
    /// <summary>
    /// Запись КПЭ за определённый день
    /// </summary>
    public class KPIRecord
    {
        public int Id { get; set; }
        public int KPIId {get;set;}
        public DateTimeOffset Date { get; set; }
        public decimal Value { get; set; }

        public virtual KPI KPI { get; set; }
    }
}

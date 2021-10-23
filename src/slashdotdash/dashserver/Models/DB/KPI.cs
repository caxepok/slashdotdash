using dashserver.Enums;
using System.Collections.Generic;

namespace dashserver.Models.DB
{
    /// <summary>
    /// КПЭ
    /// </summary>
    public class KPI
    {
        /// <summary>
        /// Идентификатор КПЭ
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Тип КПЭ
        /// </summary>
        public KPIType KPIType { get; set; }
        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Граничное занчение
        /// </summary>
        public decimal Threshold { get; set; }
        /// <summary>
        /// Направление КПЭ true - должно быть больше целевого, false - должно быть меньше целевого
        /// </summary>
        public bool ThresholdDirection { get; set; }

        public virtual ICollection<KPI> KPIRecords { get; set; }
    }
}

using System;

namespace dashserver.Models.DB
{
    /// <summary>
    /// Запись КПЭ за определённый день
    /// </summary>
    public class KPIRecord
    {
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Идентификатор КПЭ
        /// </summary>
        public int KPIId { get; set; }
        /// <summary>
        /// Дата
        /// </summary>
        public DateTimeOffset Date { get; set; }
        /// <summary>
        /// Значение
        /// </summary>
        public decimal Value { get; set; }

        public virtual KPI KPI { get; set; }
    }
}

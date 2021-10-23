using dashserver.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dashserver.Models.API
{
    /// <summary>
    /// Значение КПЭ на заданный день
    /// </summary>
    public class KPI
    {
        /// <summary>
        /// Идентификатор КПЭ
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Навзвание КПЭ
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Тип КПЭ
        /// </summary>
        public KPIType Type { get; set; }
        /// <summary>
        /// Текущее значение КПЭ
        /// </summary>
        public decimal TodayValue { get; set; }
        /// <summary>
        /// Вчерашнее значение КПЭ
        /// </summary>
        public decimal YesterdayValue { get; set; }
        /// <summary>
        /// Граниченое значение
        /// </summary>
        public decimal Threshold { get; set; }
        /// <summary>
        /// Значения КПЭ за предудущие дни
        /// </summary>
        public IEnumerable<ValueOnDate> Values { get; set; }
    }
}

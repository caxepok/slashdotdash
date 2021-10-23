using dashserver.Enums;
using System.Collections.Generic;

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
        /// <summary>
        /// Для КПЭ есть рекомендации
        /// </summary>
        public bool HaveRecomendations { get; set; }
        /// <summary>
        /// Для КПЭ есть предупреждения
        /// </summary>
        public bool HaveWarnings { get; set; }
        /// <summary>
        /// Цвет, в который нужно покрасить кружоок
        /// </summary>
        public string Color { get; set; }
    }
}

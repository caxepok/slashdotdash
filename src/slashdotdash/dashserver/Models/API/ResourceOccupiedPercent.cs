using System.Collections.Generic;

namespace dashserver.Models.API
{
    /// <summary>
    /// Сводные данные по загрузке агрегата
    /// </summary>
    public class ResourceOccupiedPercent
    {
        /// <summary>
        /// Идентификатор агрегата
        /// </summary>
        public int ResourceId { get; set; }
        /// <summary>
        /// Среднее значение загруженности за период
        /// </summary>
        public decimal AverageOccupation { get; set; }
        /// <summary>
        /// Пороговое значение
        /// </summary>
        public decimal Threshold { get; set; } = 90;
        /// <summary>
        /// Средняя загрузка агрегатов группы по дням
        /// </summary>
        public IEnumerable<ValueOnDate> PerDay { get; set; }
    }
}

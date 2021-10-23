using System.Collections.Generic;

namespace dashserver.Models.API
{
    /// <summary>
    /// Сводные данные по загрузке группы агрегатов
    /// </summary>
    public class ResourceGroupOccupiedPercent
    {
        /// <summary>
        /// Идентификатор группы агрегатов
        /// </summary>
        public int ResourceGroupId { get; set; }
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
        /// <summary>
        /// Предупреждение для группы агрегатов
        /// </summary>
        public string Warning { get; internal set; }
    }
}

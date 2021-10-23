using System;
using System.Collections.Generic;

namespace dashserver.Models.API
{
    /// <summary>
    /// Запись загрузки склада
    /// </summary>
    public class StockSummaryItem
    {
        /// <summary>
        /// Название склада
        /// </summary>
        public string StockName { get; set; }
        /// <summary>
        /// Код складаы
        /// </summary>
        public string StockCode { get; set; }
        /// <summary>
        /// Предупреждение по складу
        /// </summary>
        public string Warning { get; set; }
        /// <summary>
        /// Значения по дням
        /// </summary>
        public IEnumerable<ValueOnDate> Values { get; set; }
    }
}

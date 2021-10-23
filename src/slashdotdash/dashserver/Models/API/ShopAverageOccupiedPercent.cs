using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dashserver.Models.API
{
    /// <summary>
    /// Сводные данные по загрузке цеха
    /// </summary>
    public class ShopAverageOccupiedPercent
    {
        /// <summary>
        /// Идентификатор цеха
        /// </summary>
        public int ShopId { get; set; }
        /// <summary>
        /// Название цеха
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Пороговое значение
        /// </summary>
        public decimal Threshold { get; set; } = 90;
        /// <summary>
        /// Данные загрузки
        /// </summary>
        public decimal Value { get; set; }
    }
}

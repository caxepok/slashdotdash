using dashserver.Models.DB;
using System;
using System.Collections.Generic;

namespace dashserver.Models.API
{
    /// <summary>
    /// Данные плана на дату
    /// </summary>
    public class PlanData
    {
        /// <summary>
        /// Дата, на которую сформирован план
        /// </summary>
        public DateTimeOffset Date { get; set; }
        /// <summary>
        /// Цеха
        /// </summary>
        public IEnumerable<Shop> Shops { get; set; }
        /// <summary>
        /// Группы агрегатов
        /// </summary>
        public IEnumerable<ResourceGroup> ResourceGroups { get; set; }
        /// <summary>
        /// Агрегаты
        /// </summary>
        public IEnumerable<Resource> Resources { get; set; }
        /// <summary>
        /// Данные плана по дням
        /// </summary>
        public IEnumerable<PlanDay> PlanDays { get; set; }
    }
}

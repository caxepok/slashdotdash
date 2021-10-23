using dashserver.Enums;

namespace dashserver.Models.DB
{
    /// <summary>
    /// Связь склада с группой агрегатов
    /// </summary>
    public class StockLink
    {
        /// <summary>
        /// Идентификатор связи склада и группы агрегатов
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Идентификатор склада
        /// </summary>
        public int StockId { get; set; }
        /// <summary>
        /// Идентификатор группы агрегатов
        /// </summary>
        public int ResourceGroupId { get; set; }
        /// <summary>
        /// Тип связи
        /// </summary>
        public StockLinkType Type { get; set; }

        public virtual Stock Stock { get; set; }
        public virtual ResourceGroup ResourceGroup { get; set; }

        public StockLink(int resourceGroupId, int stockId, StockLinkType type)
        {
            ResourceGroupId = resourceGroupId;
            StockId = stockId;
            Type = type;
        }
    }
}

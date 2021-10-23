using dashserver.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dashserver.Models.API
{
    /// <summary>
    /// Данные по складам
    /// </summary>
    public class StockData
    {
        /// <summary>
        /// Дата, на которую загружены планы 
        /// </summary>
        public DateTimeOffset Date { get; set; }
        /// <summary>
        /// Список складов
        /// </summary>
        public IEnumerable<Stock> Stocks { get; set; }
        /// <summary>
        /// Балансы на складах
        /// </summary>
        public IEnumerable<StockBalance> StockBalances { get; set; }
    }
}

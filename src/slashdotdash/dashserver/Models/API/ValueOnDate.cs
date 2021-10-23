using System;

namespace dashserver.Models.API
{
    /// <summary>
    /// Значение показателя на дату
    /// </summary>
    public class ValueOnDate
    {
        /// <summary>
        /// Дата
        /// </summary>
        public DateTimeOffset Date { get; set; }
        /// <summary>
        /// Значение показателя
        /// </summary>
        public decimal Value { get; set; }
        /// <summary>
        /// Предупреждение, отображаемое пользователю
        /// </summary>
        public string Warning { get; set; }

        public ValueOnDate(DateTimeOffset date, decimal value)
        {
            Date = date;
            Value = Math.Round(value, 2);
        }
    }
}

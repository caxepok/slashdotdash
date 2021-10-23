using System;

namespace dashserver.Models.API
{
    /// <summary>
    /// Значения показателя при сравнении плана
    /// </summary>
    public class ValueOnDateWithCompare
    {
        public DateTimeOffset Date { get; set; }
        public decimal SorceValue { get; set; }
        public decimal? DestValue { get; set; }
        public decimal? Diff { get; set; }

        public ValueOnDateWithCompare(DateTimeOffset date, decimal sorceValue)
        {
            Date = date;
            SorceValue = sorceValue;
        }

        public void SetDestValue(decimal destValue)
        {
            DestValue = destValue;
            Diff = SorceValue - destValue;
        }
    }
}

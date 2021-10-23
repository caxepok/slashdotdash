using System;

namespace dashserver.Models.DB
{
    /// <summary>
    /// Срез плана на определённый день
    /// </summary>
    public class Plan
    {
        public int Id { get; set; }
        /// <summary>
        /// День плана
        /// </summary>
        public DateTimeOffset Date { get; set; }

        public Plan(DateTimeOffset date)
        {
            Date = date;
        }
    }
}

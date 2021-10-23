using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Globalization;
using System.IO;

namespace dashcsvloader
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var reader = new StreamReader("1.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<KPIRecord>();
            }
        }
    }
}

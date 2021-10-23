using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dashserver.Models
{
    /// <summary>
    /// Склад
    /// </summary>
    public class Stock
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public Stock(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}

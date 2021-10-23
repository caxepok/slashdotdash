using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dashserver.Models
{
    /// <summary>
    /// Цех
    /// </summary>
    public class Shop
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ResourceGroup> ResourceGroups { get; set; }

        public Shop(string  name)
        {
            Name = name;
        }
    }
}

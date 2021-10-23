using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dashserver.Models
{ 

    /// <summary>
    /// Ресурсная группа агрегатов
    /// </summary>
    public class ResourceGroup
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int ShopId { get; set; }

        public virtual Shop Shop { get; set; }
        public virtual ICollection<Resource> Resources { get; set; }

        public ResourceGroup(string code, string name, int shopId)
        {
            Code = code;
            Name = name;
            ShopId = shopId;
        }
    }
}

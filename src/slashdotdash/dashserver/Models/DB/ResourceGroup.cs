using System.Collections.Generic;

namespace dashserver.Models.DB
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

        public virtual ICollection<Resource> Resources { get; set; }

        public ResourceGroup(string code, string name, int shopId)
        {
            Code = code;
            Name = name;
            ShopId = shopId;
        }
    }
}

namespace dashserver.Models.DB
{
    /// <summary>
    /// Агрегат
    /// </summary>
    public class Resource
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int ResourceGroupId { get; set; }

        public Resource(string code, string name, int resourceGroupId)
        {
            Name = name;
            Code = code;
            ResourceGroupId = resourceGroupId;
        }
    }
}

namespace dashserver.Models.DB
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

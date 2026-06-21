namespace reflash_oficial.Models
{
    public class TemplatePriceModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string prices { get; set; }  // здесь хранятся ID через запятую, например "1,2,3"

        // Список ID из строки prices
        public List<int> Ids
        {
            get
            {
                if (string.IsNullOrEmpty(prices))
                    return new List<int>();

                return prices.Split(',')
                             .Select(id => int.Parse(id.Trim()))
                             .ToList();
            }
        }
    }
}

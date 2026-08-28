namespace reflash_oficial.Models
{
    public class TemplateAdditionalPriceModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string price_ids { get; set; }  // соответствует полю в БД

        // Список ID из строки price_ids
        public List<int> Ids
        {
            get
            {
                if (string.IsNullOrEmpty(price_ids))  // ✅ ИСПРАВЛЕНО
                    return new List<int>();

                return price_ids.Split(',')           // ✅ ИСПРАВЛЕНО
                               .Select(id => int.Parse(id.Trim()))
                               .ToList();
            }
        }
    }
}
namespace reflash_oficial.Models
{
    public class PriceModel
    {
        public int id { get; set; }
        public string name_ru { get; set; }
        public string name_eng { get; set; }
        public string name_ger { get; set; }
        public string base_price { get; set; }
        public string pro_price { get; set; }

        // Метод для получения имени на нужном языке
        public string GetName(string language)
        {
            return language switch
            {
                "en" => name_eng,
                "de" => name_ger,
                _ => name_ru
            };
        }
    }
}

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

        public string base_price_eng { get; set; }
        public string pro_price_eng { get; set; }

        public string base_price_ger { get; set; }
        public string pro_price_ger { get; set; }

        // ===== НОВЫЕ ПОЛЯ ДЛЯ ОПИСАНИЯ (КАК В КОНТРОЛЛЕРЕ) =====
        public string info_ru { get; set; }
        public string info_eng { get; set; }
        public string info_ger { get; set; }

        // Метод для получения названия на нужном языке
        public string GetName(string language)
        {
            return language switch
            {
                "en" => name_eng,
                "de" => name_ger,
                _ => name_ru
            };
        }

        // Метод для получения описания на нужном языке
        public string GetInfo(string language)
        {
            return language switch
            {
                "en" => info_eng,
                "de" => info_ger,
                _ => info_ru
            };
        }
    }
}
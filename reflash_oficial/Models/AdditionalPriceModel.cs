namespace reflash_oficial.Models
{
    public class AdditionalPriceModel
    {
        public int id { get; set; }
        public string name_ru { get; set; }
        public string name_eng { get; set; }
        public string name_ger { get; set; }
        public string price_rubl { get; set; }
        public string price_dolar { get; set; }
        public string price_euro { get; set; }
        public string info_ru { get; set; }
        public string info_eng { get; set; }
        public string info_ger { get; set; }
        public int sort_order { get; set; }

        public int price_controler { get; set; }

        public string free_price_ids { get; set; }  
        public string base_price_ids { get; set; } 
        public string pro_price_ids { get; set; }

        public int unselected_price_mode { get; set; }

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

        // Метод для получения цены на нужном языке
        public string GetPrice(string language)
        {
            return language switch
            {
                "en" => price_dolar,
                "de" => price_euro,
                _ => price_rubl
            };
        }

        // Метод для получения валюты
        public string GetCurrency(string language)
        {
            return language switch
            {
                "en" => "$",
                "de" => "€",
                _ => "₽"
            };
        }

        // Метод для получения информации на нужном языке
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

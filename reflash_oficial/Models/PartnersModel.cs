namespace reflash_oficial.Models
{
    public class PartnersModel
    {
        public int Id { get; set; }

        public string name { get; set; }

        // ===== КОНТАКТЫ =====
        public string phone { get; set; }          // Телефон
        public string vk { get; set; }             // ВКонтакте (ссылка)
        public string vk_group { get; set; }       // Группа ВК (ссылка)
        public string telegram { get; set; }       // Telegram
        public string whatsapp { get; set; }       // WhatsApp
        public string email { get; set; }          // Email
        public string website { get; set; }        // Сайт

        // ===== АДРЕС =====
        public string city { get; set; }
        public string address { get; set; }

        public string info { get; set; }

        // ===== КООРДИНАТЫ ДЛЯ КАРТЫ =====
        public string longitude { get; set; }
        public string latitude { get; set; }

        // ===== ФОТО =====
        public string photo { get; set; }

        public string PhotoUrl
        {
            get
            {
                if (string.IsNullOrEmpty(photo))
                    return "/images/default-partner.jpg";
                return $"/shared-fotos/partners/{photo}";
            }
        }
    }
}
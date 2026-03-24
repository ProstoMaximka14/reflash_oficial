namespace reflash_oficial.Models
{
    public class PartnersModel
    {
        public int Id { get; set; }

        public string name { get; set; }

        public string phone { get; set; }

        public string photo { get; set; }

        public string vk { get; set; }

        public string website { get; set; }

        public string city { get; set; }
       
        public string street { get; set; }
        public string house { get; set; }

        public string longitude { get; set; }
        public string latitude { get; set; }

        public string PhotoUrl
        {
            get
            {
                if (string.IsNullOrEmpty(photo))
                    return "/images/default-partner.jpg"; // Дефолтное изображение

                // Используем путь к общей папке
                return $"/shared-fotos/partners/{photo}";
            }
        }
    }
}

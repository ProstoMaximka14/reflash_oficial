namespace reflash_oficial.Models
{
    public class GraficModel
    {
        public int id { get; set; }

        public string Name { get; set; }

        public string image { get; set; }

        // ===== НОВЫЕ ПОЛЯ (КАК В КОНТРОЛЛЕРЕ) =====
        public string NameEng { get; set; }
        public string NameGer { get; set; }

        public string DescriptionRu { get; set; }
        public string DescriptionEng { get; set; }
        public string DescriptionGer { get; set; }

        // Метод для получения локализованного названия
        public string GetName(string language)
        {
            return language switch
            {
                "en" => NameEng,
                "de" => NameGer,
                _ => Name
            };
        }

        // Метод для получения локализованного описания
        public string GetDescription(string language)
        {
            return language switch
            {
                "en" => DescriptionEng,
                "de" => DescriptionGer,
                _ => DescriptionRu
            };
        }
    }
}
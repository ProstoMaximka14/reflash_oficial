namespace reflash_oficial.Models
{
    public class NewsModel
    {
        public int id { get; set; }

        public string photo_1 { get; set; }
        public string photo_2 { get; set; }

        public string name { get; set; }
        public string name_eng { get; set; }
        public string name_ger { get; set; }

        public string text { get; set; }
        public string text_eng { get; set; }
        public string text_ger { get; set; }

        public string date { get; set; }

        public string GetName(string language)
        {
            return language switch
            {
                "en" => !string.IsNullOrEmpty(name_eng) ? name_eng : name,
                "de" => !string.IsNullOrEmpty(name_ger) ? name_ger : name,
                _ => name
            };
        }

        public string GetText(string language)
        {
            return language switch
            {
                "en" => !string.IsNullOrEmpty(text_eng) ? text_eng : text,
                "de" => !string.IsNullOrEmpty(text_ger) ? text_ger : text,
                _ => text
            };
        }
    }
}

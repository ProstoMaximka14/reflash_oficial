namespace reflash_oficial.Models
{
    public class ReflashCarModel
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Generation { get; set; }
        public string Engine { get; set; }
        public string Image { get; set; }
        public string AboutRu { get; set; }
        public string AboutEng { get; set; }
        public string AboutGer { get; set; }
        public string ResultRu { get; set; }
        public string ResultEng { get; set; }
        public string ResultGer { get; set; }
        public string EngineControlRu { get; set; }
        public string EngineControlEng { get; set; }
        public string EngineControlGer { get; set; }
        public string OptionsRu { get; set; }
        public string OptionsEng { get; set; }
        public string OptionsGer { get; set; }

        // Только одно поле для цен (на русском)
        // Хранит строку с ID через запятую, например "1,2,3"
        public string PriceRu { get; set; }

        // Загруженные цены (заполняется в контроллере)
        public List<PriceModel> Prices { get; set; } = new List<PriceModel>();
    }
}

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
        
        public string ResultRu { get; set; }
        
        public string EngineControlRu { get; set; }
        
        public string PriceRu { get; set; }

        public List<AboutModel> Abouts { get; set; } = new List<AboutModel>();

        public List<ResultModel> PriResults { get; set; } = new List<ResultModel>();

        public List<EngineControlModel> Engine_controlers { get; set; } = new List<EngineControlModel>();

        public List<PriceModel> Prices { get; set; } = new List<PriceModel>();

    }
}

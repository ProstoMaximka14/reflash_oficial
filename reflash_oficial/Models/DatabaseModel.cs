namespace reflash_oficial.Models
{
    public class DatabaseModel
    {
        // ===== ОСНОВНЫЕ ДАННЫЕ =====
        public static List<ReflashCarModel> Cars = new List<ReflashCarModel>();
        public static List<PartnersModel> Partners = new List<PartnersModel>();
        public static List<NewsModel> News = new List<NewsModel>();
        public static FurstPageModel furst_page = new FurstPageModel();

        // ===== ДАННЫЕ ДЛЯ ДРОПДАУНОВ (НОВЫЕ) =====
        public static List<string> Brands = new List<string>();
        public static Dictionary<string, List<string>> Models = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> Generations = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> Engines = new Dictionary<string, List<string>>();
    }
}

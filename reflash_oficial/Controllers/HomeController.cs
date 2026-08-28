using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using reflash_oficial.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace reflash_oficial.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // ==========================================
        // ПОЛУЧЕНИЕ ДАННЫХ ИЗ БД
        // ==========================================

        private string GetConnectionString()
        {
            return _configuration.GetConnectionString("DefaultConnection")
                ?? "server=localhost;port=3306;database=reflash_cars;user=root;password=;";
        }

        private List<ReflashCarModel> GetCarsFromDatabase()
        {
            List<ReflashCarModel> cars = new List<ReflashCarModel>();
            string connectionString = GetConnectionString();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT id, brand, model, generation, engine, image, " +
                                   "about_ru, result_ru, engine_control_ru, price_ru, grafic, " +
                                   "additional_price_ru, old_url, sort_order " +
                                   "FROM reflash_cars ORDER BY brand, model, generation";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cars.Add(new ReflashCarModel
                                {
                                    Id = reader.GetInt32("id"),
                                    Brand = reader.IsDBNull(reader.GetOrdinal("brand")) ? "" : reader.GetString("brand"),
                                    Model = reader.IsDBNull(reader.GetOrdinal("model")) ? "" : reader.GetString("model"),
                                    Generation = reader.IsDBNull(reader.GetOrdinal("generation")) ? "" : reader.GetString("generation"),
                                    Engine = reader.IsDBNull(reader.GetOrdinal("engine")) ? "" : reader.GetString("engine"),
                                    Image = reader.IsDBNull(reader.GetOrdinal("image")) ? "" : reader.GetString("image"),
                                    AboutRu = reader.IsDBNull(reader.GetOrdinal("about_ru")) ? "" : reader.GetString("about_ru"),
                                    ResultRu = reader.IsDBNull(reader.GetOrdinal("result_ru")) ? "" : reader.GetString("result_ru"),
                                    EngineControlRu = reader.IsDBNull(reader.GetOrdinal("engine_control_ru")) ? "" : reader.GetString("engine_control_ru"),
                                    PriceRu = reader.IsDBNull(reader.GetOrdinal("price_ru")) ? "" : reader.GetString("price_ru"),
                                    grafic = reader.IsDBNull(reader.GetOrdinal("grafic")) ? "" : reader.GetString("grafic"),
                                    additional_price_ru = reader.IsDBNull(reader.GetOrdinal("additional_price_ru")) ? "" : reader.GetString("additional_price_ru"),
                                    old_url = reader.IsDBNull(reader.GetOrdinal("old_url")) ? "" : reader.GetString("old_url"),
                                    SortOrder = reader.IsDBNull(reader.GetOrdinal("sort_order")) ? 0 : reader.GetInt32("sort_order")
                                });
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                TempData["Error"] = $"Ошибка MySQL при загрузке автомобилей: {ex.Message}";
            }

            return cars;
        }

        private List<PartnersModel> GetPartnersFromDatabase()
        {
            List<PartnersModel> partners = new List<PartnersModel>();
            string connectionString = GetConnectionString();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM partners ORDER BY name";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var photoUrl = reader.IsDBNull(reader.GetOrdinal("photo_url")) ? "" : reader.GetString("photo_url");
                                string photoFileName = photoUrl;
                                if (!string.IsNullOrEmpty(photoUrl) && photoUrl.Contains("/"))
                                {
                                    photoFileName = Path.GetFileName(photoUrl);
                                }

                                partners.Add(new PartnersModel
                                {
                                    Id = reader.GetInt32("id"),
                                    name = reader.IsDBNull(reader.GetOrdinal("name")) ? "" : reader.GetString("name"),
                                    phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? "" : reader.GetString("phone"),
                                    vk = reader.IsDBNull(reader.GetOrdinal("vk_url")) ? "" : reader.GetString("vk_url"),
                                    vk_group = reader.IsDBNull(reader.GetOrdinal("vk_group_url")) ? "" : reader.GetString("vk_group_url"),
                                    telegram = reader.IsDBNull(reader.GetOrdinal("telegram")) ? "" : reader.GetString("telegram"),
                                    whatsapp = reader.IsDBNull(reader.GetOrdinal("whatsapp")) ? "" : reader.GetString("whatsapp"),
                                    email = reader.IsDBNull(reader.GetOrdinal("email")) ? "" : reader.GetString("email"),
                                    website = reader.IsDBNull(reader.GetOrdinal("website_url")) ? "" : reader.GetString("website_url"),
                                    city = reader.IsDBNull(reader.GetOrdinal("city")) ? "" : reader.GetString("city"),
                                    street = reader.IsDBNull(reader.GetOrdinal("street")) ? "" : reader.GetString("street"),
                                    house = reader.IsDBNull(reader.GetOrdinal("house")) ? "" : reader.GetString("house"),
                                    point_name = reader.IsDBNull(reader.GetOrdinal("point_name")) ? "" : reader.GetString("point_name"),
                                    longitude = reader.IsDBNull(reader.GetOrdinal("longitude")) ? "" : reader.GetString("longitude"),
                                    latitude = reader.IsDBNull(reader.GetOrdinal("latitude")) ? "" : reader.GetString("latitude"),
                                    photo = photoFileName
                                });
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                TempData["Error"] = $"Ошибка MySQL при загрузке партнеров: {ex.Message}";
            }
            return partners;
        }

        private FurstPageModel GetFurstPageFromDatabase()
        {
            FurstPageModel furst_page = null;
            string connectionString = GetConnectionString();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"SELECT SQL_NO_CACHE 
                        image_1, image_2, image_3, image_4,
                        head_1_ru, head_1_eng, head_1_ger,
                        head_2_ru, head_2_eng, head_2_ger,
                        text_ru, text_eng, text_ger,
                        block_1_ru, block_1_eng, block_1_ger,
                        block_1_title_ru, block_1_title_eng, block_1_title_ger,
                        block_2_ru, block_2_eng, block_2_ger,
                        block_2_title_ru, block_2_title_eng, block_2_title_ger,
                        block_3_ru, block_3_eng, block_3_ger,
                        block_3_title_ru, block_3_title_eng, block_3_title_ger,
                        block_4_ru, block_4_eng, block_4_ger,
                        block_4_title_ru, block_4_title_eng, block_4_title_ger,
                        block_5_ru, block_5_eng, block_5_ger,
                        block_5_title_ru, block_5_title_eng, block_5_title_ger,
                        block_6_ru, block_6_eng, block_6_ger,
                        block_6_title_ru, block_6_title_eng, block_6_title_ger
                    FROM first_page_content LIMIT 1";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                furst_page = new FurstPageModel
                                {
                                    // ===== ИЗОБРАЖЕНИЯ =====
                                    image_1 = reader.IsDBNull(reader.GetOrdinal("image_1")) ? "" : reader.GetString("image_1"),
                                    image_2 = reader.IsDBNull(reader.GetOrdinal("image_2")) ? "" : reader.GetString("image_2"),
                                    image_3 = reader.IsDBNull(reader.GetOrdinal("image_3")) ? "" : reader.GetString("image_3"),
                                    image_4 = reader.IsDBNull(reader.GetOrdinal("image_4")) ? "" : reader.GetString("image_4"),

                                    // ===== ЗАГОЛОВКИ =====
                                    head_1_ru = reader.IsDBNull(reader.GetOrdinal("head_1_ru")) ? "" : reader.GetString("head_1_ru"),
                                    head_1_eng = reader.IsDBNull(reader.GetOrdinal("head_1_eng")) ? "" : reader.GetString("head_1_eng"),
                                    head_1_ger = reader.IsDBNull(reader.GetOrdinal("head_1_ger")) ? "" : reader.GetString("head_1_ger"),
                                    head_2_ru = reader.IsDBNull(reader.GetOrdinal("head_2_ru")) ? "" : reader.GetString("head_2_ru"),
                                    head_2_eng = reader.IsDBNull(reader.GetOrdinal("head_2_eng")) ? "" : reader.GetString("head_2_eng"),
                                    head_2_ger = reader.IsDBNull(reader.GetOrdinal("head_2_ger")) ? "" : reader.GetString("head_2_ger"),

                                    // ===== ТЕКСТ =====
                                    text_ru = reader.IsDBNull(reader.GetOrdinal("text_ru")) ? "" : reader.GetString("text_ru"),
                                    text_eng = reader.IsDBNull(reader.GetOrdinal("text_eng")) ? "" : reader.GetString("text_eng"),
                                    text_ger = reader.IsDBNull(reader.GetOrdinal("text_ger")) ? "" : reader.GetString("text_ger"),

                                    // ===== БЛОК 1 =====
                                    block_1_ru = reader.IsDBNull(reader.GetOrdinal("block_1_ru")) ? "" : reader.GetString("block_1_ru"),
                                    block_1_eng = reader.IsDBNull(reader.GetOrdinal("block_1_eng")) ? "" : reader.GetString("block_1_eng"),
                                    block_1_ger = reader.IsDBNull(reader.GetOrdinal("block_1_ger")) ? "" : reader.GetString("block_1_ger"),
                                    block_1_title_ru = reader.IsDBNull(reader.GetOrdinal("block_1_title_ru")) ? "" : reader.GetString("block_1_title_ru"),
                                    block_1_title_eng = reader.IsDBNull(reader.GetOrdinal("block_1_title_eng")) ? "" : reader.GetString("block_1_title_eng"),
                                    block_1_title_ger = reader.IsDBNull(reader.GetOrdinal("block_1_title_ger")) ? "" : reader.GetString("block_1_title_ger"),

                                    // ===== БЛОК 2 =====
                                    block_2_ru = reader.IsDBNull(reader.GetOrdinal("block_2_ru")) ? "" : reader.GetString("block_2_ru"),
                                    block_2_eng = reader.IsDBNull(reader.GetOrdinal("block_2_eng")) ? "" : reader.GetString("block_2_eng"),
                                    block_2_ger = reader.IsDBNull(reader.GetOrdinal("block_2_ger")) ? "" : reader.GetString("block_2_ger"),
                                    block_2_title_ru = reader.IsDBNull(reader.GetOrdinal("block_2_title_ru")) ? "" : reader.GetString("block_2_title_ru"),
                                    block_2_title_eng = reader.IsDBNull(reader.GetOrdinal("block_2_title_eng")) ? "" : reader.GetString("block_2_title_eng"),
                                    block_2_title_ger = reader.IsDBNull(reader.GetOrdinal("block_2_title_ger")) ? "" : reader.GetString("block_2_title_ger"),

                                    // ===== БЛОК 3 =====
                                    block_3_ru = reader.IsDBNull(reader.GetOrdinal("block_3_ru")) ? "" : reader.GetString("block_3_ru"),
                                    block_3_eng = reader.IsDBNull(reader.GetOrdinal("block_3_eng")) ? "" : reader.GetString("block_3_eng"),
                                    block_3_ger = reader.IsDBNull(reader.GetOrdinal("block_3_ger")) ? "" : reader.GetString("block_3_ger"),
                                    block_3_title_ru = reader.IsDBNull(reader.GetOrdinal("block_3_title_ru")) ? "" : reader.GetString("block_3_title_ru"),
                                    block_3_title_eng = reader.IsDBNull(reader.GetOrdinal("block_3_title_eng")) ? "" : reader.GetString("block_3_title_eng"),
                                    block_3_title_ger = reader.IsDBNull(reader.GetOrdinal("block_3_title_ger")) ? "" : reader.GetString("block_3_title_ger"),

                                    // ===== БЛОК 4 =====
                                    block_4_ru = reader.IsDBNull(reader.GetOrdinal("block_4_ru")) ? "" : reader.GetString("block_4_ru"),
                                    block_4_eng = reader.IsDBNull(reader.GetOrdinal("block_4_eng")) ? "" : reader.GetString("block_4_eng"),
                                    block_4_ger = reader.IsDBNull(reader.GetOrdinal("block_4_ger")) ? "" : reader.GetString("block_4_ger"),
                                    block_4_title_ru = reader.IsDBNull(reader.GetOrdinal("block_4_title_ru")) ? "" : reader.GetString("block_4_title_ru"),
                                    block_4_title_eng = reader.IsDBNull(reader.GetOrdinal("block_4_title_eng")) ? "" : reader.GetString("block_4_title_eng"),
                                    block_4_title_ger = reader.IsDBNull(reader.GetOrdinal("block_4_title_ger")) ? "" : reader.GetString("block_4_title_ger"),

                                    // ===== БЛОК 5 =====
                                    block_5_ru = reader.IsDBNull(reader.GetOrdinal("block_5_ru")) ? "" : reader.GetString("block_5_ru"),
                                    block_5_eng = reader.IsDBNull(reader.GetOrdinal("block_5_eng")) ? "" : reader.GetString("block_5_eng"),
                                    block_5_ger = reader.IsDBNull(reader.GetOrdinal("block_5_ger")) ? "" : reader.GetString("block_5_ger"),
                                    block_5_title_ru = reader.IsDBNull(reader.GetOrdinal("block_5_title_ru")) ? "" : reader.GetString("block_5_title_ru"),
                                    block_5_title_eng = reader.IsDBNull(reader.GetOrdinal("block_5_title_eng")) ? "" : reader.GetString("block_5_title_eng"),
                                    block_5_title_ger = reader.IsDBNull(reader.GetOrdinal("block_5_title_ger")) ? "" : reader.GetString("block_5_title_ger"),

                                    // ===== БЛОК 6 =====
                                    block_6_ru = reader.IsDBNull(reader.GetOrdinal("block_6_ru")) ? "" : reader.GetString("block_6_ru"),
                                    block_6_eng = reader.IsDBNull(reader.GetOrdinal("block_6_eng")) ? "" : reader.GetString("block_6_eng"),
                                    block_6_ger = reader.IsDBNull(reader.GetOrdinal("block_6_ger")) ? "" : reader.GetString("block_6_ger"),
                                    block_6_title_ru = reader.IsDBNull(reader.GetOrdinal("block_6_title_ru")) ? "" : reader.GetString("block_6_title_ru"),
                                    block_6_title_eng = reader.IsDBNull(reader.GetOrdinal("block_6_title_eng")) ? "" : reader.GetString("block_6_title_eng"),
                                    block_6_title_ger = reader.IsDBNull(reader.GetOrdinal("block_6_title_ger")) ? "" : reader.GetString("block_6_title_ger")
                                };
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Ошибка MySQL при загрузке данных главной страницы: {ex.Message}");
            }

            return furst_page;
        }

        // ==========================================
        // ЗАГРУЗКА ДАННЫХ ДЛЯ ДРОПДАУНОВ
        // ==========================================

        private void LoadDropdownData()
        {
            var cars = DatabaseModel.Cars;
            if (cars == null || cars.Count == 0) return;

            // 1. Бренды
            DatabaseModel.Brands = cars
                .Select(c => c.Brand)
                .Distinct()
                .OrderBy(b => b)
                .ToList();

            // 2. Модели по бренду
            DatabaseModel.Models = cars
                .GroupBy(c => c.Brand)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(c => c.Model).Distinct().OrderBy(m => m).ToList()
                );

            // 3. Поколения по модели
            DatabaseModel.Generations = cars
        .GroupBy(c => c.Brand + "|" + c.Model)
        .ToDictionary(
            g => g.Key,
            g => g.Select(c => c.Generation)
                  .Distinct()
                  .OrderBy(gen => ParseGeneration(gen))  // ✅ Числовая сортировка
                  .ToList()
        );

            // 4. Двигатели по поколению
            DatabaseModel.Engines = cars
            .GroupBy(c => c.Brand + "|" + c.Model + "|" + c.Generation)
            .ToDictionary(
                g => g.Key,
                g => g
                    .OrderBy(c => c.SortOrder)  // ← СОРТИРУЕМ ПО SORT_ORDER!
                    .ThenBy(c => c.Engine)       // ← Затем по названию двигателя
                    .Select(c => c.Engine)
                    .Distinct()
                    .ToList()
            );
        }

        private int ParseGeneration(string generation)
        {
            if (string.IsNullOrEmpty(generation)) return 0;

            // Пробуем распарсить как число
            if (int.TryParse(generation, out int num))
                return num;

            // Если есть текст, пробуем извлечь число (например "2 поколение" → 2)
            var match = System.Text.RegularExpressions.Regex.Match(generation, @"\d+");
            if (match.Success && int.TryParse(match.Value, out int extracted))
                return extracted;

            // Если ничего не получилось — возвращаем 0
            return 0;
        }

        // ==========================================
        // МЕТОДЫ ДЛЯ ДРОПДАУНОВ
        // ==========================================

        private List<string> GetBrands()
        {
            // Получаем все бренды из кэша
            var allBrands = DatabaseModel.Brands ?? new List<string>();

            // Исключаем бренд "test" (регистронезависимо)
            return allBrands
                .Where(b => !b.Equals("test", StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        private List<string> GetModels(string brand)
        {
            if (string.IsNullOrEmpty(brand) || DatabaseModel.Models == null)
                return new List<string>();

            return DatabaseModel.Models.ContainsKey(brand)
                ? DatabaseModel.Models[brand]
                : new List<string>();
        }

        private List<string> GetGenerations(string brand, string model)
        {
            if (string.IsNullOrEmpty(brand) || string.IsNullOrEmpty(model) || DatabaseModel.Generations == null)
                return new List<string>();

            string key = brand + "|" + model;
            return DatabaseModel.Generations.ContainsKey(key)
                ? DatabaseModel.Generations[key]
                : new List<string>();
        }

        private List<string> GetEngines(string brand, string model, string generation)
        {
            if (string.IsNullOrEmpty(brand) || string.IsNullOrEmpty(model) || string.IsNullOrEmpty(generation) || DatabaseModel.Engines == null)
                return new List<string>();

            string key = brand + "|" + model + "|" + generation;
            return DatabaseModel.Engines.ContainsKey(key)
                ? DatabaseModel.Engines[key]
                : new List<string>();
        }

        // ==========================================
        // КОНТРОЛЛЕРЫ СТРАНИЦ
        // ==========================================

        public IActionResult Index()
        {
            if (DatabaseModel.Cars == null || DatabaseModel.Cars.Count == 0)
            {
                DatabaseModel.Cars = GetCarsFromDatabase();
                DatabaseModel.Partners = GetPartnersFromDatabase();
                DatabaseModel.furst_page = GetFurstPageFromDatabase();
                LoadDropdownData();
            }

            ViewBag.FurstPage = DatabaseModel.furst_page;
            return View(new ReflashCarModel());
        }

        [HttpGet]
        public IActionResult GetInitialData()
        {
            return Json(new SelectionResponse
            {
                Car = new ReflashCarModel(),
                NextField = "brand",
                Options = GetBrands()
            });
        }

        [HttpPost]
        public IActionResult ProcessSelection([FromBody] SelectionRequest request)
        {
            var updatedCar = new ReflashCarModel
            {
                Brand = request.Car.Brand,
                Model = request.Car.Model,
                Generation = request.Car.Generation,
                Engine = request.Car.Engine
            };

            switch (request.ChangedField)
            {
                case "brand":
                    updatedCar.Brand = request.NewValue;
                    updatedCar.Model = "";
                    updatedCar.Generation = "";
                    updatedCar.Engine = "";
                    break;

                case "model":
                    updatedCar.Model = request.NewValue;
                    updatedCar.Generation = "";
                    updatedCar.Engine = "";
                    break;

                case "generation":
                    updatedCar.Generation = request.NewValue;
                    updatedCar.Engine = "";
                    break;

                case "engine":
                    updatedCar.Engine = request.NewValue;
                    break;
            }

            string nextField = "";
            List<string> nextOptions = new List<string>();

            if (string.IsNullOrEmpty(updatedCar.Brand))
            {
                nextField = "brand";
                nextOptions = GetBrands();
            }
            else if (string.IsNullOrEmpty(updatedCar.Model))
            {
                nextField = "model";
                nextOptions = GetModels(updatedCar.Brand);
            }
            else if (string.IsNullOrEmpty(updatedCar.Generation))
            {
                nextField = "generation";
                nextOptions = GetGenerations(updatedCar.Brand, updatedCar.Model);
            }
            else if (string.IsNullOrEmpty(updatedCar.Engine))
            {
                nextField = "engine";
                nextOptions = GetEngines(updatedCar.Brand, updatedCar.Model, updatedCar.Generation);
            }
            else
            {
                nextField = "complete";
                nextOptions = new List<string>();
            }

            return Json(new SelectionResponse
            {
                Car = updatedCar,
                NextField = nextField,
                Options = nextOptions
            });
        }

        public async Task<IActionResult> Car(ReflashCarModel car)
        {
            // Декодируем параметры
            car.Brand = Uri.UnescapeDataString(car.Brand ?? "");
            car.Model = Uri.UnescapeDataString(car.Model ?? "");
            car.Generation = Uri.UnescapeDataString(car.Generation ?? "");
            car.Engine = Uri.UnescapeDataString(car.Engine ?? "");

            // Заменяем _ на пробелы
            car.Brand = car.Brand.Replace('_', ' ');
            car.Model = car.Model.Replace('_', ' ');
            car.Generation = car.Generation.Replace('_', ' ');
            car.Engine = car.Engine.Replace('_', ' ');

            ReflashCarModel neededCar = null;

            // ===== ПОИСК С УЧЕТОМ ТРАНСЛИТЕРАЦИИ =====
            foreach (var dbCar in DatabaseModel.Cars)
            {
                // Транслитерируем значения из БД для сравнения
                var dbBrand = TransliterateForUrl(dbCar.Brand).Replace("_", " ");
                var dbModel = TransliterateForUrl(dbCar.Model).Replace("_", " ");
                var dbGeneration = TransliterateForUrl(dbCar.Generation).Replace("_", " ");
                var dbEngine = TransliterateForUrl(dbCar.Engine).Replace("_", " ");

                // Сравниваем с транслитерированными значениями из URL
                if (dbBrand == car.Brand &&
                    dbModel == car.Model &&
                    dbGeneration == car.Generation &&
                    dbEngine == car.Engine)
                {
                    neededCar = dbCar;
                    break;
                }
            }

            // Если не нашли, пробуем найти с оригинальными значениями
            if (neededCar == null)
            {
                foreach (var dbCar in DatabaseModel.Cars)
                {
                    if (dbCar.Brand == car.Brand &&
                        dbCar.Model == car.Model &&
                        dbCar.Generation == car.Generation &&
                        dbCar.Engine == car.Engine)
                    {
                        neededCar = dbCar;
                        break;
                    }
                }
            }

            if (neededCar == null)
            {
                TempData["Error"] = "Автомобиль не найден";
                return RedirectToAction("Index");
            }

            // Сохраняем значения из URL
            neededCar.Brand = car.Brand;
            neededCar.Model = car.Model;
            neededCar.Generation = car.Generation;
            neededCar.Engine = car.Engine;

            // Загружаем связанные данные
            if (!string.IsNullOrEmpty(neededCar.PriceRu))
            {
                neededCar.Prices = await GetPriceListByIdsAsync(neededCar.PriceRu);
            }

            if (!string.IsNullOrEmpty(neededCar.AboutRu))
            {
                neededCar.Abouts = await GetAboutListByIdsAsync(neededCar.AboutRu);
            }

            if (!string.IsNullOrEmpty(neededCar.ResultRu))
            {
                neededCar.PriResults = await GetResultListByIdsAsync(neededCar.ResultRu);
            }

            if (!string.IsNullOrEmpty(neededCar.EngineControlRu))
            {
                neededCar.Engine_controlers = await GetEngineControlListByIdsAsync(neededCar.EngineControlRu);
            }

            if (!string.IsNullOrEmpty(neededCar.grafic))
            {
                neededCar.Grafics = await GetGraficListByIdsAsync(neededCar.grafic);
            }

            if (!string.IsNullOrEmpty(neededCar.additional_price_ru))
            {
                neededCar.AdditionalPrices = await GetAdditionalPriceListByIdsAsync(neededCar.additional_price_ru);
            }

            return View(neededCar);
        }

        public IActionResult CarsByBrand(string brand)
        {
            if (string.IsNullOrEmpty(brand))
            {
                return RedirectToAction("Index");
            }

            var cars = DatabaseModel.Cars
                .Where(c => c.Brand == brand)
                .OrderBy(c => c.Model)
                .ThenBy(c => c.Generation)
                .ThenBy(c => c.Engine)
                .ToList();

            ViewBag.Brand = brand;
            return View(cars);
        }

        public IActionResult Partners()
        {
            return View(DatabaseModel.Partners);
        }

        public IActionResult News()
        {
            return View(DatabaseModel.News);
        }

        [HttpPost("/api/db-notify")]
        public IActionResult DbNotify()
        {
            try
            {
                Console.WriteLine($"📡 [{DateTime.Now}] Сигнал от админки получен");

                DatabaseModel.Cars = GetCarsFromDatabase();
                DatabaseModel.Partners = GetPartnersFromDatabase();
                DatabaseModel.furst_page = GetFurstPageFromDatabase();
                LoadDropdownData();

                Console.WriteLine($"✅ [{DateTime.Now}] Кэш перезагружен");

                return Ok(new { success = true, message = "Cache reloaded" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // ==========================================
        // МЕТОДЫ ДЛЯ ЗАГРУЗКИ СВЯЗАННЫХ ДАННЫХ
        // ==========================================

        private async Task<List<PriceModel>> GetPriceListByIdsAsync(string idsString)
        {
            var result = new List<PriceModel>();
            if (string.IsNullOrEmpty(idsString)) return result;

            var ids = idsString.Split(',')
                               .Select(id => int.Parse(id.Trim()))
                               .ToList();

            string connectionString = GetConnectionString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var priceCache = new Dictionary<int, PriceModel>();

                foreach (var id in ids)
                {
                    string sourceTable = null;
                    using (var cmd = new MySqlCommand("SELECT source_table FROM global_ids WHERE id = @id", connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        var resultScalar = await cmd.ExecuteScalarAsync();
                        sourceTable = resultScalar?.ToString();
                    }

                    if (string.IsNullOrEmpty(sourceTable))
                        continue;

                    if (sourceTable == "price")
                    {
                        if (!priceCache.ContainsKey(id))
                        {
                            var price = await GetPriceByIdAsync(connection, id);
                            if (price != null)
                                priceCache[id] = price;
                        }

                        if (priceCache.ContainsKey(id))
                            result.Add(priceCache[id]);
                    }
                    else if (sourceTable == "template_price")
                    {
                        string linkedPriceIds = null;
                        using (var cmd = new MySqlCommand("SELECT prices FROM template_price WHERE id = @id", connection))
                        {
                            cmd.Parameters.AddWithValue("@id", id);
                            linkedPriceIds = (await cmd.ExecuteScalarAsync())?.ToString();
                        }

                        if (!string.IsNullOrEmpty(linkedPriceIds))
                        {
                            var linkedIds = linkedPriceIds.Split(',')
                                                           .Select(x => int.Parse(x.Trim()))
                                                           .ToList();

                            foreach (var linkedId in linkedIds)
                            {
                                if (!priceCache.ContainsKey(linkedId))
                                {
                                    var price = await GetPriceByIdAsync(connection, linkedId);
                                    if (price != null)
                                        priceCache[linkedId] = price;
                                }

                                if (priceCache.ContainsKey(linkedId))
                                    result.Add(priceCache[linkedId]);
                            }
                        }
                    }
                }
            }

            return result;
        }

        private async Task<PriceModel> GetPriceByIdAsync(MySqlConnection connection, int id)
        {
            using (var cmd = new MySqlCommand(
                "SELECT id, name_ru, name_eng, name_ger, base_price, pro_price, " +
                "base_price_eng, pro_price_eng, base_price_ger, pro_price_ger, " +
                "info_ru, info_eng, info_ger " +
                "FROM price WHERE id = @id",
                connection))
            {
                cmd.Parameters.AddWithValue("@id", id);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new PriceModel
                        {
                            id = reader.GetInt32("id"),
                            name_ru = reader.IsDBNull(reader.GetOrdinal("name_ru")) ? "" : reader.GetString("name_ru"),
                            name_eng = reader.IsDBNull(reader.GetOrdinal("name_eng")) ? "" : reader.GetString("name_eng"),
                            name_ger = reader.IsDBNull(reader.GetOrdinal("name_ger")) ? "" : reader.GetString("name_ger"),
                            base_price = reader.IsDBNull(reader.GetOrdinal("base_price")) ? "" : reader.GetString("base_price"),
                            pro_price = reader.IsDBNull(reader.GetOrdinal("pro_price")) ? "" : reader.GetString("pro_price"),
                            base_price_eng = reader.IsDBNull(reader.GetOrdinal("base_price_eng")) ? "" : reader.GetString("base_price_eng"),
                            pro_price_eng = reader.IsDBNull(reader.GetOrdinal("pro_price_eng")) ? "" : reader.GetString("pro_price_eng"),
                            base_price_ger = reader.IsDBNull(reader.GetOrdinal("base_price_ger")) ? "" : reader.GetString("base_price_ger"),
                            pro_price_ger = reader.IsDBNull(reader.GetOrdinal("pro_price_ger")) ? "" : reader.GetString("pro_price_ger"),
                            info_ru = reader.IsDBNull(reader.GetOrdinal("info_ru")) ? "" : reader.GetString("info_ru"),
                            info_eng = reader.IsDBNull(reader.GetOrdinal("info_eng")) ? "" : reader.GetString("info_eng"),
                            info_ger = reader.IsDBNull(reader.GetOrdinal("info_ger")) ? "" : reader.GetString("info_ger")
                        };
                    }
                }
            }
            return null;
        }

        private async Task<List<AboutModel>> GetAboutListByIdsAsync(string idsString)
        {
            var result = new List<AboutModel>();
            if (string.IsNullOrEmpty(idsString)) return result;

            var ids = idsString.Split(',').Select(int.Parse).ToList();
            string connectionString = GetConnectionString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var cache = new Dictionary<int, AboutModel>();

                foreach (var id in ids)
                {
                    string sourceTable = null;
                    using (var cmd = new MySqlCommand("SELECT source_table FROM global_ids WHERE id = @id", connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        sourceTable = (await cmd.ExecuteScalarAsync())?.ToString();
                    }

                    if (string.IsNullOrEmpty(sourceTable)) continue;

                    if (sourceTable == "about")
                    {
                        if (!cache.ContainsKey(id))
                        {
                            var item = await GetAboutByIdAsync(connection, id);
                            if (item != null) cache[id] = item;
                        }
                        if (cache.ContainsKey(id)) result.Add(cache[id]);
                    }
                    else if (sourceTable == "template_about")
                    {
                        string linkedIds = null;
                        using (var cmd = new MySqlCommand("SELECT ids FROM template_about WHERE id = @id", connection))
                        {
                            cmd.Parameters.AddWithValue("@id", id);
                            linkedIds = (await cmd.ExecuteScalarAsync())?.ToString();
                        }

                        if (!string.IsNullOrEmpty(linkedIds))
                        {
                            foreach (var linkedId in linkedIds.Split(',').Select(int.Parse))
                            {
                                if (!cache.ContainsKey(linkedId))
                                {
                                    var item = await GetAboutByIdAsync(connection, linkedId);
                                    if (item != null) cache[linkedId] = item;
                                }
                                if (cache.ContainsKey(linkedId)) result.Add(cache[linkedId]);
                            }
                        }
                    }
                }
            }
            return result;
        }

        private async Task<AboutModel> GetAboutByIdAsync(MySqlConnection connection, int id)
        {
            using (var cmd = new MySqlCommand("SELECT id, text_ru, text_eng, text_ger FROM about WHERE id = @id", connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new AboutModel
                        {
                            id = reader.GetInt32("id"),
                            text_ru = reader.IsDBNull(reader.GetOrdinal("text_ru")) ? "" : reader.GetString("text_ru"),
                            text_eng = reader.IsDBNull(reader.GetOrdinal("text_eng")) ? "" : reader.GetString("text_eng"),
                            text_ger = reader.IsDBNull(reader.GetOrdinal("text_ger")) ? "" : reader.GetString("text_ger")
                        };
                    }
                }
            }
            return null;
        }

        private async Task<List<ResultModel>> GetResultListByIdsAsync(string idsString)
        {
            var result = new List<ResultModel>();
            if (string.IsNullOrEmpty(idsString)) return result;

            var ids = idsString.Split(',').Select(int.Parse).ToList();
            string connectionString = GetConnectionString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var cache = new Dictionary<int, ResultModel>();

                foreach (var id in ids)
                {
                    string sourceTable = null;
                    using (var cmd = new MySqlCommand("SELECT source_table FROM global_ids WHERE id = @id", connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        sourceTable = (await cmd.ExecuteScalarAsync())?.ToString();
                    }

                    if (string.IsNullOrEmpty(sourceTable)) continue;

                    if (sourceTable == "result")
                    {
                        if (!cache.ContainsKey(id))
                        {
                            var item = await GetResultByIdAsync(connection, id);
                            if (item != null) cache[id] = item;
                        }
                        if (cache.ContainsKey(id)) result.Add(cache[id]);
                    }
                    else if (sourceTable == "template_result")
                    {
                        string linkedIds = null;
                        using (var cmd = new MySqlCommand("SELECT ids FROM template_result WHERE id = @id", connection))
                        {
                            cmd.Parameters.AddWithValue("@id", id);
                            linkedIds = (await cmd.ExecuteScalarAsync())?.ToString();
                        }

                        if (!string.IsNullOrEmpty(linkedIds))
                        {
                            foreach (var linkedId in linkedIds.Split(',').Select(int.Parse))
                            {
                                if (!cache.ContainsKey(linkedId))
                                {
                                    var item = await GetResultByIdAsync(connection, linkedId);
                                    if (item != null) cache[linkedId] = item;
                                }
                                if (cache.ContainsKey(linkedId)) result.Add(cache[linkedId]);
                            }
                        }
                    }
                }
            }
            return result;
        }

        private async Task<ResultModel> GetResultByIdAsync(MySqlConnection connection, int id)
        {
            using (var cmd = new MySqlCommand("SELECT id, text_ru, text_eng, text_ger FROM result WHERE id = @id", connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new ResultModel
                        {
                            id = reader.GetInt32("id"),
                            text_ru = reader.IsDBNull(reader.GetOrdinal("text_ru")) ? "" : reader.GetString("text_ru"),
                            text_eng = reader.IsDBNull(reader.GetOrdinal("text_eng")) ? "" : reader.GetString("text_eng"),
                            text_ger = reader.IsDBNull(reader.GetOrdinal("text_ger")) ? "" : reader.GetString("text_ger")
                        };
                    }
                }
            }
            return null;
        }

        private async Task<List<EngineControlModel>> GetEngineControlListByIdsAsync(string idsString)
        {
            var result = new List<EngineControlModel>();
            if (string.IsNullOrEmpty(idsString)) return result;

            var ids = idsString.Split(',').Select(int.Parse).ToList();
            string connectionString = GetConnectionString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var cache = new Dictionary<int, EngineControlModel>();

                foreach (var id in ids)
                {
                    string sourceTable = null;
                    using (var cmd = new MySqlCommand("SELECT source_table FROM global_ids WHERE id = @id", connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        sourceTable = (await cmd.ExecuteScalarAsync())?.ToString();
                    }

                    if (string.IsNullOrEmpty(sourceTable)) continue;

                    if (sourceTable == "engine_control")
                    {
                        if (!cache.ContainsKey(id))
                        {
                            var item = await GetEngineControlByIdAsync(connection, id);
                            if (item != null) cache[id] = item;
                        }
                        if (cache.ContainsKey(id)) result.Add(cache[id]);
                    }
                    else if (sourceTable == "template_engine_control")
                    {
                        string linkedIds = null;
                        using (var cmd = new MySqlCommand("SELECT ids FROM template_engine_control WHERE id = @id", connection))
                        {
                            cmd.Parameters.AddWithValue("@id", id);
                            linkedIds = (await cmd.ExecuteScalarAsync())?.ToString();
                        }

                        if (!string.IsNullOrEmpty(linkedIds))
                        {
                            foreach (var linkedId in linkedIds.Split(',').Select(int.Parse))
                            {
                                if (!cache.ContainsKey(linkedId))
                                {
                                    var item = await GetEngineControlByIdAsync(connection, linkedId);
                                    if (item != null) cache[linkedId] = item;
                                }
                                if (cache.ContainsKey(linkedId)) result.Add(cache[linkedId]);
                            }
                        }
                    }
                }
            }
            return result;
        }

        private async Task<EngineControlModel> GetEngineControlByIdAsync(MySqlConnection connection, int id)
        {
            using (var cmd = new MySqlCommand("SELECT id, text_ru, text_eng, text_ger FROM engine_control WHERE id = @id", connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new EngineControlModel
                        {
                            id = reader.GetInt32("id"),
                            text_ru = reader.IsDBNull(reader.GetOrdinal("text_ru")) ? "" : reader.GetString("text_ru"),
                            text_eng = reader.IsDBNull(reader.GetOrdinal("text_eng")) ? "" : reader.GetString("text_eng"),
                            text_ger = reader.IsDBNull(reader.GetOrdinal("text_ger")) ? "" : reader.GetString("text_ger")
                        };
                    }
                }
            }
            return null;
        }

        private async Task<List<GraficModel>> GetGraficListByIdsAsync(string idsString)
        {
            var result = new List<GraficModel>();
            if (string.IsNullOrEmpty(idsString)) return result;

            var ids = idsString.Split(',').Select(int.Parse).ToList();
            string connectionString = GetConnectionString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var cache = new Dictionary<int, GraficModel>();

                foreach (var id in ids)
                {
                    string sourceTable = null;
                    using (var cmd = new MySqlCommand("SELECT source_table FROM global_ids WHERE id = @id", connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        sourceTable = (await cmd.ExecuteScalarAsync())?.ToString();
                    }

                    if (string.IsNullOrEmpty(sourceTable)) continue;

                    if (sourceTable == "grafic")
                    {
                        if (!cache.ContainsKey(id))
                        {
                            var item = await GetGraficByIdAsync(connection, id);
                            if (item != null) cache[id] = item;
                        }
                        if (cache.ContainsKey(id)) result.Add(cache[id]);
                    }
                    else if (sourceTable == "template_grafic")
                    {
                        string linkedIds = null;
                        using (var cmd = new MySqlCommand("SELECT ids FROM template_grafic WHERE id = @id", connection))
                        {
                            cmd.Parameters.AddWithValue("@id", id);
                            linkedIds = (await cmd.ExecuteScalarAsync())?.ToString();
                        }

                        if (!string.IsNullOrEmpty(linkedIds))
                        {
                            foreach (var linkedId in linkedIds.Split(',').Select(int.Parse))
                            {
                                if (!cache.ContainsKey(linkedId))
                                {
                                    var item = await GetGraficByIdAsync(connection, linkedId);
                                    if (item != null) cache[linkedId] = item;
                                }
                                if (cache.ContainsKey(linkedId)) result.Add(cache[linkedId]);
                            }
                        }
                    }
                }
            }
            return result;
        }

        private async Task<GraficModel> GetGraficByIdAsync(MySqlConnection connection, int id)
        {
            using (var cmd = new MySqlCommand(
                "SELECT id, name, name_eng, name_ger, image, " +
                "description_ru, description_eng, description_ger " +
                "FROM grafic WHERE id = @id",
                connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new GraficModel
                        {
                            id = reader.GetInt32("id"),
                            Name = reader.IsDBNull(reader.GetOrdinal("name")) ? "" : reader.GetString("name"),
                            NameEng = reader.IsDBNull(reader.GetOrdinal("name_eng")) ? "" : reader.GetString("name_eng"),
                            NameGer = reader.IsDBNull(reader.GetOrdinal("name_ger")) ? "" : reader.GetString("name_ger"),
                            image = reader.IsDBNull(reader.GetOrdinal("image")) ? "" : reader.GetString("image"),
                            DescriptionRu = reader.IsDBNull(reader.GetOrdinal("description_ru")) ? "" : reader.GetString("description_ru"),
                            DescriptionEng = reader.IsDBNull(reader.GetOrdinal("description_eng")) ? "" : reader.GetString("description_eng"),
                            DescriptionGer = reader.IsDBNull(reader.GetOrdinal("description_ger")) ? "" : reader.GetString("description_ger")
                        };
                    }
                }
            }
            return null;
        }

        private async Task<List<AdditionalPriceModel>> GetAdditionalPriceListByIdsAsync(string idsString)
        {
            var result = new List<AdditionalPriceModel>();
            if (string.IsNullOrEmpty(idsString)) return result;

            var ids = idsString.Split(',')
                               .Select(id => id.Trim())
                               .ToList();

            string connectionString = GetConnectionString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var cache = new Dictionary<int, AdditionalPriceModel>();

                foreach (var idStr in ids)
                {
                    // ===== ПРОВЕРЯЕМ: МОЖЕТ БЫТЬ ШАБЛОН =====
                    if (idStr.StartsWith("tpl_", StringComparison.OrdinalIgnoreCase))
                    {
                        var templateIdStr = idStr.Substring(4);
                        if (int.TryParse(templateIdStr, out int templateId))
                        {
                            var expandedIds = await ExpandTemplateIdsAsync(connection, templateId.ToString());
                            foreach (var expandedId in expandedIds)
                            {
                                if (!cache.ContainsKey(expandedId))
                                {
                                    var item = await GetAdditionalPriceByIdAsync(connection, expandedId);
                                    if (item != null) cache[expandedId] = item;
                                }
                                if (cache.ContainsKey(expandedId)) result.Add(cache[expandedId]);
                            }
                        }
                        continue;
                    }

                    if (!int.TryParse(idStr, out int id)) continue;

                    string sourceTable = null;
                    using (var cmd = new MySqlCommand("SELECT source_table FROM global_ids WHERE id = @id", connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        sourceTable = (await cmd.ExecuteScalarAsync())?.ToString();
                    }

                    if (string.IsNullOrEmpty(sourceTable)) continue;

                    if (sourceTable == "additional_prices")
                    {
                        if (!cache.ContainsKey(id))
                        {
                            var item = await GetAdditionalPriceByIdAsync(connection, id);
                            if (item != null) cache[id] = item;
                        }
                        if (cache.ContainsKey(id)) result.Add(cache[id]);
                    }
                    else if (sourceTable == "template_additional_prices")
                    {
                        string linkedIds = null;
                        using (var cmd = new MySqlCommand("SELECT price_ids FROM template_additional_prices WHERE id = @id", connection))
                        {
                            cmd.Parameters.AddWithValue("@id", id);
                            linkedIds = (await cmd.ExecuteScalarAsync())?.ToString();
                        }

                        if (!string.IsNullOrEmpty(linkedIds))
                        {
                            // ===== ИСПРАВЛЕНИЕ: РАСКРЫВАЕМ ШАБЛОНЫ =====
                            var expandedIds = await ExpandTemplateIdsAsync(connection, linkedIds);

                            foreach (var expandedId in expandedIds)
                            {
                                if (!cache.ContainsKey(expandedId))
                                {
                                    var item = await GetAdditionalPriceByIdAsync(connection, expandedId);
                                    if (item != null) cache[expandedId] = item;
                                }
                                if (cache.ContainsKey(expandedId)) result.Add(cache[expandedId]);
                            }
                        }
                    }
                }
            }
            return result;
        }

        // В HomeController.cs

        /// <summary>
        /// Обработка старых URL - просто ищем в БД
        /// </summary>
        public async Task<IActionResult> CarByOldUrl(string oldUrl)
        {
            Console.WriteLine($"=== CarByOldUrl ===");
            Console.WriteLine($"oldUrl: '{oldUrl}'");
            if (string.IsNullOrEmpty(oldUrl))
            {
                return RedirectToAction("Index");
            }

            // ===== НОРМАЛИЗУЕМ URL =====
            // Убираем слеши в начале/конце, приводим к нижнему регистру
            var normalizedUrl = oldUrl.Trim().TrimStart('/').TrimEnd('/').ToLower();

            // ===== УБИРАЕМ ЯЗЫКОВОЙ ПРЕФИКС (ru/en/de) =====
            var parts = normalizedUrl.Split('/');
            if (parts.Length >= 2 && (parts[0] == "ru" || parts[0] == "en" || parts[0] == "de"))
            {
                // Убираем языковой префикс
                normalizedUrl = string.Join("/", parts.Skip(1));
                Console.WriteLine($"After removing language prefix: '{normalizedUrl}'");
            }

            // Если URL начинается с "cars/", убираем это
            if (normalizedUrl.StartsWith("cars/"))
            {
                normalizedUrl = normalizedUrl.Substring(5);
                Console.WriteLine($"After removing 'cars/': '{normalizedUrl}'");
            }

            Console.WriteLine($"Final normalized URL: '{normalizedUrl}'");

            // ===== ИЩЕМ МАШИНУ ПО old_url =====
            var car = DatabaseModel.Cars.FirstOrDefault(c =>
                c.old_url != null &&
                c.old_url.ToLower() == normalizedUrl);

            if (car == null)
            {
                // Пробуем найти частичное совпадение (если URL чуть отличается)
                car = DatabaseModel.Cars.FirstOrDefault(c =>
                    c.old_url != null &&
                    normalizedUrl.Contains(c.old_url.ToLower()));
            }

            if (car == null)
            {
                TempData["Error"] = "Автомобиль не найден по старой ссылке";
                return RedirectToAction("Index");
            }

            // ===== ПЕРЕНАПРАВЛЯЕМ НА НОВЫЙ URL =====
            var newBrand = TransliterateForUrl(car.Brand);
            var newModel = TransliterateForUrl(car.Model);
            var newGeneration = TransliterateForUrl(car.Generation);
            var newEngine = TransliterateForUrl(car.Engine);

            Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            Response.Headers.Add("Pragma", "no-cache");
            Response.Headers.Add("Expires", "0");

            return RedirectPermanent($"/{newBrand}/{newModel}/{newGeneration}/{newEngine}");
        }

        private async Task<AdditionalPriceModel> GetAdditionalPriceByIdAsync(MySqlConnection connection, int id)
        {
            // ===== СНАЧАЛА ПРОВЕРЯЕМ: ЯВЛЯЕТСЯ ЛИ ID ШАБЛОНОМ? =====
            string sourceTable = null;
            using (var cmd = new MySqlCommand("SELECT source_table FROM global_ids WHERE id = @id", connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                sourceTable = (await cmd.ExecuteScalarAsync())?.ToString();
            }

            // Если это шаблон дополнительных цен - раскрываем его
            if (sourceTable == "template_additional_prices")
            {
                string priceIds = null;
                using (var cmd = new MySqlCommand("SELECT price_ids FROM template_additional_prices WHERE id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    priceIds = (await cmd.ExecuteScalarAsync())?.ToString();
                }

                if (!string.IsNullOrEmpty(priceIds))
                {
                    // Разбиваем ID, раскрываем каждый
                    var ids = priceIds.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                      .Select(p => p.Trim())
                                      .ToList();

                    // Для простоты возьмём первый не-шаблонный ID (или можно обработать все)
                    foreach (var idStr in ids)
                    {
                        if (int.TryParse(idStr, out int realId))
                        {
                            // Проверяем, не шаблон ли это снова
                            string subSourceTable = null;
                            using (var subCmd = new MySqlCommand("SELECT source_table FROM global_ids WHERE id = @id", connection))
                            {
                                subCmd.Parameters.AddWithValue("@id", realId);
                                subSourceTable = (await subCmd.ExecuteScalarAsync())?.ToString();
                            }

                            if (subSourceTable == "additional_prices")
                            {
                                // Это реальная запись - возвращаем её
                                return await GetAdditionalPriceByIdDirectAsync(connection, realId);
                            }
                            else if (subSourceTable == "template_additional_prices")
                            {
                                // Рекурсивно раскрываем шаблон
                                return await GetAdditionalPriceByIdAsync(connection, realId);
                            }
                        }
                        else if (idStr.StartsWith("tpl_", StringComparison.OrdinalIgnoreCase))
                        {
                            var templateIdStr = idStr.Substring(4);
                            if (int.TryParse(templateIdStr, out int templateId))
                            {
                                return await GetAdditionalPriceByIdAsync(connection, templateId);
                            }
                        }
                    }
                }
                return null;
            }

            // Если это не шаблон - загружаем напрямую
            return await GetAdditionalPriceByIdDirectAsync(connection, id);
        }

        /// <summary>
        /// Прямая загрузка дополнительной цены по ID (без проверки на шаблон)
        /// </summary>
        private async Task<AdditionalPriceModel> GetAdditionalPriceByIdDirectAsync(MySqlConnection connection, int id)
        {
            using (var cmd = new MySqlCommand(
                "SELECT id, name_ru, name_eng, name_ger, price_rubl, price_dolar, price_euro, " +
                "info_ru, info_eng, info_ger, sort_order, price_controler, " +
                "free_price_ids, base_price_ids, pro_price_ids, unselected_price_mode " +
                "FROM additional_prices WHERE id = @id",
                connection))
            {
                cmd.Parameters.AddWithValue("@id", id);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        // Читаем сырые данные
                        var rawFreePriceIds = reader.IsDBNull(reader.GetOrdinal("free_price_ids")) ? "" : reader.GetString("free_price_ids");
                        var rawBasePriceIds = reader.IsDBNull(reader.GetOrdinal("base_price_ids")) ? "" : reader.GetString("base_price_ids");
                        var rawProPriceIds = reader.IsDBNull(reader.GetOrdinal("pro_price_ids")) ? "" : reader.GetString("pro_price_ids");

                        // Раскрываем шаблоны в полях free/base/pro
                        var expandedFree = await ExpandTemplateIdsAsync(connection, rawFreePriceIds);
                        var expandedBase = await ExpandTemplateIdsAsync(connection, rawBasePriceIds);
                        var expandedPro = await ExpandTemplateIdsAsync(connection, rawProPriceIds);

                        return new AdditionalPriceModel
                        {
                            id = reader.GetInt32("id"),
                            name_ru = reader.IsDBNull(reader.GetOrdinal("name_ru")) ? "" : reader.GetString("name_ru"),
                            name_eng = reader.IsDBNull(reader.GetOrdinal("name_eng")) ? "" : reader.GetString("name_eng"),
                            name_ger = reader.IsDBNull(reader.GetOrdinal("name_ger")) ? "" : reader.GetString("name_ger"),
                            price_rubl = reader.IsDBNull(reader.GetOrdinal("price_rubl")) ? "" : reader.GetString("price_rubl"),
                            price_dolar = reader.IsDBNull(reader.GetOrdinal("price_dolar")) ? "" : reader.GetString("price_dolar"),
                            price_euro = reader.IsDBNull(reader.GetOrdinal("price_euro")) ? "" : reader.GetString("price_euro"),
                            info_ru = reader.IsDBNull(reader.GetOrdinal("info_ru")) ? "" : reader.GetString("info_ru"),
                            info_eng = reader.IsDBNull(reader.GetOrdinal("info_eng")) ? "" : reader.GetString("info_eng"),
                            info_ger = reader.IsDBNull(reader.GetOrdinal("info_ger")) ? "" : reader.GetString("info_ger"),
                            sort_order = reader.IsDBNull(reader.GetOrdinal("sort_order")) ? 0 : reader.GetInt32("sort_order"),
                            price_controler = reader.IsDBNull(reader.GetOrdinal("price_controler")) ? 0 : reader.GetInt32("price_controler"),
                            free_price_ids = string.Join(",", expandedFree),
                            base_price_ids = string.Join(",", expandedBase),
                            pro_price_ids = string.Join(",", expandedPro),
                            unselected_price_mode = reader.IsDBNull(reader.GetOrdinal("unselected_price_mode")) ? 0 : reader.GetInt32("unselected_price_mode")
                        };
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Раскрывает шаблоны (tpl_XXX) в список ID записей, убирает дубликаты
        /// </summary>
        private async Task<List<int>> ExpandTemplateIdsAsync(MySqlConnection connection, string idsString)
        {
            var result = new List<int>();
            if (string.IsNullOrEmpty(idsString)) return result;

            var seen = new HashSet<int>();

            var parts = idsString.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                 .Select(p => p.Trim())
                                 .ToList();

            // ===== СОЗДАЁМ НОВОЕ ПОДКЛЮЧЕНИЕ ДЛЯ ВНУТРЕННИХ ЗАПРОСОВ =====
            string connectionString = GetConnectionString();

            foreach (var part in parts)
            {
                // ===== ЕСЛИ ЭТО ШАБЛОН С ПРЕФИКСОМ "tpl_" =====
                if (part.StartsWith("tpl_", StringComparison.OrdinalIgnoreCase))
                {
                    var templateIdStr = part.Substring(4);
                    if (int.TryParse(templateIdStr, out int templateId))
                    {
                        using (var newConnection = new MySqlConnection(connectionString))
                        {
                            await newConnection.OpenAsync();
                            var templateContent = await GetTemplateContentAsync(newConnection, templateId);
                            if (!string.IsNullOrEmpty(templateContent))
                            {
                                var expanded = await ExpandTemplateIdsAsync(newConnection, templateContent);
                                foreach (var id in expanded)
                                {
                                    if (!seen.Contains(id))
                                    {
                                        result.Add(id);
                                        seen.Add(id);
                                    }
                                }
                            }
                        }
                    }
                }
                // ===== ЕСЛИ ЭТО ОБЫЧНЫЙ ID =====
                else if (int.TryParse(part, out int directId))
                {
                    using (var newConnection = new MySqlConnection(connectionString))
                    {
                        await newConnection.OpenAsync();

                        // Проверяем, является ли ID шаблоном
                        string sourceTable = null;
                        using (var cmd = new MySqlCommand("SELECT source_table FROM global_ids WHERE id = @id", newConnection))
                        {
                            cmd.Parameters.AddWithValue("@id", directId);
                            sourceTable = (await cmd.ExecuteScalarAsync())?.ToString();
                        }

                        // ===== РАСКРЫВАЕМ ШАБЛОН ДОПОЛНИТЕЛЬНЫХ ЦЕН =====
                        if (sourceTable == "template_additional_prices")
                        {
                            string priceIds = null;
                            using (var cmd = new MySqlCommand("SELECT price_ids FROM template_additional_prices WHERE id = @id", newConnection))
                            {
                                cmd.Parameters.AddWithValue("@id", directId);
                                priceIds = (await cmd.ExecuteScalarAsync())?.ToString();
                            }

                            if (!string.IsNullOrEmpty(priceIds))
                            {
                                var expanded = await ExpandTemplateIdsAsync(newConnection, priceIds);
                                foreach (var id in expanded)
                                {
                                    if (!seen.Contains(id))
                                    {
                                        result.Add(id);
                                        seen.Add(id);
                                    }
                                }
                            }
                        }
                        // ===== РАСКРЫВАЕМ ШАБЛОН ЦЕН ОПЦИЙ =====
                        else if (sourceTable == "template_price")
                        {
                            string prices = null;
                            using (var cmd = new MySqlCommand("SELECT prices FROM template_price WHERE id = @id", newConnection))
                            {
                                cmd.Parameters.AddWithValue("@id", directId);
                                prices = (await cmd.ExecuteScalarAsync())?.ToString();
                            }

                            if (!string.IsNullOrEmpty(prices))
                            {
                                var expanded = await ExpandTemplateIdsAsync(newConnection, prices);
                                foreach (var id in expanded)
                                {
                                    if (!seen.Contains(id))
                                    {
                                        result.Add(id);
                                        seen.Add(id);
                                    }
                                }
                            }
                        }
                        // ===== ОБЫЧНЫЙ ID — ПРОСТО ДОБАВЛЯЕМ =====
                        else
                        {
                            if (!seen.Contains(directId))
                            {
                                result.Add(directId);
                                seen.Add(directId);
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Получает содержимое шаблона по его ID
        /// </summary>
        private async Task<string> GetTemplateContentAsync(MySqlConnection connection, int templateId)
        {
            string sourceTable = null;
            using (var cmd = new MySqlCommand("SELECT source_table FROM global_ids WHERE id = @id", connection))
            {
                cmd.Parameters.AddWithValue("@id", templateId);
                sourceTable = (await cmd.ExecuteScalarAsync())?.ToString();
            }

            if (string.IsNullOrEmpty(sourceTable)) return null;

            string idsField = sourceTable switch
            {
                "template_price" => "prices",
                "template_about" => "ids",
                "template_result" => "ids",
                "template_engine_control" => "ids",
                "template_grafic" => "ids",
                "template_additional_prices" => "price_ids",
                _ => null
            };

            if (string.IsNullOrEmpty(idsField)) return null;

            using (var cmd = new MySqlCommand($"SELECT {idsField} FROM {sourceTable} WHERE id = @id", connection))
            {
                cmd.Parameters.AddWithValue("@id", templateId);
                return (await cmd.ExecuteScalarAsync())?.ToString();
            }
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        // ==========================================
        // СТАТИЧЕСКИЙ МЕТОД ДЛЯ ТРАНСЛИТЕРАЦИИ URL
        // ==========================================

        public static string TransliterateForUrl(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";

            var replacements = new Dictionary<char, string>
    {
        // Заглавные буквы
        {'А', "A"}, {'Б', "B"}, {'В', "V"}, {'Г', "G"}, {'Д', "D"},
        {'Е', "E"}, {'Ё', "Yo"}, {'Ж', "Zh"}, {'З', "Z"}, {'И', "I"},
        {'Й', "Y"}, {'К', "K"}, {'Л', "L"}, {'М', "M"}, {'Н', "N"},
        {'О', "O"}, {'П', "P"}, {'Р', "R"}, {'С', "S"}, {'Т', "T"},
        {'У', "U"}, {'Ф', "F"}, {'Х', "Kh"}, {'Ц', "Ts"}, {'Ч', "Ch"},
        {'Ш', "Sh"}, {'Щ', "Sch"}, {'Ъ', ""}, {'Ы', "Y"}, {'Ь', ""},
        {'Э', "E"}, {'Ю', "Yu"}, {'Я', "Ya"},
        // Строчные буквы
        {'а', "a"}, {'б', "b"}, {'в', "v"}, {'г', "g"}, {'д', "d"},
        {'е', "e"}, {'ё', "yo"}, {'ж', "zh"}, {'з', "z"}, {'и', "i"},
        {'й', "y"}, {'к', "k"}, {'л', "l"}, {'м', "m"}, {'н', "n"},
        {'о', "o"}, {'п', "p"}, {'р', "r"}, {'с', "s"}, {'т', "t"},
        {'у', "u"}, {'ф', "f"}, {'х', "kh"}, {'ц', "ts"}, {'ч', "ch"},
        {'ш', "sh"}, {'щ', "sch"}, {'ъ', ""}, {'ы', "y"}, {'ь', ""},
        {'э', "e"}, {'ю', "yu"}, {'я', "ya"}
    };

            var result = new System.Text.StringBuilder();
            foreach (var ch in text)
            {
                if (replacements.ContainsKey(ch))
                    result.Append(replacements[ch]);
                else
                    result.Append(ch);
            }

            // Заменяем пробелы на _ и убираем двойные подчеркивания
            var final = result.ToString().Replace(" ", "_");

            // Убираем повторяющиеся подчеркивания
            while (final.Contains("__"))
                final = final.Replace("__", "_");

            // Убираем подчеркивания в начале и конце
            final = final.Trim('_');

            return final;
        }
    }

    // ==========================================
    // МОДЕЛИ ДЛЯ ЗАПРОСОВ/ОТВЕТОВ
    // ==========================================

    public class SelectionRequest
    {
        public ReflashCarModel Car { get; set; } = new ReflashCarModel();
        public string ChangedField { get; set; } = "";
        public string NewValue { get; set; } = "";
    }

    public class SelectionResponse
    {
        public ReflashCarModel Car { get; set; } = new ReflashCarModel();
        public string NextField { get; set; } = "";
        public List<string> Options { get; set; } = new List<string>();
    }
}
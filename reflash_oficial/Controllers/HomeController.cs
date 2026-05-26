using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using reflash_oficial.Models;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
//using System.Configuration;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;



namespace reflash_oficial.Controllers
{

    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

       

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Метод для получения списка автомобилей
        private List<ReflashCarModel> GetCarsFromDatabase()
        {
            List<ReflashCarModel> cars = new List<ReflashCarModel>();
            string connectionString = _configuration.GetConnectionString("DefaultConnection")
                ?? "server=localhost;port=3306;database=reflash_cars;user=root;password=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM reflash_cars ORDER BY brand, model, generation";

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
                                    AboutEng = reader.IsDBNull(reader.GetOrdinal("about_eng")) ? "" : reader.GetString("about_eng"),
                                    AboutGer = reader.IsDBNull(reader.GetOrdinal("about_ger")) ? "" : reader.GetString("about_ger"),
                                    ResultRu = reader.IsDBNull(reader.GetOrdinal("result_ru")) ? "" : reader.GetString("result_ru"),
                                    ResultEng = reader.IsDBNull(reader.GetOrdinal("result_eng")) ? "" : reader.GetString("result_eng"),
                                    ResultGer = reader.IsDBNull(reader.GetOrdinal("result_ger")) ? "" : reader.GetString("result_ger"),
                                    EngineControlRu = reader.IsDBNull(reader.GetOrdinal("engine_control_ru")) ? "" : reader.GetString("engine_control_ru"),
                                    EngineControlEng = reader.IsDBNull(reader.GetOrdinal("engine_control_eng")) ? "" : reader.GetString("engine_control_eng"),
                                    EngineControlGer = reader.IsDBNull(reader.GetOrdinal("engine_control_ger")) ? "" : reader.GetString("engine_control_ger"),
                                    OptionsRu = reader.IsDBNull(reader.GetOrdinal("options_ru")) ? "" : reader.GetString("options_ru"),
                                    OptionsEng = reader.IsDBNull(reader.GetOrdinal("options_eng")) ? "" : reader.GetString("options_eng"),
                                    OptionsGer = reader.IsDBNull(reader.GetOrdinal("options_ger")) ? "" : reader.GetString("options_ger"),
                                    PriceRu = reader.IsDBNull(reader.GetOrdinal("price_ru")) ? "" : reader.GetString("price_ru"),
                                    PriceEng = reader.IsDBNull(reader.GetOrdinal("price_eng")) ? "" : reader.GetString("price_eng"),
                                    PriceGer = reader.IsDBNull(reader.GetOrdinal("price_ger")) ? "" : reader.GetString("price_ger")
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
        // метод для получение столбцов таблицы с предыдущем столбцом таблицы в виде списка 
        private List<SortDatabaseModel> Get_Sort_Cars(string Type_now, List<ReflashCarModel> cars)
        {
            List<SortDatabaseModel> type = new List<SortDatabaseModel>();
            List<String> spisok = new List<String>();
            if (Type_now == "brand")
            {
                foreach (ReflashCarModel car_now in cars)
                {
                    if (!spisok.Contains(car_now.Brand))
                    {
                        spisok.Add(car_now.Brand);
                        type.Add(new SortDatabaseModel
                        {
                            Name_of_type = car_now.Brand,
                            Name_of_preType = ""
                        });
                    }
                }
            }
            else if (Type_now == "model")
            {
                foreach (ReflashCarModel car_now in cars)
                {
                    if (!spisok.Contains(car_now.Model))
                    {
                        spisok.Add(car_now.Model);
                        type.Add(new SortDatabaseModel
                        {
                            Name_of_type = car_now.Model,
                            Name_of_preType = car_now.Brand
                        });
                    }
                }
            }
            else if (Type_now == "generation")
            {
                foreach (ReflashCarModel car_now in cars)
                {
                    spisok.Add(car_now.Model);
                    type.Add(new SortDatabaseModel
                    {
                        Name_of_type = car_now.Generation,
                        Name_of_preType = car_now.Model
                    });
                }
            }
            else if (Type_now == "engine")
            {
                foreach (ReflashCarModel car_now in cars)
                {
                    if (!spisok.Contains(car_now.Engine))
                    {
                        spisok.Add(car_now.Engine);
                        type.Add(new SortDatabaseModel
                        {
                            Name_of_type = car_now.Engine,
                            Name_of_preType = car_now.Generation
                        });
                    }
                }
            }
            else if (Type_now == "engine_with_model")
            {
                foreach (ReflashCarModel car_now in cars)
                {
                    //spisok.Add(car_now.Model);
                    type.Add(new SortDatabaseModel
                    {
                        Name_of_type = car_now.Engine,
                        Name_of_preType = car_now.Model
                    });
                }
            }
            return type;
        }
        // получение списка для drod daun
        public  List<string> spisok_to_dropdown(string type, string pre_type, string for_engin )
        {
            List<string> spisok = new List<string>();
            if (type == "brand")
            {
                foreach (SortDatabaseModel now in DatabaseModel.Brand_l)
                {
                    if ((pre_type==now.Name_of_preType) && (!spisok.Contains(now.Name_of_type)) )
                    {
                        spisok.Add(now.Name_of_type);
                    }
                }
            }
            else if (type == "model")
            {
                foreach (SortDatabaseModel now in DatabaseModel.Model_l)
                {
                    if ((pre_type == now.Name_of_preType) && (!spisok.Contains(now.Name_of_type)))
                    {
                        spisok.Add(now.Name_of_type);
                    }
                }
            }
            else if (type == "generation")
            {
                
                foreach (SortDatabaseModel now in DatabaseModel.Generation_l)
                {
                    if ((pre_type == now.Name_of_preType) && (!spisok.Contains(now.Name_of_type)))
                    {
                        spisok.Add(now.Name_of_type);
                    }
                }
            }
            else if (type == "engine")
            {
                
                foreach (SortDatabaseModel now in DatabaseModel.Engine_l)
                {

                    if ((pre_type == now.Name_of_preType) && (!spisok.Contains(now.Name_of_type)))
                    {
                        spisok.Add(now.Name_of_type);
                    }
                }
                List<string> check = new List<string>();
                foreach (SortDatabaseModel now in DatabaseModel.Engine_with_model)
                {

                    if ((for_engin == now.Name_of_preType) && (!check.Contains(now.Name_of_type)))
                    {
                        check.Add(now.Name_of_type);
                    }
                }
                var intersection = spisok.Intersect(check).ToList();
                spisok = intersection;
            }
            return spisok;
        }

        
        //вызов первой страницы с прочтением базы данных и отделением столбцов при первом переходе на страницу

        public static void RefreshData()
        {
            Console.WriteLine($"🔄 [{DateTime.Now}] Обновление данных из БД...");

            DatabaseModel.Cars = null;
            DatabaseModel.Partners = null;
            DatabaseModel.Brand_l = null;
            DatabaseModel.Model_l = null;
            DatabaseModel.Generation_l = null;
            DatabaseModel.Engine_l = null;
            DatabaseModel.Engine_with_model = null;
            DatabaseModel.News = null;
            DatabaseModel.furst_page = null;

            Console.WriteLine($"✅ [{DateTime.Now}] database rewrite");
        }


        public IActionResult Index()
        {
            DatabaseModel.furst_page = GetFurstPageFromDatabase();
            if (DatabaseModel.Cars == null || DatabaseModel.Cars.Count == 0)
            {
                DatabaseModel.Cars = GetCarsFromDatabase();
                DatabaseModel.Partners = GetPartnersFromDatabase();
                DatabaseModel.Brand_l = Get_Sort_Cars("brand", DatabaseModel.Cars);
                DatabaseModel.Model_l = Get_Sort_Cars("model", DatabaseModel.Cars);
                DatabaseModel.Generation_l = Get_Sort_Cars("generation", DatabaseModel.Cars);
                DatabaseModel.Engine_l = Get_Sort_Cars("engine", DatabaseModel.Cars);
                DatabaseModel.Engine_with_model = Get_Sort_Cars("engine_with_model", DatabaseModel.Cars);
                DatabaseModel.News = Get_News_from_data();
                

            }
            ViewBag.FurstPage = DatabaseModel.furst_page;
            return View(new ReflashCarModel());
        }

        
        // Единственный API метод для обработки ВСЕХ изменений
        [HttpPost]
        public IActionResult ProcessSelection([FromBody] SelectionRequest request)
        {
            // 1. Копируем текущее состояние из запроса
            var updatedCar = new ReflashCarModel
            {
                Brand = request.Car.Brand,
                Model = request.Car.Model,
                Generation = request.Car.Generation,
                Engine = request.Car.Engine
            };

            // 2. Обрабатываем изменение

            switch (request.ChangedField)
            {
                case "brand":
                    // Обновляем бренд
                    updatedCar.Brand = request.NewValue;
                    // Сбрасываем все что после бренда
                    updatedCar.Model = "";
                    updatedCar.Generation = "";
                    updatedCar.Engine = "";
                    break;

                case "model":
                    // Обновляем модель
                    updatedCar.Model = request.NewValue;
                    // Сбрасываем все что после модели
                    updatedCar.Generation = "";
                    updatedCar.Engine = "";
                    break;

                case "generation":
                    // Обновляем поколение
                    updatedCar.Generation = request.NewValue;
                    // Сбрасываем все что после поколения
                    updatedCar.Engine = "";
                    break;

                case "engine":
                    // Обновляем двигатель
                    updatedCar.Engine = request.NewValue;
                    break;

                default:

                    // Ничего не меняем (начальный запрос)
                    break;
            }
            if (string.IsNullOrEmpty(request.NewValue))
            {
                switch (request.ChangedField)
                {
                    case "model":
                        request.NewValue = updatedCar.Brand;
                        break;
                    case "generation":
                        request.NewValue = updatedCar.Model;
                        break;
                    case "engine":
                        request.NewValue = updatedCar.Generation;
                        break;
                    default:
                        break;
                }
            }
            // 3. Определяем какой дропдаун заполнять следующим
            string nextField = "";
            List<string> nextOptions = new List<string>();

            if (string.IsNullOrEmpty(updatedCar.Brand))
            {
                nextField = "brand";
                nextOptions = spisok_to_dropdown(nextField, "", "");
            }
            else if (string.IsNullOrEmpty(updatedCar.Model))
            {
                nextField = "model";
                nextOptions = spisok_to_dropdown(nextField, request.NewValue, "");
            }
            else if (string.IsNullOrEmpty(updatedCar.Generation))
            {
                nextField = "generation";
                nextOptions = spisok_to_dropdown(nextField, request.NewValue, "");
            }
            else if (string.IsNullOrEmpty(updatedCar.Engine))
            {
                nextField = "engine";
                
                nextOptions = spisok_to_dropdown(nextField, request.NewValue, updatedCar.Model);
            }
            else
            {
                nextField = "complete";
                nextOptions = new List<string>();
            }

            // 4. Возвращаем ответ
            return Json(new SelectionResponse
            {
                Car = updatedCar,
                NextField = nextField,
                Options = nextOptions
            });
        }

        // Получение начальных данных (тоже через ProcessSelection)
        [HttpGet]
        public IActionResult GetInitialData()
        {
            return Json(new SelectionResponse
            {
                Car = new ReflashCarModel(),
                NextField = "brand",
                Options = spisok_to_dropdown("brand", "", "")
            });
        }

        //Вызов странице генерирующейся по выбранной машине из базы данных
        public IActionResult Car(ReflashCarModel car)
        {
            // Декодируем значения (на случай если были спецсимволы)
            car.Brand = Uri.UnescapeDataString(car.Brand ?? "");
            car.Model = Uri.UnescapeDataString(car.Model ?? "");
            car.Generation = Uri.UnescapeDataString(car.Generation ?? "");
            car.Engine = Uri.UnescapeDataString(car.Engine ?? "");

            // Ищем автомобиль
            foreach (ReflashCarModel needed_car in DatabaseModel.Cars)
            {
                if ((needed_car.Brand == car.Brand) &&
                    (needed_car.Model == car.Model) &&
                    (needed_car.Generation == car.Generation) &&
                    (needed_car.Engine == car.Engine))
                {
                    return View(needed_car);
                }
            }

            // Если не нашли
            TempData["Error"] = "Автомобиль не найден";
            return RedirectToAction("Index");
        }


        //Партнёры

        public IActionResult Partners()
        {
            return View(DatabaseModel.Partners);
        }

        // Метод для получения списка партнеров из БД
        private List<PartnersModel> GetPartnersFromDatabase()
        {
            List<PartnersModel> partners = new List<PartnersModel>();
            string connectionString = _configuration.GetConnectionString("DefaultConnection")
                ?? "server=localhost;port=3306;database=reflash_cars;user=root;password=;";

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

                                // ВАЖНО: Если в БД сохранен полный путь, оставляем только имя файла
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
                                    photo = photoFileName, // Только имя файла
                                    vk = reader.IsDBNull(reader.GetOrdinal("vk_url")) ? "" : reader.GetString("vk_url"),
                                    website = reader.IsDBNull(reader.GetOrdinal("website_url")) ? "" : reader.GetString("website_url"),
                                    city = reader.IsDBNull(reader.GetOrdinal("city")) ? "" : reader.GetString("city"),
                                    street = reader.IsDBNull(reader.GetOrdinal("street")) ? "" : reader.GetString("street"),
                                    house = reader.IsDBNull(reader.GetOrdinal("house")) ? "" : reader.GetString("house"),
                                    longitude = reader.IsDBNull(reader.GetOrdinal("longitude")) ? "" : reader.GetString("longitude"),
                                    latitude = reader.IsDBNull(reader.GetOrdinal("latitude")) ? "" : reader.GetString("latitude")
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // новости
        
        public IActionResult News()
        {
            return View(DatabaseModel.News);
        }

        // Метод для получения списка нвостей из БД
        private List<NewsModel> Get_News_from_data()
        {
            List<NewsModel> news = new List<NewsModel>();
            string connectionString = _configuration.GetConnectionString("DefaultConnection")
                ?? "server=localhost;port=3306;database=reflash_cars;user=root;password=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM news ORDER BY news_name";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var photoUrl = reader.IsDBNull(reader.GetOrdinal("news_url")) ? "" : reader.GetString("news_url");

                                // ВАЖНО: Если в БД сохранен полный путь, оставляем только имя файла
                                string photoFileName = photoUrl;
                                if (!string.IsNullOrEmpty(photoUrl) && photoUrl.Contains("/"))
                                {
                                    photoFileName = Path.GetFileName(photoUrl);
                                }

                                news.Add(new NewsModel
                                {
                                    id = reader.GetInt32("id"),
                                    news_name = reader.IsDBNull(reader.GetOrdinal("news_name")) ? "" : reader.GetString("news_name"),
                                    news_text = reader.IsDBNull(reader.GetOrdinal("news_text")) ? "" : reader.GetString("news_text"),
                                    news_date = reader.IsDBNull(reader.GetOrdinal("news_date")) ? "" : reader.GetString("news_date"),
                                    news_url = photoFileName
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
            return news;
        }

        // главная страница
        //Полуение данных главной странцицы из бд
        private FurstPageModel GetFurstPageFromDatabase()
        {
            FurstPageModel furst_page = null;
            string connectionString = _configuration.GetConnectionString("DefaultConnection")
                ?? "server=localhost;port=3306;database=reflash ;user=root;password=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT SQL_NO_CACHE * FROM first_page_content LIMIT 1";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                furst_page = new FurstPageModel
                                {
                                    // Изображения
                                    image_1 = reader.IsDBNull(reader.GetOrdinal("image_1")) ? "" : reader.GetString("image_1"),
                                    image_2 = reader.IsDBNull(reader.GetOrdinal("image_2")) ? "" : reader.GetString("image_2"),
                                    image_3 = reader.IsDBNull(reader.GetOrdinal("image_3")) ? "" : reader.GetString("image_3"),
                                    image_4 = reader.IsDBNull(reader.GetOrdinal("image_4")) ? "" : reader.GetString("image_4"),

                                    // Заголовки (первый блок)
                                    head_1_ru = reader.IsDBNull(reader.GetOrdinal("head_1_ru")) ? "" : reader.GetString("head_1_ru"),
                                    head_1_eng = reader.IsDBNull(reader.GetOrdinal("head_1_eng")) ? "" : reader.GetString("head_1_eng"),
                                    head_1_ger = reader.IsDBNull(reader.GetOrdinal("head_1_ger")) ? "" : reader.GetString("head_1_ger"),

                                    // Заголовки (второй блок)
                                    head_2_ru = reader.IsDBNull(reader.GetOrdinal("head_2_ru")) ? "" : reader.GetString("head_2_ru"),
                                    head_2_eng = reader.IsDBNull(reader.GetOrdinal("head_2_eng")) ? "" : reader.GetString("head_2_eng"),
                                    head_2_ger = reader.IsDBNull(reader.GetOrdinal("head_2_ger")) ? "" : reader.GetString("head_2_ger"),

                                    // Основной текст
                                    text_ru = reader.IsDBNull(reader.GetOrdinal("text_ru")) ? "" : reader.GetString("text_ru"),
                                    text_eng = reader.IsDBNull(reader.GetOrdinal("text_eng")) ? "" : reader.GetString("text_eng"),
                                    text_ger = reader.IsDBNull(reader.GetOrdinal("text_ger")) ? "" : reader.GetString("text_ger"),

                                    // Блок 1
                                    block_1_ru = reader.IsDBNull(reader.GetOrdinal("block_1_ru")) ? "" : reader.GetString("block_1_ru"),
                                    block_1_eng = reader.IsDBNull(reader.GetOrdinal("block_1_eng")) ? "" : reader.GetString("block_1_eng"),
                                    block_1_ger = reader.IsDBNull(reader.GetOrdinal("block_1_ger")) ? "" : reader.GetString("block_1_ger"),

                                    // Блок 2
                                    block_2_ru = reader.IsDBNull(reader.GetOrdinal("block_2_ru")) ? "" : reader.GetString("block_2_ru"),
                                    block_2_eng = reader.IsDBNull(reader.GetOrdinal("block_2_eng")) ? "" : reader.GetString("block_2_eng"),
                                    block_2_ger = reader.IsDBNull(reader.GetOrdinal("block_2_ger")) ? "" : reader.GetString("block_2_ger"),

                                    // Блок 3
                                    block_3_ru = reader.IsDBNull(reader.GetOrdinal("block_3_ru")) ? "" : reader.GetString("block_3_ru"),
                                    block_3_eng = reader.IsDBNull(reader.GetOrdinal("block_3_eng")) ? "" : reader.GetString("block_3_eng"),
                                    block_3_ger = reader.IsDBNull(reader.GetOrdinal("block_3_ger")) ? "" : reader.GetString("block_3_ger"),

                                    // Блок 4
                                    block_4_ru = reader.IsDBNull(reader.GetOrdinal("block_4_ru")) ? "" : reader.GetString("block_4_ru"),
                                    block_4_eng = reader.IsDBNull(reader.GetOrdinal("block_4_eng")) ? "" : reader.GetString("block_4_eng"),
                                    block_4_ger = reader.IsDBNull(reader.GetOrdinal("block_4_ger")) ? "" : reader.GetString("block_4_ger"),

                                    // Блок 5
                                    block_5_ru = reader.IsDBNull(reader.GetOrdinal("block_5_ru")) ? "" : reader.GetString("block_5_ru"),
                                    block_5_eng = reader.IsDBNull(reader.GetOrdinal("block_5_eng")) ? "" : reader.GetString("block_5_eng"),
                                    block_5_ger = reader.IsDBNull(reader.GetOrdinal("block_5_ger")) ? "" : reader.GetString("block_5_ger"),

                                    // Блок 6
                                    block_6_ru = reader.IsDBNull(reader.GetOrdinal("block_6_ru")) ? "" : reader.GetString("block_6_ru"),
                                    block_6_eng = reader.IsDBNull(reader.GetOrdinal("block_6_eng")) ? "" : reader.GetString("block_6_eng"),
                                    block_6_ger = reader.IsDBNull(reader.GetOrdinal("block_6_ger")) ? "" : reader.GetString("block_6_ger")
                                };
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Логирование ошибки
                Console.WriteLine($"Ошибка MySQL при загрузке данных главной страницы: {ex.Message}");
                // Если используете TempData в контроллере
                // TempData["Error"] = $"Ошибка MySQL: {ex.Message}";
            }

            return furst_page;
        }
        // Добавьте этот метод в конец класса HomeController (перед последней скобкой)

        [HttpPost("/api/db-notify")]
        public IActionResult DbNotify()
        {
            try
            {
                Console.WriteLine($"📡 [{DateTime.Now}] Сигнал от админки получен");

                // Сбрасываем кэш
                RefreshData();

                // Перезагружаем главную страницу
                DatabaseModel.furst_page = GetFurstPageFromDatabase();

                Console.WriteLine($"✅ [{DateTime.Now}] Кэш очищен");

                return Ok(new { success = true, message = "Cache cleared" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }
    }

    // Запрос от клиента: 
    public class SelectionRequest
    {
        public ReflashCarModel Car { get; set; } = new ReflashCarModel();  // 1. Текущая машина
        public string ChangedField { get; set; } = "";       // 2. Какое поле изменили
        public string NewValue { get; set; } = "";           // 3. На что изменили

    }

    // Ответ сервера: 
    public class SelectionResponse
    {
        public ReflashCarModel Car { get; set; } = new ReflashCarModel();  // 1. Обновленная машина
        public string NextField { get; set; } = "";          // 2. Какое поле заполнять следующим
        public List<string> Options { get; set; } = new List<string>(); // Список для дропдауна
    }

}

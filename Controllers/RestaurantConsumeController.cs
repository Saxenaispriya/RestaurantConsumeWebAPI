using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantConsumeWebAPI.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace RestaurantConsumeWebAPI.Controllers
{
    public class RestaurantConsumeController : Controller
    {
        // public  HttpClient _client=new HttpClient();
        private readonly HttpClient _httpClient;

        public RestaurantConsumeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5236/api/");
        }
        
        [HttpGet]
        public IActionResult AddTable()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddTable(int tablenumber, bool isTableOccupied=false)
        {
            Table addtables = new Table();
            addtables.TableNumber = tablenumber;
            addtables.IsTableOccupied = isTableOccupied;
            StringContent content = new StringContent(JsonConvert.SerializeObject(addtables), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Restaurant/addTableRoute", content);
                if(!response.IsSuccessStatusCode)
                {
                    ViewBag.Error = "Failed to Add Table";
                    return View();
                }
            return RedirectToAction("Index", "RestaurantConsume");
        }
        public async Task<IActionResult> ShowTable()
        {
            List<Table> tables = new List<Table>();
            using (var response = await _httpClient.GetAsync("Restaurant/AvailabletableRoute"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                tables = JsonConvert.DeserializeObject<List<Table>>(apiResponse);
            }

            return View(tables);
        }

        [HttpGet]
        public IActionResult DoOrders()
        {
            return View(new DoOrderRequestPayload()); // Initialize empty model for the view
        }

        [HttpPost]
        public async Task<IActionResult> DoOrders(DoOrderRequestPayload doOrderRequestPayload)
        {
            string apiUrl = "Restaurant/do_OrderRoute";

            // Serialize model to JSON
            string jsonPayload = JsonConvert.SerializeObject(doOrderRequestPayload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Log the request payload (for debugging)
            Console.WriteLine("Sending Request: " + jsonPayload);

            HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, content);

            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Response: " + responseContent);
            return View();

        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GenerateBill()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GenerateBill(int tablenumber)
        {
            int bill = 0;
            _httpClient.BaseAddress = new Uri("Restaurant/");
            string s = string.Format("generatebillRoute?tablenumber={0}", tablenumber);
            HttpResponseMessage response = await _httpClient.GetAsync(s);
            if(!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Bill not found for Table {tablenumber} .";
            }
            string jsonResponse=await response.Content.ReadAsStringAsync();
            bill=int.Parse(jsonResponse);
            ViewBag.BillAmount = bill;
            ViewBag.TableNumber = tablenumber;
            //Console.WriteLine("this is the response", response);
            return View();
        }

        public async Task<IActionResult> showFoodItem()
        {
            List<Food> foodList = new List<Food>();
            var response = await _httpClient.GetAsync("Restaurant/MenuesRoute");
            string jsonResponse = await response.Content.ReadAsStringAsync();
            foodList=JsonConvert.DeserializeObject<List<Food>>(jsonResponse);
            return View(foodList);
        }

        [HttpGet]
        public async Task<IActionResult> AddFoodItem()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFoodItem(foodRequestPayload food)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(food), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Restaurant/addFoodItemRoute", content);
            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Failed to Add Table";
                return View();
            }
            return RedirectToAction("Index", "RestaurantConsume");
        }

        [HttpPost]
        public async Task<IActionResult> deleteFoodItem(int id)
        {
           // _httpClient.BaseAddress = new Uri("http://localhost:5236/api/");
            var response = await _httpClient.DeleteAsync($"Restaurant/deleteFoodItemRoute?_fooditem={id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Food item deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete food item.";
            }

            return RedirectToAction("ShowFoodItem", "RestaurantConsume"); // Redirect back to the grid
        }

    }
}

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
        public  HttpClient _client=new HttpClient();
        //public RestaurantConsumeController(HttpClient client)
        //{
        //    _client = client;
        //    _client.BaseAddress = new Uri("http://localhost:5236/");
        //}
        public async Task<IActionResult> AddTable(int tablenumber, bool isTableOccupied)
        {
            Table addtables = new Table();
            addtables.TableNumber = tablenumber;
            addtables.IsTableOccupied = isTableOccupied;
            StringContent content = new StringContent(JsonConvert.SerializeObject(addtables), Encoding.UTF8, "application/json");
            using (var response = await _client.PostAsync("http://localhost:5236/api/Restaurant/addTableRoute", content))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                addtables = JsonConvert.DeserializeObject<Table>(apiResponse);
            }
            return View(addtables);
        }
        public async Task<IActionResult> ShowTable()
        {
            List<Table> tables = new List<Table>();
            using (var response = await _client.GetAsync("http://localhost:5236/api/Restaurant/AvailabletableRoute"))
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
            string apiUrl = "http://localhost:5236/api/Restaurant/do_OrderRoute";

            // Serialize model to JSON
            string jsonPayload = JsonConvert.SerializeObject(doOrderRequestPayload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Log the request payload (for debugging)
            Console.WriteLine("Sending Request: " + jsonPayload);

            HttpResponseMessage response = await _client.PostAsync(apiUrl, content);

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
            _client.BaseAddress = new Uri("http://localhost:5236/api/Restaurant/");
            string s = string.Format("generatebillRoute?tablenumber={0}", tablenumber);
            HttpResponseMessage response = await _client.GetAsync(s);
            Console.WriteLine("this is the response", response);
            return View();
        }
    }
}

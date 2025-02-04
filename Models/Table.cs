using Newtonsoft.Json;

namespace RestaurantConsumeWebAPI.Models
{
    public class Table
    {
        public int TableNumber {  get; set; }
        public bool IsTableOccupied {  get; set; }

        [JsonProperty("orderlist")]
        public List<Order> orderlist = new List<Order>();
    }
}

using Newtonsoft.Json;

namespace RestaurantConsumeWebAPI.Models
{
    public class DoOrderRequestPayload
    {
        [JsonProperty("tablenumber")]
        public int tablenumber {  get; set; }

        [JsonProperty("orderslst")]
        public List<Order> orderslst { get; set; }
    }
}



using System.Text.Json.Serialization;

namespace webApp.Models
{
    public class Market
    {
        public int Id { get; set; }
        public string title { get; set; }
        public int user_id { get; set; }
        public int balance { get; set; }
        public string image { get; set; }
        public int field_id { get; set; }
        public double lat { get; set; }
        public int isClosed { get; set; }
        public double lng { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string mobile { get; set; }
        public string status { get; set; }
        public string summary { get; set; }
        public int rate { get; set; }
        public int order_count { get; set; }
        public int picks { get; set; }

    }
}

using System;
namespace webApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int address_id { get; set; }
        public int market_id { get; set; }
        public int price { get; set; }
        public int user_id { get; set; }
        public int delivery_id { get; set; }
        public int status { get; set; }

        public double market_lat { get; set; }
        public double market_lng { get; set; }
        public double user_lat { get; set; }
        public double user_lng { get; set; }
        public double market_distance { get; set; }
        public string username { get; set; }
        public string marketname { get; set; }
        public string marketimage { get; set; }
        public int market_rate { get; set; }
        public DateTime date { get; set; }

    }
}

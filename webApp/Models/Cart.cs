using System;
namespace webApp.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int price { get; set; }
        public int user_id { get; set; }
        public int quantity { get; set; }
        public int food_id { get; set; }
        public int order_id { get; set; }
        public string status { get; set; }
    }
}

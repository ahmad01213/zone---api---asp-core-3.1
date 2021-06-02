using System;
using System.Collections.Generic;

namespace webApp.Dtos
{
    public class CartAddRequest
    {
        public int user_id { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
        public int food_id { get; set; }
        public Dictionary<String,String> options { get; set; }
    }
}

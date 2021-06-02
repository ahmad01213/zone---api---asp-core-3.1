using System;
namespace webApp.Models
{
    public class Food
    {
        public int Id { get; set; }
        public int picks { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public string desc { get; set; }
        public int price { get; set; }
        public int category_id { get; set; }
        public int market_id { get; set; }
   
    }
}

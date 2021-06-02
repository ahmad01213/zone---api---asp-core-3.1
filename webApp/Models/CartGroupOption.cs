using System;
namespace webApp.Models
{
    public class CartGroupOption
    {
        public int Id { get; set; }
        public int cart_id { get; set; }
        public int group_id { get; set; }
        public int option_id { get; set; }
    }
}

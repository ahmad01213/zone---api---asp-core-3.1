using System;
namespace webApp.Models
{
    public class Address
    {
        public int Id { get; set; }
        public int user_id { get; set; }
        public string city { get; set; }
        public string street { get; set; }
        public int block { get; set; }
        public int building { get; set; }
        public int floor { get; set; }
        public int flat_no { get; set; }
        public string additional_directions { get; set; }
        public string lable { get; set; }
        public string phone { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
    }
}

using System;
namespace webApp.Models
{
    public class Driver
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string image { get; set; }
        public string role { get; set; }
        public int rate { get; set; }
        public int reviews { get; set; }
        public int user_id { get; set; }
        public int order_count { get; set; }
        public int balance { get; set; }
        public string status { get; set; }
        public string address { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public virtual string device_token { get; set; }
    }
}

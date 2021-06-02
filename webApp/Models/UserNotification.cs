using System;
namespace webApp.Models
{
    public class UserNotification
    {
        public int Id { get; set; }
        public int user_id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public string image { get; set; }
        public DateTime date { get; set; }
    }
}

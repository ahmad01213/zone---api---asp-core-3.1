using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;


namespace API.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }

        public virtual string password { get; set; }
        public string image { get; set; }
        public string role { get; set; }

        public virtual string device_token { get; set; }

    }
}

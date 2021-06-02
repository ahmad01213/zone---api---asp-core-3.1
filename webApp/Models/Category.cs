using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace webApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public string status { get; set; }
    }
}

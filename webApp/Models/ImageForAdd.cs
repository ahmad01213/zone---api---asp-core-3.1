using System;
using Microsoft.AspNetCore.Http;

namespace webApp.Models
{
    public class ImageForAdd
    {
        public IFormFile file { get; set; }
    }
}

using System;
using System.Collections.Generic;
using webApp.Models;

namespace webApp.Dtos
{
    public class MarketDetailResponse
    {
        public Market market { get; set; }
        public double distance { get; set; }
        public List<Order> orders { get; set; }
    }
}

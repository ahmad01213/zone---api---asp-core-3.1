using System;
namespace webApp.Dtos
{
    public class MarketDetailRequest
    {
        public double lat { get; set; }
        public double lng { get; set; }
        public int marketId { get; set; }

    }
}

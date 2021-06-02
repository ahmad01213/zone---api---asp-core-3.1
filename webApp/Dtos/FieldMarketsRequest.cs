using System;
namespace webApp.Dtos
{
    public class FieldMarketsRequest
    {
        public int FieldId { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
    }
}

using System;
namespace webApp.Dtos
{
    public class OrderStatusRequest
    {
        public int deliveryId { get; set; }
        public int orderId { get; set; }
        public int status { get; set; }
    }
}

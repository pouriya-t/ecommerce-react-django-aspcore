using Application.Users;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders
{
    public class OrderCreatedVM
    {
        public int _id { get; set; }
        public OrderItemDto[] OrderItems { get; set; }
        public dynamic ShippingAddress { get; set; }
        public UserDto User { get; set; }
        public string PaymentMethod { get; set; }
        public double TaxPrice { get; set; }
        public double ShippingPrice { get; set; }
        public double TotalPrice { get; set; }
        public bool IsPaid { get; set; }
        public DateTime PaidAt { get; set; }
        public bool IsDelivered { get; set; }
        public DateTime DeliveredAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

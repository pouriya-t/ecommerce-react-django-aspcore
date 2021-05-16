using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Order
    {
        [Key]
        public int _id { get; set; }
        public string PaymentMethod { get; set; }
        public double TaxPrice { get; set; }
        public double ShippingPrice { get; set; }
        public double TotalPrice { get; set; }
        public bool IsPaid { get; set; }
        public DateTime PaidAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime DeliveredAt { get; set; } = DateTime.Now;
        public bool IsDelivered { get; set; }


        public string user_id { get; set; }
        [ForeignKey("user_id")]
        public virtual User User { get; set; }


    }
}

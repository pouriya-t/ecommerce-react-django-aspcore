using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ShippingAddress
    {
        [Key]
        public int _id { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public double ShippingPrice { get; set; }

        public int order_id { get; set; }
        [ForeignKey("order_id")]
        public virtual Order Order { get; set; }

    }
}

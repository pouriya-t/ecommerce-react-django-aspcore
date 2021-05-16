using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class OrderItem
    {
        [Key]
        public int _id { get; set; }
        public string Name { get; set; }
        public int Qty { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }

        public int product_id { get; set; }
        [ForeignKey("product_id")]
        public virtual Product Product{ get; set; }


        public int order_id { get; set; }
        [ForeignKey("order_id")]
        public virtual Order Order { get; set; }

        

    }
}

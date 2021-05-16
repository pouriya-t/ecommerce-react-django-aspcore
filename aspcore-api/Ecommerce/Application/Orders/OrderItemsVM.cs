using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders
{
    public class OrderItemsVM
    {
        public int _id { get; set; }
        public int Product { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public int CountInStock { get; set; }
        public int Qty { get; set; }
    }
}

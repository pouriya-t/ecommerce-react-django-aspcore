using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products
{
    public class ProductDto
    {
        public int _id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public string brand { get; set; }
        public string category { get; set; }
        public string description { get; set; }
        public double rating { get; set; }
        public int numReviews { get; set; }
        public double price { get; set; }
        public int countInStock { get; set; }
        public DateTime createdAt { get; set; }
        public string user { get; set; }
    }
}

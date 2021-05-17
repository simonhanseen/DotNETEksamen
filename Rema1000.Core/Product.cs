using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rema1000.Core
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public int AmountInPackage { get; set; }
        public double Price { get; set; }
        public double? Discount {get; set;}
        public Category Category { get; set; }
        public int AmountInStock { get; set; }
        public Supplier Supplier { get; set; }

    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int AmountInStock { get; set; }

        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

    }
}

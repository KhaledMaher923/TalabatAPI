using System;
using System.Collections.Generic;
using System.Text;

namespace Talabat.Core.Entities.Order_Aggregate
{
    public class OrderItem : BaseEntity
    {
        public ProductItemOrdered Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

    }
}

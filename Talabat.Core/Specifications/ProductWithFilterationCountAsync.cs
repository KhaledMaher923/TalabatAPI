using System;
using System.Collections.Generic;
using System.Text;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithFilterationCountAsync : BaseSpecifications<Product>
    {
        public ProductWithFilterationCountAsync(ProductSpecParams Params)
            : base(P =>
            (!Params.BrandId.HasValue || P.ProductBrandId == Params.BrandId)
            &&
            (!Params.TypeId.HasValue || P.ProductTypeId == Params.TypeId)
            )
        {
            
        }
    }
}

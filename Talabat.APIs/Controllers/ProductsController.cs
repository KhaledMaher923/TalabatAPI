using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : APIBaseController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<ProductType> _typeRepo;
        private readonly IGenericRepository<ProductBrand> _brandRepo;

        public ProductsController(IGenericRepository<Product> ProductRepo,
                                  IMapper mapper,
                                  IGenericRepository<ProductType> TypeRepo,
                                  IGenericRepository<ProductBrand> BrandRepo
                                    )
        {
            _productRepo = ProductRepo;
            _mapper = mapper;
            _typeRepo = TypeRepo;
            _brandRepo = BrandRepo;
        }

        [HttpGet] 
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts(string? Sort)
         {
            var Spec = new ProductWithBrandAndTypeSpecifications(Sort);
            var products = await _productRepo.GetAllWithSpecAsync(Spec);
            var MappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
            return Ok(MappedProducts);
        }
        [ProducesResponseType(typeof(ProductToReturnDto) ,StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse) ,StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var Spec = new ProductWithBrandAndTypeSpecifications(id);
            var product = await _productRepo.GetByIdWithSpecAsync(Spec);
            if (product == null) return NotFound(new ApiResponse(404));
            var MappedProduct = _mapper.Map<Product, ProductToReturnDto>(product);
            return Ok(MappedProduct);
        }

        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetTypes()
        {
            var types = await _typeRepo.GetAllAsync();
            return Ok(types);
        }

        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _brandRepo.GetAllAsync();
            return Ok(brands);
        }

    }
}   

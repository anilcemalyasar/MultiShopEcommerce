using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Catalog.Dtos.ProductImageDtos;
using MultiShop.Catalog.Services.ProductImageServices;

namespace MultiShop.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImagesController : ControllerBase
    {
        private readonly IProductImageService _productImageService;

        public ProductImagesController(IProductImageService productImageService)
        {
            _productImageService = productImageService;
        }


        [HttpGet]
        public async Task<IActionResult> ProductImageList()
        {
            var productImages = await _productImageService.GetAllProductImagesAsync();
            return Ok(productImages);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetProductImageById(string id)
        {
            var productImage = await _productImageService.GetByIdProductImageAsync(id);
            return Ok(productImage);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductImage(CreateProductImageDto createProductImageDto)
        {
            await _productImageService.CreateProductImageAsync(createProductImageDto);
            return Ok("Ürün Görseli başarıyla eklendi!");
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteProductImage(string id)
        {
            await _productImageService.DeleteProductImageAsync(id);
            return Ok("Ürün görseli başarıyla silindi!");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProductImage(UpdateProductImageDto updateProductImageDto)
        {
            await _productImageService.UpdateProductImageAsync(updateProductImageDto);
            return Ok("Ürün görseli başarıyla güncellendi!");
        }



    }
}

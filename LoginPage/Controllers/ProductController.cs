using LoginPage.Interface;
using LoginPage.Model;
using Microsoft.AspNetCore.Mvc;

namespace LoginPage.Controllers
{

    [ApiController]
    [Route("ProductControllerApi")]

    public class ProductController : ControllerBase
    {
        private readonly IProducts _product;

        public ProductController(IProducts product)
        {
            _product = product;
        }


        [HttpPost("Add-Product")]
        public async Task<IActionResult> AddProductsAsync(Product product)
        {
            var result = await _product.AddProductsAsync(product);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("Failed to add product");
        }

        [HttpGet("Total-Product")]

        public async Task<IActionResult> TotalProductAsync()
        {
            var totalProducts = await _product.TotalProductAsync();

            return Ok(totalProducts);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductAsync(int id, Product product)
        {
            var UpdateProduct = await _product.UpdateProductAsync(id,product);

            if(UpdateProduct == null)
            {
                return NotFound("Id not found");
            }

            return Ok(UpdateProduct);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>DeleteProductAsynce(int id)
        {
            var deleteProducts = await _product.DeleteProductAsync(id);

            if(deleteProducts == null)
            {
                return NotFound("Product not found");
            }

            return Ok(deleteProducts);
        }



    }
}

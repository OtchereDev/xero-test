using System;
using System.Threading.Tasks;
using System.Web.Http;
using refactor_this.Models;
using refactor_this.Services;

namespace refactor_this.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        private readonly IProductServices _service;

        public ProductsController(IProductServices service)
        {
            _service = service;
        }

        [Route]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllAsync(string name = null)
        {
            try
            {
                var products = await _service.GetAllProductsAsync(name);
                return Ok(new {Items= products});
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [Route("{id:guid}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetProductAsync(Guid id)
        {
            try
            {
                var product = await _service.GetProductAsync(id);

                if (product == null)
                    return NotFound();

                return Ok(product);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [Route]
        [HttpPost]
        public async Task<IHttpActionResult> CreateAsync([FromBody] Product product)
        {
            if (product == null)
                return BadRequest("Product cannot be null.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _service.CreateProductAsync(product);
                return Created(Request.RequestUri, new { message = "Product successfully created" });
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [Route("{id:guid}")]
        [HttpPut]
        public async Task<IHttpActionResult> Update(Guid id, [FromBody] Product product)
        {
            if (product == null)
                return BadRequest("Product cannot be null.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedProduct = await _service.UpdateProductAsync(product, id);

                if (updatedProduct == null)
                    return NotFound();

                return Ok(new { message = "Product successfully updated" });
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [Route("{id:guid}")]
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            try
            {
                var deleted = await _service.DeleteProductAsync(id);

                if (deleted == null)
                    return NotFound();

                return Ok(new { message = "Product successfully deleted" });
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
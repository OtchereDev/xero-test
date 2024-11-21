using System;
using System.Threading.Tasks;
using System.Web.Http;
using refactor_this.Models;
using refactor_this.Services;

namespace refactor_this.Controllers
{
    [RoutePrefix("products")]
    public class ProductOptionsController : ApiController
    {
        private readonly IProductOptionsServices _service;
        
        public ProductOptionsController(IProductOptionsServices productOptionsService)
        {
            _service = productOptionsService;
        }

        [Route("{productId}/options")]
        [HttpGet]
        public async Task<IHttpActionResult> GetOptions(Guid productId)
        {
            try
            {
                var options = await _service.GetProductOptionsAsync(productId.ToString());
                return Ok(options);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("{productId}/options/{id:guid}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetOption(Guid productId, Guid id)
        {
            try
            {
                var option = await _service.GetProductOptionByIdAsync(id);
                if (option == null)
                    return NotFound();

                return Ok(option);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("{productId}/options")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateOption(Guid productId, ProductOption option)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdOption = await _service.CreateProductOptionAsync(option, productId);
                return Created($"{Request.RequestUri}/{createdOption.Id}", new { message = "Option created successfully" });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("{productId}/options/{id:guid}")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateOption(Guid id, ProductOption option)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedOption = await _service.UpdateProductOptionAsync(option, id);
                if (updatedOption == null)
                    return NotFound();

                return Ok(new { message = "Option updated successfully" });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("{productId}/options/{id:guid}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteOption(Guid id)
        {
            try
            {
                var deletedOption = await _service.DeleteProductOptionAsync(id);
                if (deletedOption == null)
                    return NotFound();

                return Ok(new { message = "Option deleted successfully" });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
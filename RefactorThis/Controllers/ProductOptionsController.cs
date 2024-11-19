using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using refactor_this.Models;
using refactor_this.Services;

namespace refactor_this.Controllers
{
    [RoutePrefix("products")]
    public class ProductOptionsController: ApiController
    {
        private readonly ProductOptionsServices _service;
        
        public ProductOptionsController(ProductOptionsServices productOptionsService)
        {
            _service = productOptionsService;
        }

        [Route("{productId}/options")]
        [HttpGet]
        public IReadOnlyList<ProductOption> GetOptions(Guid productId)
        {
            return _service.GetProductOptions(productId.ToString());
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public ProductOption GetOption(Guid productId, Guid id)
        {
            var option = _service.GetProductOptionById(id);
            
            if (option == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return option;
        }

        [Route("{productId}/options")]
        [HttpPost]
        public IHttpActionResult CreateOption(Guid productId, ProductOption option)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _service.CreateProductOption(option, productId);
                return Json(new {message = "Option created successfully"});
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [Route("{productId}/options/{id}")]
        [HttpPut]
        public IHttpActionResult UpdateOption(Guid id, ProductOption option)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            try
            {
                var result = _service.UpdateProductOption(option, id);
                
                if (result == null)
                    return NotFound();
                
                return Json(new {message = "Option updated successfully"});
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public void DeleteOption(Guid id)
        {
            var result = _service.DeleteProductOption(id);
            
            if (result == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
        }
    }
}
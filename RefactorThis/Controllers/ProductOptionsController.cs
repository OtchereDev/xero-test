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
        [Route("{productId}/options")]
        [HttpGet]
        public List<ProductOption> GetOptions(Guid productId)
        {
            return new ProductOptionsServices().GetProductOptions(productId);
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public ProductOption GetOption(Guid productId, Guid id)
        {
            var service = new ProductOptionsServices();

            var option = service.GetProductOptionById(id);
            
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
                var service = new ProductOptionsServices();
                service.CreateProductOption(option, productId);
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
            
            var service = new ProductOptionsServices();

            try
            {
                var result = service.UpdateProductOption(option, id);
                
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
            var service = new ProductOptionsServices();
            var result = service.DeleteProductOption(id);
            
            if (result == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
        }
    }
}
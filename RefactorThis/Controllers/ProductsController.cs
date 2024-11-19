using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using refactor_this.Models;
using refactor_this.Services;


namespace refactor_this.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        [Route]
        [HttpGet]
        public List<Product> GetAll()
        {
            var service = new ProductServices();

            return service.GetAllProducts();
        }

        [Route]
        [HttpGet]
        public List<Product> SearchByName(string name)
        {
            var service = new ProductServices();

            return service.GetAllProducts(name);
        }

        [Route("{id}")]
        [HttpGet]
        public Product GetProduct(Guid id)
        {
            var service = new ProductServices();
            var product = service.GetProduct(id);
            
            if (product == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return product;
        }

        [Route]
        [HttpPost]
        public IHttpActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var service = new ProductServices();
                service.CreateProduct(product);
                return Json(new { message = "Product successfully created"});
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            
        }

        [Route("{id}")]
        [HttpPut]
        public IHttpActionResult Update(Guid id, Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var service = new ProductServices();
                var result = service.UpdateProduct(product, id);
                
                if(result == null)
                    return NotFound();
                
                return Json(new { message = "Product successfully updated" });
                
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

        }

        [Route("{id}")]
        [HttpDelete]
        public void Delete(Guid id)
        {
            var service = new ProductServices();
            var result = service.DeleteProduct(id);
            
            if (result == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
        }
    }
}

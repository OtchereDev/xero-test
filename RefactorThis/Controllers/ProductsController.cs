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

        private ProductServices _service;

        public ProductsController(ProductServices service)
        {
            _service= service;
        }
        
        [Route]
        [HttpGet]
        public IReadOnlyList<Product> GetAll()
        {
            return _service.GetAllProducts();
        }

        [Route]
        [HttpGet]
        public IReadOnlyList<Product> SearchByName(string name)
        {
            return _service.GetAllProducts(name);
        }

        [Route("{id}")]
        [HttpGet]
        public Product GetProduct(Guid id)
        {
            var product = _service.GetProduct(id);
            
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
                _service.CreateProduct(product);
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
                var result = _service.UpdateProduct(product, id);
                
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
            var result = _service.DeleteProduct(id);
            
            if (result == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
        }
    }
}

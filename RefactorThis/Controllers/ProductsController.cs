﻿using System;
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
        public ProductServices GetAll()
        {
            return new ProductServices();
        }

        [Route]
        [HttpGet]
        public ProductServices SearchByName(string name)
        {
            return new ProductServices(name);
        }

        [Route("{id}")]
        [HttpGet]
        public Product GetProduct(Guid id)
        {
            var repo = new ProductRepository();
            var product = repo.GetById(id);
            if (product == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return product;
        }

        [Route]
        [HttpPost]
        public void Create(Product product)
        {
            var repo = new ProductRepository();
            repo.Save(product);
        }

        [Route("{id}")]
        [HttpPut]
        public void Update(Guid id, Product product)
        {
            var repo = new ProductRepository();
            var orig = repo.GetById(id);
    

            if (orig != null)
            {
                orig.Name = product.Name;
                orig.Description = product.Description;
                orig.Price = product.Price;
                orig.DeliveryPrice = product.DeliveryPrice;
                
                repo.Save(orig);
            }
               
        }

        [Route("{id}")]
        [HttpDelete]
        public void Delete(Guid id)
        {
            var repo = new ProductRepository();
            var product = repo.GetById(id);
            repo.Delete(product);
        }
    }
}

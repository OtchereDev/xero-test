using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using refactor_this.Models;

namespace refactor_this.Services
{
    
    public class ProductServices
    {
        private ProductRepository _repository;

        public ProductServices(ProductRepository repository)
        {
            _repository = repository;
        }

        public IReadOnlyList<Product> GetAllProducts(string name = null)
        {
            return _repository.GetAll(name);
        }
        
        public void CreateProduct(Product product)
        {
            _repository.Save(product);
        }

        public Product UpdateProduct(Product product, Guid productId)
        {
            
            var orig = _repository.GetById(productId);

            if (orig == null)
                return null;
            
            orig.Name = product.Name;
            orig.Description = product.Description;
            orig.Price = product.Price;
            orig.DeliveryPrice = product.DeliveryPrice;
            
            _repository.Save(orig);
            
            return orig;
        }

        public Product GetProduct(Guid productId)
        {
            var product = _repository.GetById(productId);
            
            return product;
        }

        public Product DeleteProduct(Guid productId)
        {
            var product = this.GetProduct(productId);
            
            if (product == null)
                return null;
            
            _repository.Delete(product);
            
            return product;
        }
    }
}
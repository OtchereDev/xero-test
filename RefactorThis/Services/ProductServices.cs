using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using refactor_this.Models;

namespace refactor_this.Services
{
    
    public class ProductServices : IProductServices
    {
        private readonly IProductRepository _repository;

        public ProductServices(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<Product>> GetAllProductsAsync(string name = null)
        {
            return await _repository.GetAllAsync(name);
        }
        
        public async Task<Product> CreateProductAsync(Product product)
        {
             await _repository.SaveAsync(product);
             return product;
        }

        public async Task<Product> UpdateProductAsync(Product product, Guid productId)
        {
            
            var orig = await _repository.GetByIdAsync(productId);

            if (orig == null)
                return null;
            
            orig.Name = product.Name;
            orig.Description = product.Description;
            orig.Price = product.Price;
            orig.DeliveryPrice = product.DeliveryPrice;
            
            await _repository.UpdateAsync(orig);
            
            return orig;
        }

        public async Task<Product> GetProductAsync(Guid productId)
        {
            var product = await _repository.GetByIdAsync(productId);
            
            return product;
        }

        public async Task<Product> DeleteProductAsync(Guid productId)
        {
            var product = await GetProductAsync(productId);
            
            if (product == null)
                return null;
            
            await _repository.DeleteAsync(product);
            
            return product;
        }
    }
}
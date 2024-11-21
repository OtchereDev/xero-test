using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using refactor_this.Models;

namespace refactor_this.Services
{
    public interface IProductServices
    {
        Task<IReadOnlyList<Product>> GetAllProductsAsync(string name = null);
        Task<Product> CreateProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product, Guid productId);
        Task<Product> GetProductAsync(Guid productId);
        Task<Product> DeleteProductAsync(Guid productId);
    }
}
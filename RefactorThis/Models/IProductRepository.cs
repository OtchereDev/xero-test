using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace refactor_this.Models
{
    public interface IProductRepository
    {
        Task SaveAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
        Task<Product> GetByIdAsync(Guid id);
        Task<IReadOnlyList<Product>> GetAllAsync(string name);
    }
}
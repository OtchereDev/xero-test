using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace refactor_this.Models
{
    public interface IProductOptionRepository
    {
        Task SaveAsync(ProductOption productOption);
        Task UpdateAsync(ProductOption productOption);
        Task<ProductOption> GetByIdAsync(Guid id);
        Task DeleteAsync(ProductOption productOption);
        Task DeleteByProductIdAsync(Guid productId);
        Task<IReadOnlyList<ProductOption>> GetAllAsync(string productId);
    }
}
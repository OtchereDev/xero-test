using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using refactor_this.Models;

namespace refactor_this.Services
{
    public interface IProductOptionsServices
    {
        Task<IReadOnlyList<ProductOption>> GetProductOptionsAsync(string productId = null);
        Task<ProductOption> CreateProductOptionAsync(ProductOption productOption, Guid productId);
        Task<ProductOption> UpdateProductOptionAsync(ProductOption productOption, Guid productId);
        Task<ProductOption> DeleteProductOptionAsync(Guid optionId);
        Task<ProductOption> GetProductOptionByIdAsync(Guid productOptionId);
    }
}
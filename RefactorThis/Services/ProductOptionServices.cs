using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using refactor_this.Models;

namespace refactor_this.Services
{
    public class ProductOptionsServices
    {
        private readonly ProductOptionRepository _repository;
        
        public ProductOptionsServices(ProductOptionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<ProductOption>> GetProductOptionsAsync(string productId = null)
        {
            return await _repository.GetAllAsync(productId);
        }

        public async Task<ProductOption> CreateProductOptionAsync(ProductOption productOption, Guid productId)
        {
            productOption.ProductId = productId;
            await _repository.SaveAsync(productOption);
            
            return productOption;
        }

        public async Task<ProductOption> UpdateProductOptionAsync(ProductOption productOption, Guid productId)
        {
            var orig = await _repository.GetByIdAsync(productId);

            if (orig == null)
                return null;
            
            orig.Name = productOption.Name;
            orig.Description = productOption.Description;
            await _repository.SaveAsync(orig);
            
            return orig;
        }

        public async Task<ProductOption> DeleteProductOptionAsync(Guid optionId)
        {
            var opt = await _repository.GetByIdAsync(optionId);
            
            if (opt == null)
                return null;
            
            await _repository.DeleteAsync(opt);
            
            return opt;
        }

        public async Task<ProductOption> GetProductOptionByIdAsync(Guid productOptionId)
        {
            return await _repository.GetByIdAsync(productOptionId);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

        public IReadOnlyList<ProductOption> GetProductOptions(string productId = null)
        {
            return _repository.GetAll(productId);
        }

        public ProductOption CreateProductOption(ProductOption productOption, Guid productId)
        {
            productOption.ProductId = productId;
            _repository.Save(productOption);
            
            return productOption;
        }

        public ProductOption UpdateProductOption(ProductOption productOption, Guid productId)
        {

            var orig = _repository.GetById(productId);

            if (orig == null)
                return null;
            
            orig.Name = productOption.Name;
            orig.Description = productOption.Description;
            _repository.Save(orig);
            
            return orig;
        }

        public ProductOption DeleteProductOption(Guid optionId)
        {
            var opt = _repository.GetById(optionId);
            
            if (opt == null)
                return null;
            
            _repository.Delete(opt);
            
            return opt;
        }

        public ProductOption GetProductOptionById(Guid productOptionId)
        {
            var option = _repository.GetById(productOptionId);
            return option;
        }
    }
}
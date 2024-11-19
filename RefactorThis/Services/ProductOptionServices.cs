using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using refactor_this.Models;

namespace refactor_this.Services
{
    public class ProductOptionsServices
    {
        private readonly ProductOptionRepository _repository;
        
        public ProductOptionsServices()
        {
            _repository = new ProductOptionRepository();
        }


        public List<ProductOption> GetProductOptions()
        {
            return LoadProductOptions(null);
        }

        public List<ProductOption> GetProductOptions(Guid productId)
        {
            return LoadProductOptions($"where productid = '{productId}'");
        }

        private List<ProductOption> LoadProductOptions(string where)
        {
            var items = new List<ProductOption>();
            var conn = Helpers.NewConnection();
            var cmd = new SqlCommand($"select id from productoption {where}", conn);
            conn.Open();

            var repo = new ProductOptionRepository();
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var id = Guid.Parse(rdr["id"].ToString());
                items.Add(repo.GetById(id));
            }
            
            return items;
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
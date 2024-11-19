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
            return LoadProductOptions(productId);
        }

        private IReadOnlyList<ProductOption> LoadProductOptions(string productId)
        {
            var items = new List<ProductOption>();
            var conn = Helpers.NewConnection();

            // Build query with parameterized SQL
            var query = "SELECT id FROM productoption";
            
            if (productId != null)
            {
                query += " WHERE productid = @productId";
            }

            var cmd = new SqlCommand(query, conn);
            if (productId != null)
            {
                cmd.Parameters.AddWithValue("@productId", productId);
            }

            conn.Open();
            
            try
            {
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var id = Guid.Parse(rdr["id"].ToString());
                    items.Add(_repository.GetById(id));
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An error occurred while loading product options.", ex);
            }
    
            return items.AsReadOnly();
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
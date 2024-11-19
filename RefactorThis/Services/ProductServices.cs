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
            return LoadProducts(name);
        }
        
        private IReadOnlyList<Product> LoadProducts(string name)
        {
            var items = new List<Product>();
            var conn = Helpers.NewConnection();

            var query = "SELECT id FROM product";
            if (!string.IsNullOrEmpty(name))
            {
                query += " WHERE lower(name) LIKE @name";
            }

            var cmd = new SqlCommand(query, conn);

            if (!string.IsNullOrEmpty(name))
            {
                cmd.Parameters.AddWithValue("@name", "%" + name.ToLower() + "%");
            }

            try
            {
                conn.Open();
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var id = Guid.Parse(rdr["id"].ToString());
                    items.Add(_repository.GetById(id));
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An error occurred while loading products.", ex);
            }

            return items.AsReadOnly();
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
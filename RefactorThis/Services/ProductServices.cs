using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using refactor_this.Models;

namespace refactor_this.Services
{
    public class ProductServices
    {
        public List<Product> Items { get; private set; }

        public List<Product> GetAllProducts()
        {
            LoadProducts(null);
            
            return this.Items;
        }

        public List<Product> GetAllProducts(string name)
        {
            LoadProducts($"where lower(name) like '%{name.ToLower()}%'");
            return this.Items;
        }

        private void LoadProducts(string where)
        {
            Items = new List<Product>();
            var conn = Helpers.NewConnection();
            var cmd = new SqlCommand($"select id from product {where}", conn);
            conn.Open();

            var rdr = cmd.ExecuteReader();
            var productRepo = new ProductRepository();

            while (rdr.Read())
            {
                var id = Guid.Parse(rdr["id"].ToString());
                Items.Add(productRepo.GetById(id));
            }
        }

        public void CreateProduct(Product product)
        {
            var repo = new ProductRepository();
            repo.Save(product);
        }

        public Product UpdateProduct(Product product, Guid productId)
        {
            var repo = new ProductRepository();
            
            var orig = repo.GetById(productId);

            if (orig == null)
                return null;
            
            orig.Name = product.Name;
            orig.Description = product.Description;
            orig.Price = product.Price;
            orig.DeliveryPrice = product.DeliveryPrice;
            
            repo.Save(orig);
            
            return orig;
        }

        public Product GetProduct(Guid productId)
        {
            var repo = new ProductRepository();
            
            var product = repo.GetById(productId);
            
            return product;
        }

        public Product DeleteProduct(Guid productId)
        {
            var repo = new ProductRepository();
            
            var product = this.GetProduct(productId);
            
            if (product == null)
                return null;
            
            repo.Delete(product);
            
            return product;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using refactor_this.Services;

namespace refactor_this.Models
{
    public class ProductRepository
    {
        private readonly IDatabase _database;

        public ProductRepository(IDatabase database)
        {
            _database = database;
        }
        
        public void Save(Product product)
        {
            var conn = _database.GetConnection();
            var cmd = product.IsNew ? 
                new SqlCommand($"insert into product (id, name, description, price, deliveryprice) values ('{product.Id}', '{product.Name}', '{product.Description}', {product.Price}, {product.DeliveryPrice})", conn) : 
                new SqlCommand($"update product set name = '{product.Name}', description = '{product.Description}', price = {product.Price}, deliveryprice = {product.DeliveryPrice} where id = '{product.Id}'", conn);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(Product product)
        {
            var optionsRepo = new ProductOptionRepository(_database);
            foreach (var option in new ProductOptionsServices(optionsRepo).GetProductOptions(product.Id.ToString()))
                optionsRepo.Delete(option);

            var conn = _database.GetConnection();
            conn.Open();
            var cmd = new SqlCommand($"delete from product where id = '{product.Id}'", conn);
            cmd.ExecuteNonQuery();
        }

        public Product GetById(Guid id)
        {
            var conn = _database.GetConnection();
            var cmd = new SqlCommand($"select * from product where id = '{id}'", conn);
            conn.Open();

            var rdr = cmd.ExecuteReader();
            if (!rdr.Read())
                return null;

            return new Product
            {
                Id = Guid.Parse(rdr["Id"].ToString()),
                Name = rdr["Name"].ToString(),
                Description = rdr["Description"] == DBNull.Value ? null : rdr["Description"].ToString(),
                Price = decimal.Parse(rdr["Price"].ToString()),
                DeliveryPrice = decimal.Parse(rdr["DeliveryPrice"].ToString())
            };
        }

        public IReadOnlyList<Product> GetAll(string name)
        {
            var items = new List<Product>();
            var conn = _database.GetConnection();

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
                    items.Add(GetById(id));
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An error occurred while loading products.", ex);
            }

            return items.AsReadOnly();
        }
    }
}
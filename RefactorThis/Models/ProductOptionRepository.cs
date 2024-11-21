using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace refactor_this.Models
{
    public class ProductOptionRepository
    {
        private  readonly IDatabase _database;

        public ProductOptionRepository(IDatabase database)
        {
            _database = database;
        }
        
        public void Save(ProductOption productOption)
        {
            var conn = _database.GetConnection();
            var cmd = productOption.IsNew ?
                new SqlCommand($"insert into productoption (id, productid, name, description) values ('{productOption.Id}', '{productOption.ProductId}', '{productOption.Name}', '{productOption.Description}')", conn.SqlConnection) :
                new SqlCommand($"update productoption set name = '{productOption.Name}', description = '{productOption.Description}' where id = '{productOption.Id}'", conn.SqlConnection);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
        
        public ProductOption GetById(Guid id)
        {
            var conn = _database.GetConnection();;
            var cmd = new SqlCommand($"select * from productoption where id = '{id}'", conn.SqlConnection);
            conn.Open();

            var rdr = cmd.ExecuteReader();
            if (!rdr.Read())
                return null;

            return new ProductOption
            {
                Id = Guid.Parse(rdr["Id"].ToString()),
                ProductId = Guid.Parse(rdr["ProductId"].ToString()),
                Name = rdr["Name"].ToString(),
                Description = rdr["Description"] == DBNull.Value ? null : rdr["Description"].ToString()
            };
        }
        
        public void Delete(ProductOption productOption)
        {
            var conn = _database.GetConnection();;
            var cmd = new SqlCommand($"delete from productoption where id = '{productOption.Id}'", conn.SqlConnection);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public IReadOnlyList<ProductOption> GetAll(string productId)
        {
            var items = new List<ProductOption>();
            var conn = _database.GetConnection();

            // Build query with parameterized SQL
            var query = "SELECT id FROM productoption";
            
            if (productId != null)
            {
                query += " WHERE productid = @productId";
            }

            var cmd = new SqlCommand(query, conn.SqlConnection);
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
                    items.Add(GetById(id));
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An error occurred while loading product options.", ex);
            }
    
            return items.AsReadOnly();
        }
    }
}
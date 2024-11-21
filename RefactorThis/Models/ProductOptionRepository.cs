using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace refactor_this.Models
{
    public class ProductOptionRepository
    {
        private readonly IDatabase _database;

        public ProductOptionRepository(IDatabase database)
        {
            _database = database;
        }

        public async Task SaveAsync(ProductOption productOption)
        {
            var conn = _database.GetConnection();
            await conn.Open();

            var query = productOption.IsNew
                ? "INSERT INTO productoption (id, productid, name, description) VALUES (@Id, @ProductId, @Name, @Description)"
                : "UPDATE productoption SET name = @Name, description = @Description WHERE id = @Id";

            using (var cmd = new SqlCommand(query, conn.SqlConnection))
            {
                cmd.Parameters.AddWithValue("@Id", productOption.Id);
                cmd.Parameters.AddWithValue("@ProductId", productOption.ProductId);
                cmd.Parameters.AddWithValue("@Name", productOption.Name);
                cmd.Parameters.AddWithValue("@Description", productOption.Description ?? (object)DBNull.Value);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<ProductOption> GetByIdAsync(Guid id)
        {
            var conn = _database.GetConnection();
            await conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM productoption WHERE id = @Id";
                cmd.Parameters.Add(new SqlParameter("@Id", id));

                using (var rdr =  cmd.ExecuteReader())
                {
                    if (! rdr.Read())
                        return null;

                    return new ProductOption
                    {
                        Id = Guid.Parse(rdr["Id"].ToString()),
                        ProductId = Guid.Parse(rdr["ProductId"].ToString()),
                        Name = rdr["Name"].ToString(),
                        Description = rdr["Description"] == DBNull.Value ? null : rdr["Description"].ToString()
                    };
                }
            }
        }

        public async Task DeleteAsync(ProductOption productOption)
        {
            var conn = _database.GetConnection();
            await conn.Open();

            using (var cmd = new SqlCommand("DELETE FROM productoption WHERE id = @Id", conn.SqlConnection))
            {
                cmd.Parameters.AddWithValue("@Id", productOption.Id);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<IReadOnlyList<ProductOption>> GetAllAsync(string productId)
        {
            var items = new List<ProductOption>();
            var conn = _database.GetConnection();
            await conn.Open();

            var query = "SELECT id FROM productoption";
            if (!string.IsNullOrEmpty(productId))
            {
                query += " WHERE productid = @ProductId";
            }

            using (var cmd = new SqlCommand(query, conn.SqlConnection))
            {
                if (!string.IsNullOrEmpty(productId))
                {
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                }

                using (var rdr = await cmd.ExecuteReaderAsync())
                {
                    while (await rdr.ReadAsync())
                    {
                        var id = Guid.Parse(rdr["id"].ToString());
                        items.Add(await GetByIdAsync(id)); // Async call to fetch full details.
                    }
                }
            }

            return items.AsReadOnly();
        }
    }
}
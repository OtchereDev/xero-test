using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace refactor_this.Models
{
    public class ProductOptionRepository : IProductOptionRepository
    {
        private readonly IDatabase _database;

        public ProductOptionRepository(IDatabase database)
        {
            _database = database;
        }

        public async Task SaveAsync(ProductOption productOption)
        {
            using (var conn = _database.GetConnection())
            {
                const string query =
                    "INSERT INTO productoption (id, productid, name, description) VALUES (@Id, @ProductId, @Name, @Description)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", productOption.Id);
                    cmd.Parameters.AddWithValue("@ProductId", productOption.ProductId);
                    cmd.Parameters.AddWithValue("@Name", productOption.Name);
                    cmd.Parameters.AddWithValue("@Description", productOption.Description ?? (object)DBNull.Value);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateAsync(ProductOption productOption)
        {
            using (var conn = _database.GetConnection())
            {
                const string query = "UPDATE productoption SET name = @Name, description = @Description WHERE id = @Id";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", productOption.Id);
                    cmd.Parameters.AddWithValue("@Name", productOption.Name);
                    cmd.Parameters.AddWithValue("@Description", productOption.Description ?? (object)DBNull.Value);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<ProductOption> GetByIdAsync(Guid id)
        {
            using (var conn = _database.GetConnection())
            {
                const string query = "SELECT * FROM productoption WHERE id = @Id";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    await conn.OpenAsync();
                    using (var rdr = await cmd.ExecuteReaderAsync())
                    {
                        if (!await rdr.ReadAsync())
                            return null;

                        return MapProductOption(rdr);
                    }
                }
            }
        }

        public async Task DeleteAsync(ProductOption productOption)
        {
            using (var conn = _database.GetConnection())
            {
                const string query = "DELETE FROM productoption WHERE id = @Id";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", productOption.Id);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteByProductIdAsync(Guid productId)
        {
            using (var conn = _database.GetConnection())
            {
                const string query = "DELETE FROM productoption WHERE productid = @Id";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", productId);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<IReadOnlyList<ProductOption>> GetAllAsync(string productId)
        {
            var items = new List<ProductOption>();

            using (var conn = _database.GetConnection())
            {
                var query = "SELECT * FROM productoption";
                if (!string.IsNullOrEmpty(productId))
                {
                    query += " WHERE productid = @ProductId";
                }

                using (var cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(productId))
                    {
                        cmd.Parameters.AddWithValue("@ProductId", productId);
                    }

                    await conn.OpenAsync();
                    using (var rdr = await cmd.ExecuteReaderAsync())
                    {
                        while (await rdr.ReadAsync())
                        {
                            items.Add(MapProductOption(rdr));
                        }
                    }
                }
            }

            return items.AsReadOnly();
        }

        private ProductOption MapProductOption(IDataRecord record)
        {
            return new ProductOption
            {
                Id = Guid.Parse(record["id"].ToString()),
                ProductId = Guid.Parse(record["productid"].ToString()),
                Name = record["name"].ToString(),
                Description = record["description"] == DBNull.Value ? null : record["description"].ToString()
            };
        }
    }
}
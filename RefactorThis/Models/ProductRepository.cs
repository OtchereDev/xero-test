using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace refactor_this.Models
{
    public class ProductRepository: IProductRepository
    {
        private readonly IDatabase _database;

        public ProductRepository(IDatabase database)
        {
            _database = database;
        }

        public async Task SaveAsync(Product product)
        {
            using (var conn = _database.GetConnection())
            {
                using (var cmd = conn.SqlConnection.CreateCommand())
                {
                    cmd.CommandText =
                        "INSERT INTO product (id, name, description, price, deliveryprice) VALUES (@Id, @Name, @Description, @Price, @DeliveryPrice)";
                    cmd.Parameters.AddWithValue("@Id", product.Id);
                    cmd.Parameters.AddWithValue("@Name", product.Name);
                    cmd.Parameters.AddWithValue("@Description", (object)product.Description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@DeliveryPrice", product.DeliveryPrice);

                    await conn.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        
        public async Task UpdateAsync(Product product)
        {
            using (var conn = _database.GetConnection())
            {
                using (var cmd = conn.SqlConnection.CreateCommand())
                {
                    cmd.CommandText = "UPDATE product SET name = @Name, description = @Description, price = @Price, deliveryprice = @DeliveryPrice WHERE id = @Id";

                    cmd.Parameters.AddWithValue("@Id", product.Id);
                    cmd.Parameters.AddWithValue("@Name", product.Name);
                    cmd.Parameters.AddWithValue("@Description", (object)product.Description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@DeliveryPrice", product.DeliveryPrice);

                    await conn.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteAsync(Product product)
        {
            var optionsRepo = new ProductOptionRepository(_database);
            await optionsRepo.DeleteByProductIdAsync(product.Id);
            
            using (var conn = _database.GetConnection())
            {
                using (var cmd = conn.SqlConnection.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM product WHERE id = @Id";
                    cmd.Parameters.AddWithValue("@Id", product.Id);

                    await conn.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            using (var conn = _database.GetConnection())
            {
                using (var cmd = conn.SqlConnection.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM product WHERE id = @Id";
                    cmd.Parameters.AddWithValue("@Id", id);

                    await conn.Open();
                    using (var rdr = await cmd.ExecuteReaderAsync())
                    {
                        if (!await rdr.ReadAsync())
                            return null;

                        return MapProduct(rdr);
                    }
                }
            }
        }

        public async Task<IReadOnlyList<Product>> GetAllAsync(string name)
        {
            var products = new List<Product>();
            using (var conn = _database.GetConnection())
            {
                using (var cmd = conn.SqlConnection.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM product";
                    if (!string.IsNullOrEmpty(name))
                    {
                        cmd.CommandText += " WHERE LOWER(name) LIKE @Name";
                        cmd.Parameters.AddWithValue("@Name", $"%{name.ToLower()}%");
                    }

                    await conn.Open();
                    using (var rdr = await cmd.ExecuteReaderAsync())
                    {
                        while (await rdr.ReadAsync())
                        {
                            products.Add(MapProduct(rdr));
                        }
                    }
                }
            }

            return products.AsReadOnly();
        }

        private Product MapProduct(IDataRecord record)
        {
            return new Product
            {
                Id = Guid.Parse(record["Id"].ToString()),
                Name = record["Name"].ToString(),
                Description = record["Description"] == DBNull.Value ? null : record["Description"].ToString(),
                Price = decimal.Parse(record["Price"].ToString()),
                DeliveryPrice = decimal.Parse(record["DeliveryPrice"].ToString())
            };
        }
    }
}
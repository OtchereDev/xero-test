using System;
using System.Data.SqlClient;

namespace refactor_this.Models
{
    public class ProductOptionRepository
    {
        public void Save(ProductOption productOption)
        {
            var conn = Helpers.NewConnection();
            var cmd = productOption.IsNew ?
                new SqlCommand($"insert into productoption (id, productid, name, description) values ('{productOption.Id}', '{productOption.ProductId}', '{productOption.Name}', '{productOption.Description}')", conn) :
                new SqlCommand($"update productoption set name = '{productOption.Name}', description = '{productOption.Description}' where id = '{productOption.Id}'", conn);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
        
        public ProductOption GetById(Guid id)
        {
            var conn = Helpers.NewConnection();
            var cmd = new SqlCommand($"select * from productoption where id = '{id}'", conn);
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
            var conn = Helpers.NewConnection();
            var cmd = new SqlCommand($"delete from productoption where id = '{productOption.Id}'", conn);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
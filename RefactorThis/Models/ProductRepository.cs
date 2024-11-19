using System;
using System.Data.SqlClient;
using refactor_this.Services;

namespace refactor_this.Models
{
    public class ProductRepository
    {
        public void Save(Product product)
        {
            var conn = Helpers.NewConnection();
            var cmd = product.IsNew ? 
                new SqlCommand($"insert into product (id, name, description, price, deliveryprice) values ('{product.Id}', '{product.Name}', '{product.Description}', {product.Price}, {product.DeliveryPrice})", conn) : 
                new SqlCommand($"update product set name = '{product.Name}', description = '{product.Description}', price = {product.Price}, deliveryprice = {product.DeliveryPrice} where id = '{product.Id}'", conn);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(Product product)
        {
            var optionsRepo = new ProductOptionRepository();
            foreach (var option in new ProductOptionsServices(optionsRepo).GetProductOptions(product.Id.ToString()))
                optionsRepo.Delete(option);

            var conn = Helpers.NewConnection();
            conn.Open();
            var cmd = new SqlCommand($"delete from product where id = '{product.Id}'", conn);
            cmd.ExecuteNonQuery();
        }

        public Product GetById(Guid id)
        {
            var conn = Helpers.NewConnection();
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
    }
}
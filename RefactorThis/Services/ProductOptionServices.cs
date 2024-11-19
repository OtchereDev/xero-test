using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using refactor_this.Models;

namespace refactor_this.Services
{
    public class ProductOptionsServices
    {
        public List<ProductOption> Items { get; private set; }

        public ProductOptionsServices()
        {
            LoadProductOptions(null);
        }

        public ProductOptionsServices(Guid productId)
        {
            LoadProductOptions($"where productid = '{productId}'");
        }

        private void LoadProductOptions(string where)
        {
            Items = new List<ProductOption>();
            var conn = Helpers.NewConnection();
            var cmd = new SqlCommand($"select id from productoption {where}", conn);
            conn.Open();

            var repo = new ProductOptionRepository();
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var id = Guid.Parse(rdr["id"].ToString());
                Items.Add(repo.GetById(id));
            }
        }
    }
}
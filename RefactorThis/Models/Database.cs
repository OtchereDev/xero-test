using System.Data.SqlClient;
using System.Web;

namespace refactor_this.Models
{
    public class Database: IDatabase
    {
        private const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={DataDirectory}\Database.mdf;Integrated Security=True";

        public IDbConnectionWrapper GetConnection()
        {
            var connstr = ConnectionString.Replace("{DataDirectory}", HttpContext.Current.Server.MapPath("~/App_Data"));
            return new SqlConnectionWrapper(connstr);
        }
    }
}
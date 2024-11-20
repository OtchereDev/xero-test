using System.Data.SqlClient;

namespace refactor_this.Models
{
    public interface IDatabase
    {
        SqlConnection GetConnection();
    }
}
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace refactor_this.Models
{
    public interface IDbConnectionWrapper
    {
        IDbCommand CreateCommand();
        Task Open();
        SqlConnection SqlConnection { get; }
        
        ConnectionState State { get; }
    }
}
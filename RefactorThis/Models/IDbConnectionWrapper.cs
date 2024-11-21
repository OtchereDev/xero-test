using System.Data;
using System.Data.SqlClient;

namespace refactor_this.Models
{
    public interface IDbConnectionWrapper
    {
        IDbCommand CreateCommand();
        void Open();
        SqlConnection SqlConnection { get; }
    }
}
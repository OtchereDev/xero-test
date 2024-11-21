using System.Data;
using System.Data.SqlClient;

namespace refactor_this.Models
{
    public class SqlConnectionWrapper : IDbConnectionWrapper
    {
        private readonly SqlConnection _connection;

        public SqlConnectionWrapper(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public IDbCommand CreateCommand()
        {
            return _connection.CreateCommand();
        }

        public void Open()
        {
            _connection.Open();
        }

        public SqlConnection SqlConnection => _connection;

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
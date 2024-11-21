using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace refactor_this.Models
{
    public class SqlConnectionWrapper : IDbConnectionWrapper
    {
        private readonly SqlConnection _connection;

        public SqlConnectionWrapper(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));

            _connection = new SqlConnection(connectionString);
        }

        public IDbCommand CreateCommand()
        {
            if (_connection.State != ConnectionState.Open)
                throw new InvalidOperationException("Cannot create a command when the connection is not open.");

            return _connection.CreateCommand();
        }

        public async Task Open()
        {
            try
            {
                if (_connection.State != ConnectionState.Open)
                {
                    await _connection.OpenAsync();
                }

            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Failed to open the connection",e);
            }
        }

        public SqlConnection SqlConnection => _connection;

        public ConnectionState State => _connection.State;

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
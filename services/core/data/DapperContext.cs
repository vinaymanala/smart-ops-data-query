using Microsoft.Data.SqlClient;
using System.Data;

namespace core.data;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
public class SqlDbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqlDbConnectionFactory(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {            
            throw new InvalidOperationException("Connection string not found.");
        }
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}
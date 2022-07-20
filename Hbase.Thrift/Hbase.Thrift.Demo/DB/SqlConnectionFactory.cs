using Hbase.Thrift.Demo.Settings;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Hbase.Thrift.Demo.DB
{
    public class SqlConnectionFactory : ISqlConnectionFactory, IDisposable
    {
        private readonly DbOptions _dbOptions;
        private IDbConnection _connection;

        public SqlConnectionFactory(DbOptions dbOptions)
        {
            _dbOptions = dbOptions;
        }
        public IDbConnection GetOpenConnection()
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                _connection = new SqlConnection(_dbOptions.conn);
                _connection.Open();
            }
            return _connection;
        }
        public void Dispose()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Dispose();
            }
        }
    }
}

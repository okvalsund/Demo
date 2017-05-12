using Demo.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using System.Reflection;
using Dapper;
using System.Linq;

namespace Demo.Repository
{
    public class AsyncDapperRepository<T> : IAsyncDataRepository<T> where T : class, IEntityBase, new()
    {
        public AsyncDapperRepository(IConnectionString connectionString)
        {
            this._connectionString = connectionString;
        }

        private readonly IConnectionString _connectionString;

        public async Task<bool> DeleteAsync(T item)
        {
            bool deleteOk = true;
            using (var connection = new SqlConnection(_connectionString.Path))
            {
                SqlTransaction transaction = null;
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    deleteOk = await connection.DeleteAsync<T>(item, transaction);
                    transaction.Commit();
                }
                catch
                {
                    transaction?.Rollback();
                    deleteOk = false;
                }
                finally
                {
                    connection?.Close();
                }
            }
            return deleteOk;
        }

        public async Task<bool> DeleteAsync(IEnumerable<T> list)
        {
            bool deleteOk;
            using (var connection = new SqlConnection(_connectionString.Path))
            {
                SqlTransaction transaction = null;
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    deleteOk = await connection.DeleteAsync<IEnumerable<T>>(list, transaction);
                    transaction.Commit();
                }
                catch
                {
                    transaction?.Rollback();
                    deleteOk = false;
                }
                finally
                {
                    connection?.Close();
                }
            }
            return deleteOk;
        }

        public async Task<bool> DeleteAllAsync()
        {
            bool deleteOk;
            using (var connection = new SqlConnection(_connectionString.Path))
            {
                SqlTransaction transaction = null;
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    deleteOk = await connection.DeleteAllAsync<T>(transaction);
                    transaction.Commit();
                }
                catch
                {
                    transaction?.Rollback();
                    deleteOk = false;
                }
                finally
                {
                    connection?.Close();
                }
            }
            return deleteOk;
        }

        public async Task<T> GetAsync(int id)
        {
            T obj = null;
            using (var connection = new SqlConnection(_connectionString.Path))
            {
                try
                {
                    connection.Open();
                    obj = await connection.GetAsync<T>(id);
                }
                catch
                {

                }
                finally
                {
                    connection?.Close();
                }
            }
            return obj;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            IEnumerable<T> list = null;
            using (var connection = new SqlConnection(_connectionString.Path))
            {
                try
                {
                    connection.Open();
                    list = await connection.GetAllAsync<T>();
                }
                catch { }
                finally
                {
                    connection?.Close();
                }
            }
            return list;
        }

        public async Task<int> InsertAsync(T item)
        {
            int newId;
            using (var connection = new SqlConnection(_connectionString.Path))
            {
                SqlTransaction transaction = null;
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    newId = await connection.InsertAsync<T>(item, transaction);
                    transaction.Commit();
                }   
                catch
                {
                    newId = -1;
                    transaction?.Rollback();
                }
                finally
                {
                    connection?.Close();
                }
            }
            return newId;
        }

        public async Task<int> InsertAsync(IEnumerable<T> list)
        {
            int committedCount = 0;
            using (var connection = new SqlConnection(_connectionString.Path))
            {
                SqlTransaction transaction = null;
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    committedCount = await connection.InsertAsync<IEnumerable<T>>(list, transaction);
                    transaction.Commit();
                }
                catch
                {
                    transaction?.Rollback();
                }
                finally
                {
                    connection?.Close();
                }
            }
            return committedCount;
        }

        public async Task<bool> UpdateAsync(T item)
        {
            bool updateOk;
            using (var connection = new SqlConnection(_connectionString.Path))
            {
                SqlTransaction transaction = null;
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    updateOk = await connection.UpdateAsync<T>(item, transaction);
                    transaction.Commit();
                }
                catch
                {
                    updateOk = false;
                    transaction?.Rollback();
                }
                finally
                {
                    connection?.Close();
                }
            }
            return updateOk;
        }

        public async Task<bool> UpdateAsync(IEnumerable<T> list)  
        {
            bool updateOk;
            using (var connection = new SqlConnection(_connectionString.Path))
            {
                SqlTransaction transaction = null;
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    updateOk = await connection.UpdateAsync<IEnumerable<T>>(list, transaction);
                    transaction.Commit();
                }
                catch
                {
                    updateOk = false;
                    transaction?.Rollback();
                }
                finally
                {
                    connection?.Close();
                }
            }
            return updateOk;
        }

        public async Task<T> GetByForeignKeyAsync(int foreignKeyValue, string foreignKeyColumn, int id)
        {
            T obj = null;
            //To check if column is present and prevent any kind of SQL injection
            if (typeof(T).GetProperty(foreignKeyColumn) != null)
            {
                using (var connection = new SqlConnection(_connectionString.Path))
                {
                    try
                    {
                        connection.Open();
                        string sql = $"select * from {TableName()} where {nameof(IEntityBase.Id)} = @Id and {foreignKeyColumn} = @ForeignKeyValue";
                        obj = await connection.QueryFirstAsync<T>(sql, new { Id = id, ForeignKeyValue = foreignKeyValue });
                    }
                    catch
                    {

                    }
                    finally
                    {
                        connection?.Close();
                    }
                }
            }

            return obj;
        }

        public async Task<T> GetFilterByInheritedAsync(Type type, int id)
        {
            T obj = null;
            using (var connection = new SqlConnection(_connectionString.Path))
            {
                try
                {
                    
                    connection.Open();
                    var list = await connection.QueryAsync<T>($"select t.* from {TableName()} t inner join {TableName(type)} i on t.id = i.id and t.id = {id}");
                    obj = list.FirstOrDefault();
                }
                catch { }
                finally
                {
                    connection?.Close();
                }
            }
            return obj;
        }

        public async Task<IEnumerable<T>> GetFilterByInheritedAsync(Type type)
        {
            IEnumerable<T> list = null;
            using (var connection = new SqlConnection(_connectionString.Path))
            {
                try
                {

                    connection.Open();
                    list = await connection.QueryAsync<T>($"select t.* from {TableName()} t inner join {TableName(type)} i on t.id = i.id");
                }
                catch { }
                finally
                {
                    connection?.Close();
                }
            }
            return list;
        }

        public async Task<IEnumerable<T>> GetFilterByForeignKey(int foreignKeyValue, string foreignKeyColumn)
        {
            IEnumerable<T> list = null;
            if (typeof(T).GetProperty(foreignKeyColumn) != null)
            {
                using (var connection = new SqlConnection(_connectionString.Path))
                {
                    try
                    {
                        connection.Open();
                        string sql = $"select * from {TableName()} where {foreignKeyColumn} = @ForeignKeyValue";
                        list = await connection.QueryAsync<T>(sql, new { ForeignKeyValue = foreignKeyValue });
                    }
                    catch { }
                    finally
                    {
                        connection?.Close();
                    }
                }
            }
            return list;
        }
        
        public static string TableName()
        {
            return TableName(typeof(T));
        }

        private static string TableName(Type type)
        {
            var tableAttribute = type.GetTypeInfo()
                .GetCustomAttributes(false)
                .SingleOrDefault(attr => attr.GetType().FullName == typeof(TableAttribute).FullName) as dynamic;

            //postfix with "s" is dapper.contrib standard
            //https://github.com/StackExchange/Dapper/blob/master/Dapper.Contrib/SqlMapperExtensions.cs#L273
            return tableAttribute != null ? tableAttribute.Name : typeof(T).Name + "s";
        }
    }
}

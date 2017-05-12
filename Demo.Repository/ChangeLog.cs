using Demo.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using KellermanSoftware.CompareNetObjects;
using Demo.Repository.Entities;
using System.Data.SqlClient;
using Dapper.Contrib.Extensions;
using System.Linq;

namespace Demo.Repository
{
    public class ChangeLog : IChangeLog
    {
        private readonly IConnectionString _connectionString;

        public ChangeLog(IConnectionString connectionString)
        {
            this._connectionString = connectionString;
        }

        public async Task<int> LogCreated(string verb, string uri, string obj)
        {
            return await InsertAsync(new ChangeLogItem(verb, uri, obj));
        }
        public async Task<int> LogUpated(string verb, string uri, object oldObject, object newObject)
        {
            return await InsertAsync(new ChangeLogItem(verb, uri, await Changes(oldObject, newObject)));
        }

        public async Task<string> Changes(object oldObject, object newObject)
        {
            var formattedChanges = new CompareLogic().Compare(oldObject, newObject).Differences.Select(c => $"{c.PropertyName.Replace(".", "")}: '{c.Object1Value}' -> '{c.Object2Value}'");
            return formattedChanges.Count() == 0 ? "No changes" : string.Join(Environment.NewLine, formattedChanges);

        }

        public async Task<int> LogDeleted(string verb, string uri, string obj)
        {
            return await InsertAsync(new ChangeLogItem(verb, uri, obj));
        }

        public async Task<int> InsertAsync(ChangeLogItem item)
        {
            int newId;
            using (var connection = new SqlConnection(_connectionString.Path))
            {
                SqlTransaction transaction = null;
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    newId = await connection.InsertAsync<ChangeLogItem>(item, transaction);
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
    }
}

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.Common;

namespace GST.Library.StoredProcedureHelper
{
    // TODO Surveiller https://github.com/aspnet/EntityFramework/issues/1862 et faire en sorte qu'une requête sur une procédure stocké puisse être utilisé comme une Iqueriable
    /// <summary>
    /// Service to manage stored procedures
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class StoredProcedureService<TContext> : IStoredProcedureService where TContext : DbContext
    {
        private readonly TContext context;
        /// <summary>
        /// StoredProcedureService
        /// </summary>
        /// <param name="_context"></param>
        public StoredProcedureService(TContext _context)
        {
            context = _context;
        }

        /// <summary>
        /// Allow to create a stored procedure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public StoredProcedure<T> CreateStoredProcedure<T>(string storedProcedureName, IDictionary<string, string> parameters) where T : new()
        {
            StoredProcedure<T> storedProcedure = new StoredProcedure<T>(GetDbCommand(), storedProcedureName);
            storedProcedure.parameters = parameters;
            return storedProcedure;
        }

        /// <summary>
        /// Get the Command Db
        /// </summary>
        /// <returns></returns>
        public DbCommand GetDbCommand()
        {
            return context.Database.GetDbConnection().CreateCommand();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.Common;

namespace GST.Library.StoredProcedureHelper
{
    // TODO Surveiller https://github.com/aspnet/EntityFramework/issues/1862 et faire en sorte qu'une requête sur une procédure stocké puisse être utilisé comme une Iqueriable
    public class StoredProcedureService<TContext> : IStoredProcedureService where TContext : DbContext
    {
        private readonly TContext context;

        public StoredProcedureService(TContext _context)
        {
            context = _context;
        }

        public StoredProcedure<T> CreateStoredProcedure<T>(string storedProcedureName, IDictionary<string, string> parameters) where T : new()
        {
            StoredProcedure<T> storedProcedure = new StoredProcedure<T>(GetDbCommand(), storedProcedureName);
            storedProcedure.parameters = parameters;
            return storedProcedure;
        }

        public DbCommand GetDbCommand()
        {
            return context.Database.GetDbConnection().CreateCommand();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.Common;

namespace GST.Library.StoredProcedureHelper
{
    // TODO Surveiller https://github.com/aspnet/EntityFramework/issues/1862 et faire en sorte qu'une requête sur une procédure stocké puisse être utilisé comme une Iqueriable
    public class StoredProcedureService : IStoredProcedureService
    {
        private readonly DbContext context;

        public StoredProcedureService(DbContext _context)
        {
            context = _context;
        }

        public StoredProcedure<T> CreateStoreProcedure<T>(string storedProcedureName, IDictionary<string, string> parameters) where T : new()
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

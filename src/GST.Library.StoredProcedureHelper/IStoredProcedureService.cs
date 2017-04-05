using System.Collections.Generic;
using System.Data.Common;


namespace GST.Library.StoredProcedureHelper
{
    /// <summary>
    /// Service to provide Stored Procedure
    /// </summary>
    public interface IStoredProcedureService
    {
        /// <summary>
        /// Return a DbCommand who can be used to build a stored procedure
        /// </summary>
        /// <returns></returns>
        DbCommand GetDbCommand();

        /// <summary>
        /// Create a stored procedure 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <example>
        /// IDictionary<string, string> parameters = new Dictionary<string, string>()
        ///  {
        ///     { "name", "andrea" },
        ///     { "age", 24 },
        ///   };
        /// storedProcedureService.CreateStoredProcedure<EvenementViewModel>("StoredProcedureNameInTheDatabase", parameters)
        /// </example>
        StoredProcedure<T> CreateStoredProcedure<T>(string storedProcedureName, IDictionary<string, string> parameters) where T : new();
    }
}

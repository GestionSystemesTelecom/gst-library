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
        /// IDictionary&lt;string, string&gt; parameters = new Dictionary&lt;string, string&gt;()
        ///  {
        ///     { "name", "andrea" },
        ///     { "age", 24 },
        ///   };
        /// storedProcedureService.CreateStoredProcedure&lt;EvenementViewModel&gt;("StoredProcedureNameInTheDatabase", parameters)
        /// </example>
        /// 
        StoredProcedure<T> CreateStoredProcedure<T>(string storedProcedureName, IDictionary<string, string> parameters) where T : new();
    }
}

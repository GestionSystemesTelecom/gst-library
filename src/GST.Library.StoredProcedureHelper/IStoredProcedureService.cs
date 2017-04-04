using System.Collections.Generic;
using System.Data.Common;


namespace GST.Library.StoredProcedureHelper
{
    /// <summary>
    /// Optimise l'utilisation des procédures stockées
    /// </summary>
    public interface IStoredProcedureService
    {
        /// <summary>
        /// Retourne un DbCommand qui permet de contruire une requête vers une procédure stockée
        /// </summary>
        /// <returns></returns>
        DbCommand GetDbCommand();

        StoredProcedure<T> CreateStoreProcedure<T>(string storedProcedureName, IDictionary<string, string> parameters) where T : new();
    }
}

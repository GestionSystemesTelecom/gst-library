using GST.Library.Helper.Type;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace GST.Library.StoredProcedureHelper
{
    /// <summary>
    /// StoredProcedure
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StoredProcedure<T> where T : new()
    {
        IDbCommand command;
        string storedFunctioname;
        /// <summary>
        /// Parameters List
        /// </summary>
        public IDictionary<string, string> parameters;

        /// <summary>
        /// Number of element that a query can return
        /// </summary>
        public int? limit { get; set; }
        /// <summary>
        /// Number of element that must be ignored
        /// </summary>
        public int? offset { get; set; }
        /// <summary>
        /// List of property name to sort the result of the request
        /// </summary>
        public string[] order;
        /// <summary>
        /// Defined if the result is ascending or descending 
        /// </summary>
        public string orderDir;
        /// <summary>
        /// Make a filter on all the string property
        /// </summary>
        public string search;

        /// <summary>
        /// StoredProcedure
        /// </summary>
        /// <param name="command"></param>
        /// <param name="storedFunctioname"></param>
        public StoredProcedure(IDbCommand command, string storedFunctioname)
        {
            this.command = command;
            this.storedFunctioname = storedFunctioname;
        }

        /// <summary>
        /// Count the number of result
        /// </summary>
        /// <returns></returns>
        public long Count()
        {
            command.CommandText = $"SELECT COUNT(*) FROM {storedFunctioname}({ParameterStringify()})";

            init();

            return (long)command.ExecuteScalar();
        }

        /// <summary>
        /// List all the result of the query
        /// </summary>
        /// <returns></returns>
        public List<T> ToList()
        {
            List<T> retObject = new List<T>();
            Type type = typeof(T);
            PropertyInfo[] genericClassProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            string commandText = $"SELECT * FROM {storedFunctioname}({ParameterStringify()})";

            init();

            if (search != null && search.Length > 0)
            {
                List<string> searchQuery = new List<string>();
                foreach (PropertyInfo prop in genericClassProperties)
                {
                    if (search.IsNumeric() && prop.IsNumeric())
                    {
                        try
                        {
                            searchQuery.Add($" {prop.Name} = @propSearch{prop.Name.ToLower()}");
                            command.Parameters.Add(new NpgsqlParameter($"propSearch{prop.Name.ToLower()}", Convert.ChangeType(search, prop.PropertyType)));
                        }
                        catch (System.OverflowException)
                        {
                            // TODO Trouver le bon algo pour éviter d'essayer d'insérer un numéro de tel dans du Int16 (non ca passe pas...)
                        }
                    }
                    else if (prop.IsString() && search.IsString())
                    {
                        searchQuery.Add($" lower({prop.Name}) LIKE @propSearch{prop.Name.ToLower()} ");
                        command.Parameters.Add(new NpgsqlParameter($"propSearch{prop.Name.ToLower()}", $"%{search.ToString().ToLower()}%"));
                    }
                    else if (prop.IsDate() && search.IsDate())
                    {
                        searchQuery.Add($" {prop.Name}::date = @propSearch{prop.Name.ToLower()} ");
                        command.Parameters.Add(new NpgsqlParameter($"propSearch{prop.Name.ToLower()}", search.ToDateTime()));
                    }
                }

                commandText += $" WHERE {string.Join(" OR ", searchQuery)}";
            }

            // Construction des filtres sur la procédure stockée
            if (order != null)
            {
                commandText += $" ORDER BY {orderDir ?? orderDir: 'ASC'}";
            }

            if (limit != null)
            {
                commandText += $" LIMIT {limit}";
            }

            if (offset != null)
            {
                commandText += $" OFFSET {offset}";
            }

            command.CommandText = commandText;

            // Exécution de la requête et traitement du résultat afin de retourner un liste d'objet T
            using (var dataReader = command.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    T dataRow = new T();

                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                    {
                        string fieldName = dataReader.GetName(iFiled);
                        PropertyInfo prop = genericClassProperties.FirstOrDefault(x => x.Name.ToLower() == fieldName.ToLower());
                        if (prop != null)
                        {
                            if (dataReader.GetValue(iFiled) != DBNull.Value)
                                prop.SetValue(dataRow, dataReader.GetValue(iFiled), null);
                        }
                    }

                    retObject.Add(dataRow);
                }
            }

            return retObject;
        }

        /// <summary>
        /// Returns the string that represents the parameters of the stored procedure
        /// </summary>
        /// <returns></returns>
        private string ParameterStringify()
        {
            return string.Join(",", parameters.Select(d => $"@{d.Key}").ToArray<string>());
        }

        /// <summary>
        /// Init the Stored Procedure system
        /// </summary>
        private void init()
        {
            command.CommandType = System.Data.CommandType.Text;

            command.Parameters.Clear();

            // Ajout de la valeurs des paramètres
            foreach (KeyValuePair<string, string> param in parameters)
            {
                command.Parameters.Add(new NpgsqlParameter($"@{param.Key}", param.Value));
            }

            OpenConnection();
        }

        /// <summary>
        /// Open a connection to the database
        /// </summary>
        private void OpenConnection()
        {
            if (command.Connection.State != ConnectionState.Open)
            {
                command.Connection.Open();
            }
        }
    }
}

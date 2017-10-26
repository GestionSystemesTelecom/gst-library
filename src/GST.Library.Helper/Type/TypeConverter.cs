using System;
using System.Globalization;

namespace GST.Library.Helper.Type
{
    /// <summary>
    /// Convert value to another
    /// </summary>
    public static class TypeConverter
    {
        private static string[] dateFormats = {
                   "yyyy-MM-dd",
                   "yyyy-MM",
                   "dd/MM/yyyy"
                    };

        /// <summary>
        /// String to DateTime
        /// </summary>
        /// <remarks>
        /// If the conversion is not possible, the method returns null
        /// </remarks>
        /// <param name="rawDate"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this System.String rawDate)
        {
            DateTime dateValue;

            if (DateTime.TryParseExact(rawDate, dateFormats,
                            new CultureInfo("fr-FR"),
                            DateTimeStyles.None,
                            out dateValue))
            {
                return dateValue;
            }

            return null;
        }
    }
}

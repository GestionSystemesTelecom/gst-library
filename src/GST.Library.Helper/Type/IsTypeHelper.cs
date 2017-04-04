using System;
using System.Globalization;
using System.Reflection;

namespace GST.Library.Helper.Type
{
    public static class IsTypeHelper
    {
        private static string[] dateFormats = {
                   "yyyy-MM",
                   "yyyy-MM-dd",
                   "dd/MM/yyyy"
                    };

        /// <summary>
        /// Check if a value is a numeric one
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNumeric(this System.Object obj)
        {
            if (obj is PropertyInfo)
            {
                return IsNumericType(((PropertyInfo)obj).PropertyType);
            }

            if (IsNumericType(obj.GetType()))
            {
                return true;
            }

            try
            {
                Convert.ToInt64(obj);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Determines if a type is numeric. Nullable numeric types are considered numeric.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNumericType(this System.Type type)
        {
            if (type == null)
            {
                return false;
            }

            switch (System.Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                case TypeCode.Object:
                    return false;
            }
            return false;
        }

        public static bool IsString(this System.Object obj)
        {
            if (obj is PropertyInfo)
            {
                return IsStringType(((PropertyInfo)obj).PropertyType);
            }

            return IsStringType(obj.GetType());
        }

        /// <summary>
        /// Determines if a type is string.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsStringType(this System.Type type)
        {
            if (type == null)
            {
                return false;
            }

            switch (System.Type.GetTypeCode(type))
            {
                case TypeCode.Char:
                case TypeCode.String:
                    return true;
                case TypeCode.Object:
                    return false;
            }
            return false;
        }


        public static bool IsDate(this System.Object obj)
        {
            if (obj is PropertyInfo)
            {
                return IsDateType(((PropertyInfo)obj).PropertyType);
            }

            if (IsDateType(obj.GetType()))
            {
                return true;
            }

            DateTime dateValue;

            if (DateTime.TryParseExact(obj.ToString(), dateFormats,
                            new CultureInfo("fr-FR"),
                            DateTimeStyles.None,
                            out dateValue))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines if a type is date.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsDateType(this System.Type type)
        {
            if (type == null)
            {
                return false;
            }

            switch (System.Type.GetTypeCode(type))
            {
                case TypeCode.DateTime:
                    return true;
                case TypeCode.Object:
                    return false;
            }
            return false;
        }
    }
}


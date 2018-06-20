using GST.Library.TimeZone.Core;
using GST.Library.TimeZone.Services.Abstract;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Linq;
using System.Reflection;

namespace GST.Library.TimeZone.ModelBinder
{
    /// <summary>
    /// 
    /// </summary>
    public class DateTimeConverter : DateTimeConverterBase
    {
        /// <summary>
        /// The user culture
        /// </summary>
        protected readonly Func<ITimeZoneHelper> TimeZoneHelper;

        /// <summary>
        /// Store the previous class type
        /// </summary>
        protected Type PreviousClass;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeConverter"/> class.
        /// </summary>
        /// <param name="_TimeZoneHelper">The user culture.</param>
        public DateTimeConverter(Func<ITimeZoneHelper> _TimeZoneHelper)
        {
            TimeZoneHelper = _TimeZoneHelper;
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            if(!(objectType == typeof(DateTime) || objectType == typeof(DateTime?)))
            {
                PreviousClass = objectType;
                return false;
            }

            return true;
            //|| objectType == typeof(DateTimeZone);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (HasJsonIgnoreTimeZoneAttribute(reader.Path))
            {
                return reader.Value;
            }
            return TimeZoneHelper().GetUtcTime(DateTime.Parse(reader.Value.ToString()));
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (HasJsonIgnoreTimeZoneAttribute(writer.Path))
            {
                writer.WriteValue(Convert.ToDateTime(value));
            }
            else
            {
                writer.WriteValue(TimeZoneInfo.ConvertTime(Convert.ToDateTime(value), TimeZoneHelper().TimeZone).ToString());
            }

            writer.Flush();
        }

        private bool HasJsonIgnoreTimeZoneAttribute(string Path)
        {
            string propertyName = Path.Split('.').Last();
            var a = PreviousClass.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            return a.GetCustomAttribute<JsonIgnoreTimeZoneAttribute>() != null;
        }
    }
}

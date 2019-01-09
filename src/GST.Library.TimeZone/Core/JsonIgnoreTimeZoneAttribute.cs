#if NETCOREAPP
using System;

namespace GST.Library.TimeZone.Core
{
    /// <summary>
    /// This Json Attribute must be used to ignore the Datetime when the data must be unchange
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class JsonIgnoreTimeZoneAttribute : Attribute
    {
    }
}
#endif
using GST.Library.TimeZone.Core;
using System;

namespace GST.Library.TimeZone.Tests.Misc
{
    public class DateTimeModelIO
    {
        public DateTime DateWithTimeZoneAdaptation { get; set; }
        [JsonIgnoreTimeZone]
        public DateTime DateWithoutTimeZoneAdaptation { get; set; }

        public SubDateTimeModelIO Sub { get; set; }
    }

    public class SubDateTimeModelIO
    {
        public DateTime SubDateWithTimeZoneAdaptation { get; set; }
        [JsonIgnoreTimeZone]
        public DateTime SubDateWithoutTimeZoneAdaptation { get; set; }
    }
}

using GST.Library.TimeZone.Services.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using TimeZoneConverter;

namespace GST.Library.TimeZone.Services
{
    /// <summary>
    /// User's culture information.
    /// </summary>
    public class TimeZoneHelper : ITimeZoneHelper
    {
        /// <summary>
        /// Gets or sets the time zone.
        /// </summary>
        /// <value>
        /// The time zone.
        /// </value>
        public TimeZoneInfo TimeZone { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeZoneHelper"/> class.
        /// </summary>
        public TimeZoneHelper(IHttpContextAccessor context)
        {
            TimeZone = TZConvert.GetTimeZoneInfo(context.HttpContext?.User.FindFirst("zoneinfo").Value);
            //TimeZone =  TimeZoneInfo.FindSystemTimeZoneById(context.HttpContext?.User.FindFirst("zoneinfo").Value);
        }

        /// <summary>
        /// Gets the user local time.
        /// </summary>
        /// <returns></returns>
        public DateTime GetUserLocalTime()
        {
            return TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZone);
        }

        /// <summary>
        /// Gets the UTC time.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <returns>Get universal date time based on User's Timezone</returns>
        public DateTime GetUtcTime(DateTime datetime)
        {
            return TimeZoneInfo.ConvertTime(datetime, TimeZone, TimeZoneInfo.Utc);
        }

        /// <summary>
        /// Gets the User time.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <returns>Get universal date time based on User's Timezone</returns>
        public DateTime GetUserLocalTime(DateTime datetime)
        {
            return TimeZoneInfo.ConvertTime(datetime, TimeZone);
        }
    }
}

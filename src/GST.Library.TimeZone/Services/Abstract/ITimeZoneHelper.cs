using System;

namespace GST.Library.TimeZone.Services.Abstract
{
    /// <summary>
    /// User's culture information.
    /// </summary>
    public interface ITimeZoneHelper
    {
        /// <summary>
        /// Gets or sets the time zone.
        /// </summary>
        /// <value>
        /// The time zone.
        /// </value>
        TimeZoneInfo TimeZone { get; set; }

        /// <summary>
        /// Gets the user local time.
        /// </summary>
        /// <returns></returns>
        DateTime GetUserLocalTime();

        /// <summary>
        /// Gets the User time.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <returns>Get universal date time based on User's Timezone</returns>
        DateTime GetUserLocalTime(DateTime datetime);

        /// <summary>
        /// Gets the UTC time.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <returns>Get universal date time based on User's Timezone</returns>
        DateTime GetUtcTime(DateTime datetime);
    }
}
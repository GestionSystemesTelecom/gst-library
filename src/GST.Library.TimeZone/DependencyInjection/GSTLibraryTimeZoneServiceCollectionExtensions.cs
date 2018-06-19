using GST.Library.TimeZone.Services;
using GST.Library.TimeZone.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace GST.Library.TimeZone.DependencyInjection
{
    /// <summary>
    /// DI extension methods for adding TimeZone service
    /// </summary>
    public static class GSTLibraryTimeZoneServiceCollectionExtensions
    {

        /// <summary>
        /// Add TimeZone service
        /// </summary>
        /// <param name="services"></param>
        public static void AddTimeZoneService(this IServiceCollection services)
        {
            services.AddScoped<ITimeZoneHelper, TimeZoneHelper>();
        }       
    }
}

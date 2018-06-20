using GST.Library.TimeZone.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace GST.Library.TimeZone.ModelBinder
{
    /// <summary>
    /// <see cref="DateTimeConverter"/> initializer.
    /// </summary>
    public static class DateTimeConverterExtension
    {
        /// <summary>
        /// Registers the date time converter.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="serviceCollection">The service collection.</param>
        /// <returns></returns>
        public static MvcJsonOptions UseDateTimeConverter(this MvcJsonOptions option, IServiceCollection serviceCollection)
        {
            // TODO: BuildServiceProvider could be optimized
            option.SerializerSettings.Converters.Add(new DateTimeConverter(() => serviceCollection.BuildServiceProvider().GetService<ITimeZoneHelper>()));
            return option;
        }
    }
}

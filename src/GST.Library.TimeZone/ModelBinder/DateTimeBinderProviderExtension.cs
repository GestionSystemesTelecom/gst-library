using GST.Library.TimeZone.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GST.Library.TimeZone.ModelBinder
{
    /// <summary>
    /// 
    /// </summary>
    public static class DateTimeBinderProviderExtension
    {
        /// <summary>
        /// Registers the date time provider.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="serviceCollection">The service collection.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">option</exception>
        public static MvcOptions RegisterDateTimeProvider(this MvcOptions option, IServiceCollection serviceCollection)
        {
            if (option == null)
            {
                throw new ArgumentNullException(nameof(option));
            }

            // TODO: BuildServiceProvider could be optimized
            option.ModelBinderProviders.Insert(0, new DateTimeBinderProvider(() => serviceCollection.BuildServiceProvider().GetService<ITimeZoneHelper>()));

            return option;
        }
    }
}

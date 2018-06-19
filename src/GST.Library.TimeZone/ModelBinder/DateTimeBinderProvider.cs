using GST.Library.TimeZone.Services.Abstract;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace GST.Library.TimeZone.ModelBinder
{
    /// <summary>
    /// 
    /// </summary>
    public class DateTimeBinderProvider : IModelBinderProvider
    {
        private readonly Func<ITimeZoneHelper> TimeZoneHelper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_TimeZoneHelper"></param>
        public DateTimeBinderProvider(Func<ITimeZoneHelper> _TimeZoneHelper)
        {
            TimeZoneHelper = _TimeZoneHelper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.UnderlyingOrModelType == typeof(DateTime))
            {
                return new DateTimeBinder(TimeZoneHelper());
            }

            return null; // TODO: Find alternate.
        }
    }
}

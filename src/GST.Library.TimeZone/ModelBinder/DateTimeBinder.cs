using GST.Library.TimeZone.Services.Abstract;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace GST.Library.TimeZone.ModelBinder
{
    /// <summary>
    /// For Date in the URL
    /// </summary>
    public class DateTimeBinder : IModelBinder
    {
        private readonly ITimeZoneHelper TimeZoneHelper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_TimeZoneHelper"></param>
        public DateTimeBinder(ITimeZoneHelper _TimeZoneHelper)
        {
            TimeZoneHelper = _TimeZoneHelper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindingContext"></param>
        /// <returns></returns>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (string.IsNullOrEmpty(valueProviderResult.FirstValue))
            {
                return Task.CompletedTask;
            }

            DateTime datetime;
            if (DateTime.TryParse(valueProviderResult.FirstValue, null, DateTimeStyles.AdjustToUniversal, out datetime))
            {
                bindingContext.Result =
                    ModelBindingResult.Success(TimeZoneHelper.GetUtcTime(datetime));
            }
            else
            {
                // TODO: [Enhancement] Could be implemented in better way.  
                bindingContext.ModelState.TryAddModelError(
                    bindingContext.ModelName,
                    bindingContext.ModelMetadata
                    .ModelBindingMessageProvider.AttemptedValueIsInvalidAccessor(
                      valueProviderResult.ToString(), nameof(DateTime)));
            }

            return Task.CompletedTask;
        }
    }
}

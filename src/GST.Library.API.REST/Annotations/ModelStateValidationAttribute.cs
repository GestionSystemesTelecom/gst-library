using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GST.Library.API.REST.Annotations
{
    /// <summary>
    /// Allow to be really sure that model isn't null and is valid.
    /// </summary>
    /// <example>
    /// [ModelStateValidation]
    /// public class WoOoMethode() { //...
    /// </example>
    /// Thanks to [Filip W](http://www.strathweb.com/2012/10/clean-up-your-web-api-controllers-with-model-validation-and-null-check-filters/)
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class ModelStateValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {

            if (actionContext.ActionArguments.Any(kv => kv.Value == null))
            {
                actionContext.Result = new BadRequestObjectResult("Arguments cannot be null");
            }

            if (actionContext.ModelState.IsValid == false)
            {
                actionContext.Result = new BadRequestObjectResult(actionContext.ModelState);
            }
        }
    }
}

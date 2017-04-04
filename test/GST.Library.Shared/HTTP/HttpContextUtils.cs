using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace GST.Library.Shared.HTTP
{
    /// <summary>
    /// From http://stackoverflow.com/questions/38285815/how-to-write-unit-test-for-actionfilter-when-using-service-locator
    /// </summary>
    public class HttpContextUtils
    {
        public static ActionExecutingContext MockedActionExecutingContext (
            ModelStateDictionary modelState = null,
            ActionContext actionContext = null,
            IDictionary<string, object> actionArguments = null
        )
        {
            if (modelState == null)
            {
                modelState = new ModelStateDictionary();
            }

            if (actionContext == null)
            {
                actionContext = new ActionContext(
                   new Mock<HttpContext>().Object,
                   new Mock<RouteData>().Object,
                   new Mock<ActionDescriptor>().Object,
                   modelState);
            }

            if(actionArguments == null)
            {
                actionArguments = new Dictionary<string, object>();
            }
            
            IList<IFilterMetadata> filters = new List<IFilterMetadata>();
            Mock<ControllerBase> controller = new Mock<ControllerBase>();

           return new ActionExecutingContext(actionContext, filters, actionArguments, controller);
        }
    }
}

using GST.Library.API.REST.Annotations;
using GST.Library.Shared.HTTP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using Xunit;

namespace GST.Library.API.REST.Tests.Annotations
{
    public class ModelStateValidationAttributeTest
    {
        [Fact]
        public void MustReturnA400Response()
        {
            ModelStateValidationAttribute msva = new ModelStateValidationAttribute();

            IDictionary<string, object> actionArguments = new Dictionary<string, object>();
            actionArguments.Add("name", null);
            actionArguments.Add("age", null);
            actionArguments.Add("something", null);

            ActionExecutingContext actionExecutingContext = HttpContextUtils.MockedActionExecutingContext(null, null, actionArguments);

            msva.OnActionExecuting(actionExecutingContext);

            BadRequestObjectResult viewResult = actionExecutingContext.Result as BadRequestObjectResult;

            Assert.Equal(400, viewResult.StatusCode);
            Assert.Equal("Arguments cannot be null", viewResult.Value);
        }

        [Fact]
        public void MustReturnA400ResponseInvalidModel()
        {
            ModelStateValidationAttribute msva = new ModelStateValidationAttribute();

            ModelStateDictionary modelState = new ModelStateDictionary();
            modelState.AddModelError("name", "invalid");

            ActionExecutingContext actionExecutingContext = HttpContextUtils.MockedActionExecutingContext(modelState, null, null);

            msva.OnActionExecuting(actionExecutingContext);

            BadRequestObjectResult viewResult = actionExecutingContext.Result as BadRequestObjectResult;

            Assert.Equal(400, viewResult.StatusCode);
            Assert.Equal("invalid", ((dynamic)((IDictionary<string, object>)viewResult.Value)["name"])[0]);
        }

        [Fact]
        public void MustDoNothing()
        {
            ModelStateValidationAttribute msva = new ModelStateValidationAttribute();

            ActionExecutingContext actionExecutingContext = HttpContextUtils.MockedActionExecutingContext(null, null, null);

            msva.OnActionExecuting(actionExecutingContext);

            Assert.Null(actionExecutingContext.Result);
        }
    }
}

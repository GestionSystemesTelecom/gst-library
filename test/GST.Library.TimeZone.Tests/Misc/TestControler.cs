using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GST.Library.TimeZone.Tests.Misc
{
    [Route("datetime-test")]
    public class TestControler : Controller
    {

        [HttpPost]
        public IActionResult Post([FromBody] DateTimeModelIO model)
        {
            return new OkObjectResult(model);
        }

        [Authorize()]
        [HttpPost("secured")]
        public IActionResult PostSecured([FromBody] DateTimeModelIO model)
        {
            return new OkObjectResult(model);
        }
    }
}

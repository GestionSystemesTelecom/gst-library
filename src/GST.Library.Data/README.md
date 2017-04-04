# GST Library API REST

> Helper for building nice REST API

## Model State validation

Base on [Filip W](http://www.strathweb.com/2012/10/clean-up-your-web-api-controllers-with-model-validation-and-null-check-filters/)'s work.

This helper ensure that the Data Transfer Object (DTO) (also called "View Model"), is valid and not null.  
If the model is null or invalid an HTTP response 400 within errors for each faulted property.

How to use it :

```C#
using GST.Library.API.REST.Annotations;

namespace My.API.Controllers
{
    [Route("api/some")]
    public class SomeController : Controller
    {
        [HttpPost]
        [ModelStateValidation]
        public IActionResult Post([FromBody] SomeInputViewModel scope)
        {
            return new OkObjectResult("Well Done !");
        }
    }
}
```

## Pagination

Because pagination is something useful but redundant, here is a little help.  
This helper store pagination data in the Header of the HTTP request.

You can find four data :
 * int CurrentPage: The current page to show
 * int ItemsPerPage: The number of items to show per page
 * int TotalItems: The total number of items in the object
 * int TotalPages: The total number of page to show all items of the object

How to use it :

```C#
using GST.Library.API.REST.Pagination;

namespace My.API.Controllers
{
    [Route("api/some")]
    public class SomeController : Controller
    {

        private int defaultFirstPage = 1;
        private int defaultItemPerPage = 10;

        [HttpGet]
        public IActionResult Get([FromQuery]int? page, [FromQuery]int? limit)
        {
            // Do something nice

            List<Object> objList = new List<Object>();


            int currentPage = page == null || page < defaultFirstPage ? defaultFirstPage : (int)page;
            int currentItemPerPage = limit == null || limit < defaultItemPerPage ? defaultItemPerPage : (int)limit;
            int totalItem = objList.count();
            int totalPages = (int)Math.Ceiling((double)totalItem / currentItemPerPage);

            Response.AddPagination(currentPage, currentItemPerPage, totalItem, totalPages);

            return new OkObjectResult(/* Lot of paginated object */);
        }
    }
}
```


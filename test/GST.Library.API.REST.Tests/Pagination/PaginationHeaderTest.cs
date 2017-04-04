using GST.Library.API.REST.Pagination;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GST.Library.API.REST.Tests.Pagination
{
    public class PaginationHeaderTest
    {
        [Fact]
        public void MustAddHeaderData()
        {
            Mock<HttpResponse> httpResponseMock = new Mock<HttpResponse>();

            httpResponseMock.SetupGet(r => r.Headers).Returns(new HeaderDictionary());

            httpResponseMock.Object.AddPagination(1, 2, 3, 4);

            PaginationHeader deSerialized = Newtonsoft.Json.JsonConvert.DeserializeObject<PaginationHeader>(httpResponseMock.Object.Headers["Pagination"]);

            Assert.Equal(1, deSerialized.CurrentPage);
            Assert.Equal(2, deSerialized.ItemsPerPage);
            Assert.Equal(3, deSerialized.TotalItems);
            Assert.Equal(4, deSerialized.TotalPages);

            Assert.Equal("Pagination", httpResponseMock.Object.Headers["access-control-expose-headers"]);
        }
    }
}

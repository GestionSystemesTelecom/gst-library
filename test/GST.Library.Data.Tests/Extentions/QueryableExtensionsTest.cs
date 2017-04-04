using Xunit;
using System;
using System.Linq;
using System.Collections.Generic;
using GST.Library.Shared.Mock;
using GST.Library.Data.Extentions;

namespace GST.Library.Data.Tests.Extentions
{
    public class QueryableExtensionsTest
    {

        [Fact]
        public void testThrowExceptionOrderBy()
        {
            IQueryable<Bumb> data = new List<Bumb> { }.AsQueryable<Bumb>();

            Exception ex = Assert.ThrowsAny<ArgumentException>(()
              => data.OrderBy(new string[] { "propertyA", "SomethingThatDoesntExist" }, "desc"));

            Assert.StartsWith("'SomethingThatDoesntExist' is not a member of type 'GST.Library.Shared.Mock.Bumb'", ex.Message);
        }

        [Fact]
        public void testOrderBy()
        {

            IQueryable<Bumb> data = new List<Bumb>
            {
                new Bumb { propertyA = "a"},
                new Bumb { propertyA = "b"},
                new Bumb { propertyA = "c"},
                new Bumb { propertyA = "d"}
            }
            .AsQueryable<Bumb>();


            var result = data.OrderBy(new string[] { "propertyA" }, "desc");
            Bumb first = result.ToArray().First();
            Bumb last = result.ToArray().Last();

            Assert.Equal("d", first.propertyA);

            Assert.Equal("a", last.propertyA);
        }

        [Fact]
        public void testDontThrowException()
        {
            IQueryable<Bumb> data = new List<Bumb>
            {
                new Bumb { propertyA = "a"},
                new Bumb { propertyA = "b"},
                new Bumb { propertyA = "c"},
                new Bumb { propertyA = "d"}
            }
           . AsQueryable<Bumb>();


            var result = data.OrderBy(new string[] { }, "desc");
            Bumb first = result.ToArray().First();
            Bumb last = result.ToArray().Last();

            Assert.Equal("a", first.propertyA);
            Assert.Equal("d", last.propertyA);
        }
    }
}

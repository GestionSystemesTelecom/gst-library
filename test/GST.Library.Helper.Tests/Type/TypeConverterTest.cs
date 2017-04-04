using System;
using Xunit;
using GST.Library.Helper.Type;

namespace GST.Library.Helper.Tests.Type
{
    public class TypeConverterTest
    {
        [Fact]
        public void testMustTransformStringToDateTime()
        {
            Assert.Equal(
                ((DateTime)"2017-02-14".ToDateTime()).ToString("u"),
                "2017-02-14 00:00:00Z");
            Assert.Equal(
                ((DateTime)"2017-02".ToDateTime()).ToString("u"),
                "2017-02-01 00:00:00Z");
        }

        [Fact]
        public void testFailTransformStringToDateTime()
        {
            Assert.Null("(╯°□°）╯︵ ┻━━┻ !!!".ToDateTime());
        }
    }
}
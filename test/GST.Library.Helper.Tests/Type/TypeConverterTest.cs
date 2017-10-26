using System;
using Xunit;
using GST.Library.Helper.Type;

namespace GST.Library.Helper.Tests.Type
{
    public class TypeConverterTest
    {
        [Fact]
        public void MustTransformStringToDateTime()
        {
            Assert.Equal(
                "2017-02-14 00:00:00Z",
                ((DateTime)"2017-02-14".ToDateTime()).ToString("u"));
            Assert.Equal(
                "2017-02-01 00:00:00Z",
                ((DateTime)"2017-02".ToDateTime()).ToString("u"));
        }

        [Fact]
        public void FailTransformStringToDateTime()
        {
            Assert.Null("(╯°□°）╯︵ ┻━━┻ !!!".ToDateTime());
        }
    }
}
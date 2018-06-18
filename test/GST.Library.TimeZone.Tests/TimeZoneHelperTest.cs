using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;

namespace GST.Library.String.Tests
{
    public class TimeZoneHelperTest
    {
        public TimeZoneHelperTest()
        {
        }

        [Fact]
        public void GetUserLocalTime()
        {
            Mock<IHttpContextAccessor> _mockHttp = new Mock<IHttpContextAccessor>();

            _mockHttp.Setup(x => x.HttpContext.User).Returns(new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("zoneinfo", "Europe/Paris") })));

            DateTime dt1 = new TimeZone.Services.TimeZoneHelper(_mockHttp.Object).GetUserLocalTime();

            _mockHttp.Setup(x => x.HttpContext.User).Returns(new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("zoneinfo", "America/New_York") })));

            DateTime dt2 = new TimeZone.Services.TimeZoneHelper(_mockHttp.Object).GetUserLocalTime();

            Assert.NotEqual(dt1, dt2);
            Assert.Equal(6, Math.Round((dt1 - dt2).TotalHours));
        }

        [Theory]
        [MemberData(nameof(GetUserLocalTimeDate_ListData))]
        public void GetUserLocalTimeDate(string zoneinfo, DateTime dt, Func<dynamic, dynamic> assertion)
        {
            Mock<IHttpContextAccessor> _mockHttp = new Mock<IHttpContextAccessor>();
            _mockHttp.Setup(x => x.HttpContext.User).Returns(new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("zoneinfo", zoneinfo) })));
            DateTime dt1 = new TimeZone.Services.TimeZoneHelper(_mockHttp.Object).GetUserLocalTime(dt);
            assertion.Invoke(dt1);
        }

        public static IEnumerable<object[]> GetUserLocalTimeDate_ListData()
        {
            yield return new object[] {
                "Europe/Paris", new DateTime(2018, 06, 18, 13, 27, 00, DateTimeKind.Utc),
                (
                    (Func<dynamic, dynamic>) delegate(dynamic dt1)
                    {
                        Assert.Equal(new DateTime(2018, 06, 18, 15, 27, 00), dt1);
                        return null;
                    }
                )
            };

            yield return new object[] {
                "America/New_York", new DateTime(2018, 06, 18, 13, 27, 00, DateTimeKind.Utc),
                (
                    (Func<dynamic, dynamic>) delegate(dynamic dt1)
                    {
                        Assert.Equal(new DateTime(2018, 06, 18, 09, 27, 00), dt1);
                        return null;
                    }
                )
            };
        }

        [Theory]
        [MemberData(nameof(GetUtcTime_ListData))]
        public void GetUtcTime(string zoneinfo, DateTime dt, Func<dynamic, dynamic> assertion)
        {
            Mock<IHttpContextAccessor> _mockHttp = new Mock<IHttpContextAccessor>();
            _mockHttp.Setup(x => x.HttpContext.User).Returns(new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("zoneinfo", zoneinfo) })));
            DateTime dt1 = new TimeZone.Services.TimeZoneHelper(_mockHttp.Object).GetUtcTime(dt);
            assertion.Invoke(dt1);
        }

        public static IEnumerable<object[]> GetUtcTime_ListData()
        {
            yield return new object[] {
                "Europe/Paris", new DateTime(2018, 06, 18, 13, 27, 00),
                (
                    (Func<dynamic, dynamic>) delegate(dynamic dt1)
                    {
                        // Summer time (+2h)
                        Assert.Equal(new DateTime(2018, 06, 18, 11, 27, 00), dt1);
                        return null;
                    }
                )
            };

            yield return new object[] {
                "Europe/Paris", new DateTime(2018, 12, 18, 13, 27, 00),
                (
                    (Func<dynamic, dynamic>) delegate(dynamic dt1)
                    {
                        // Summer time (+1h)
                        Assert.Equal(new DateTime(2018, 12, 18, 12, 27, 00), dt1);
                        return null;
                    }
                )
            };

            yield return new object[] {
                "America/New_York", new DateTime(2018, 06, 18, 13, 27, 00),
                (
                    (Func<dynamic, dynamic>) delegate(dynamic dt1)
                    {
                        // Summer time (-4h)
                        Assert.Equal(new DateTime(2018, 06, 18, 17, 27, 00), dt1);
                        return null;
                    }
                )
            };

            yield return new object[] {
                "America/New_York", new DateTime(2018, 12, 18, 13, 27, 00),
                (
                    (Func<dynamic, dynamic>) delegate(dynamic dt1)
                    {
                        // Winter time (-5h)
                        Assert.Equal(new DateTime(2018, 12, 18, 18, 27, 00), dt1);
                        return null;
                    }
                )
            };
        }
    }
}

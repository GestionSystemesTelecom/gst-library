using GST.Fake.Authentication.JwtBearer;
using GST.Library.TimeZone.Tests.Misc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GST.Library.TimeZone.Tests
{
    public class TimeZoneHelperFonctionnalTest : IClassFixture<TestFixture<Startup>>
    {
        private readonly HttpClient client;
        private TestFixture<Startup> fixture;
        private readonly string format = "dd/MM/yyyy HH:mm:ss";
        private readonly CultureInfo provider = new CultureInfo("fr-FR");

        public TimeZoneHelperFonctionnalTest(TestFixture<Startup> _fixture)
        {
            fixture = _fixture;
            client = fixture.Client;
        }

        [Fact]
        public void WithoutUserInfo()
        {
            var content = new StringContent(JsonConvert.SerializeObject(new DateTimeModelIO {
                DateWithoutTimeZoneAdaptation = DateTime.ParseExact("01/01/2018 01:01:01", format, provider),
                DateWithTimeZoneAdaptation = DateTime.ParseExact("02/01/2018 01:01:01", format, provider),
            }), Encoding.UTF8, "application/json");

            var response = client.PostAsync("datetime-test", content).Result;

            var result = response.Content.ReadAsStringAsync().Result;

            DateTimeModelIO resultModel = JsonConvert.DeserializeObject<DateTimeModelIO>(result, new IsoDateTimeConverter { DateTimeFormat = format });
            Assert.Equal(DateTime.ParseExact("01/01/2018 01:01:01", format, provider), resultModel.DateWithoutTimeZoneAdaptation);
            Assert.Equal(DateTime.ParseExact("02/01/2018 01:01:01", format, provider), resultModel.DateWithTimeZoneAdaptation);
        }

        [Fact]
        public void WithUserInfo()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            data.zoneinfo = "Europe/Paris";

            fixture.Client.SetFakeBearerToken("guys", new[] { "none" }, (object)data);

            DateTimeModelIO expectedModel = new DateTimeModelIO
            {
                DateWithoutTimeZoneAdaptation = DateTime.ParseExact("01/01/2018 01:01:01", format, provider),
                DateWithTimeZoneAdaptation = DateTime.ParseExact("02/01/2018 01:01:01", format, provider),
                Sub = new SubDateTimeModelIO
                {
                    SubDateWithoutTimeZoneAdaptation = DateTime.ParseExact("03/01/2018 01:01:01", format, provider),
                    SubDateWithTimeZoneAdaptation = DateTime.ParseExact("04/01/2018 01:01:01", format, provider),
                }
            };

            var content = new StringContent(JsonConvert.SerializeObject(expectedModel), Encoding.UTF8, "application/json");

            var response = fixture.Client.PostAsync("datetime-test/secured", content).Result;
        
            var result = response.Content.ReadAsStringAsync().Result;

            DateTimeModelIO resultModel = JsonConvert.DeserializeObject<DateTimeModelIO>(result, new IsoDateTimeConverter { DateTimeFormat = format });
            Assert.Equal(expectedModel.DateWithoutTimeZoneAdaptation, resultModel.DateWithoutTimeZoneAdaptation);
            Assert.Equal(expectedModel.DateWithTimeZoneAdaptation, resultModel.DateWithTimeZoneAdaptation);
            Assert.Equal(expectedModel.Sub.SubDateWithoutTimeZoneAdaptation, resultModel.Sub.SubDateWithoutTimeZoneAdaptation);
            Assert.Equal(expectedModel.Sub.SubDateWithTimeZoneAdaptation, resultModel.Sub.SubDateWithTimeZoneAdaptation);
        }
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;

namespace GST.Library.TimeZone.Tests.Misc
{
    public class TestFixture<TStartup> : IDisposable where TStartup : class
    {
        public readonly TestServer server;
        public HttpClient Client { get; }

        public TestFixture()
        {
            IWebHostBuilder builder = new WebHostBuilder()
               .UseStartup<TStartup>()
               .UseEnvironment("Test");

            server = new TestServer(builder)
            {
                BaseAddress = new Uri("http://localhost/")
            };

            Client = server.CreateClient();
            Client.BaseAddress = new Uri("http://localhost/");
            server.CreateHandler();
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}

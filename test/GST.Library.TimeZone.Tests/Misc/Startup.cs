using GST.Fake.Authentication.JwtBearer;
using GST.Library.TimeZone.DependencyInjection;
using GST.Library.TimeZone.ModelBinder;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace GST.Library.TimeZone.Tests.Misc
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTimeZoneService();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = FakeJwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = FakeJwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = FakeJwtBearerDefaults.AuthenticationScheme;
            }).AddFakeJwtBearer();

            services.AddMvc(option => option.UseDateTimeProvider(services))
                        .AddJsonOptions(jsonOption =>
                            jsonOption.UseDateTimeConverter(services));
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}

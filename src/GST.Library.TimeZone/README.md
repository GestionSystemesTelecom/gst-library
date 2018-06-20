# GST Library TimeZone

> Helper for building TimeZone

Use `zoneinfo` claim of User attributes for mapping UTC datetime to the user TimeZone.

You can use the services `ITimeZoneHelper` in your own.

## Install

Like all [Nuget](https://www.nuget.org/packages/GST.Library.TimeZone/) package: `Install-Package GST.Library.TimeZone`

## How to use it

In the `Startup` file, You have to add the code bellow :

```C#
public void ConfigureServices(IServiceCollection services)
{
//...
 services.AddTimeZoneService();
 services.AddMvc(option => option.UseDateTimeProvider(services))
                        .AddJsonOptions(jsonOption =>
                            jsonOption.UseDateTimeConverter(services));
//...
}
```

## Sources

* [Elegantly dealing with TimeZones in MVC Core / WebApi](https://vikutech.blogspot.com/2017/05/elegantly-dealing-with-timezones-in-mvc-core-webapi.html)
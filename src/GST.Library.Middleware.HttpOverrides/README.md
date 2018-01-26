# GST Library Middleware HttpOverrides

> This is a *fork* of [ForwardedHeadersMiddleware](https://github.com/aspnet/BasicMiddleware)

When an .Net Core app run in a container, the original `ForwardedHeadersMiddleware` don't do the job.  
Please see this issue on Github [app.UseForwardedHeaders gives different result when run in container #242](https://github.com/aspnet/BasicMiddleware/issues/242)

## Install

Like all [Nuget](https://www.nuget.org/packages/GST.Library.Middleware.HttpOverrides/) package: `Install-Package GST.Library.Middleware.HttpOverrides`

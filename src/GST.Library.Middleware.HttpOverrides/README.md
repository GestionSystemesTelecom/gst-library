# GST Library Middleware HttpOverrides

> This is a *fork* of [ForwardedHeadersMiddleware](https://github.com/aspnet/BasicMiddleware)

When an .Net Core app run in a container, the original `ForwardedHeadersMiddleware` don't do the job.  
Please see this issue on Github [app.UseForwardedHeaders gives different result when run in container #242](https://github.com/aspnet/BasicMiddleware/issues/242)

This library can be used like the original.

If your application run inside a container (Docker) and behind a reverse proxy like Nginx or [Traefik](traefik.io), you can configure the middleware like bellow : 

```C#
app.UseGSTForwardedHeaders(new GST.Library.Middleware.HttpOverrides.Builder.ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XRealIp,
    ForceXForxardedOrRealIp = true,
});
```

## Install

Like all [Nuget](https://www.nuget.org/packages/GST.Library.Middleware.HttpOverrides/) package: `Install-Package GST.Library.Middleware.HttpOverrides`

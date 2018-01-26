﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GST.Library.Middleware.HttpOverrides.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace GST.Library.Middleware.HttpOverrides.Tests
{
    public class ForwardedHeadersMiddlewareTests
    {
        /*[Fact]
        public async Task XForwardedForDefaultSettingsChangeRemoteIpAndPort()
        {
            var builder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.UseForwardedHeaders(new ForwardedHeadersOptions
                    {
                        ForwardedHeaders = ForwardedHeaders.XForwardedFor
                    });
                });
            var server = new TestServer(builder);

            var request = new HttpRequestMessage(HttpMethod.Get, "");
            request.Headers.Add("X-Forwarded-For", "11.111.111.11:9090");

            var context = await server.CreateClient().SendAsync(c =>
            {
                c.Request.Headers["X-Forwarded-For"] = "11.111.111.11:9090";
            });

            Assert.Equal("11.111.111.11", context.Connection.RemoteIpAddress.ToString());
            Assert.Equal(9090, context.Connection.RemotePort);
            // No Original set if RemoteIpAddress started null.
            Assert.False(context.Request.Headers.ContainsKey("X-Original-For"));
            // Should have been consumed and removed
            Assert.False(context.Request.Headers.ContainsKey("X-Forwarded-For"));
        }

        [Theory]
        [InlineData(1, "11.111.111.11.12345", "10.0.0.1", 99)] // Invalid
        public async Task XForwardedForFirstValueIsInvalid(int limit, string header, string expectedIp, int expectedPort)
        {
            var builder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.UseForwardedHeaders(new ForwardedHeadersOptions
                    {
                        ForwardedHeaders = ForwardedHeaders.XForwardedFor,
                        ForwardLimit = limit,
                    });
                });
            var server = new TestServer(builder);

            var context = await server.SendAsync(c =>
            {
                c.Request.Headers["X-Forwarded-For"] = header;
                c.Connection.RemoteIpAddress = IPAddress.Parse("10.0.0.1");
                c.Connection.RemotePort = 99;
            });

            Assert.Equal(expectedIp, context.Connection.RemoteIpAddress.ToString());
            Assert.Equal(expectedPort, context.Connection.RemotePort);
            Assert.False(context.Request.Headers.ContainsKey("X-Original-For"));
            Assert.True(context.Request.Headers.ContainsKey("X-Forwarded-For"));
            Assert.Equal(header, context.Request.Headers["X-Forwarded-For"]);
        }

        [Theory]
        [InlineData(1, "11.111.111.11:12345", "11.111.111.11", 12345, "", false)]
        [InlineData(1, "11.111.111.11:12345", "11.111.111.11", 12345, "", true)]
        [InlineData(10, "11.111.111.11:12345", "11.111.111.11", 12345, "", false)]
        [InlineData(10, "11.111.111.11:12345", "11.111.111.11", 12345, "", true)]
        [InlineData(1, "12.112.112.12:23456, 11.111.111.11:12345", "11.111.111.11", 12345, "12.112.112.12:23456", false)]
        [InlineData(1, "12.112.112.12:23456, 11.111.111.11:12345", "11.111.111.11", 12345, "12.112.112.12:23456", true)]
        [InlineData(2, "12.112.112.12:23456, 11.111.111.11:12345", "12.112.112.12", 23456, "", false)]
        [InlineData(2, "12.112.112.12:23456, 11.111.111.11:12345", "12.112.112.12", 23456, "", true)]
        [InlineData(10, "12.112.112.12:23456, 11.111.111.11:12345", "12.112.112.12", 23456, "", false)]
        [InlineData(10, "12.112.112.12:23456, 11.111.111.11:12345", "12.112.112.12", 23456, "", true)]
        [InlineData(10, "12.112.112.12.23456, 11.111.111.11:12345", "11.111.111.11", 12345, "12.112.112.12.23456", false)] // Invalid 2nd value
        [InlineData(10, "12.112.112.12.23456, 11.111.111.11:12345", "11.111.111.11", 12345, "12.112.112.12.23456", true)] // Invalid 2nd value
        [InlineData(10, "13.113.113.13:34567, 12.112.112.12.23456, 11.111.111.11:12345", "11.111.111.11", 12345, "13.113.113.13:34567,12.112.112.12.23456", false)] // Invalid 2nd value
        [InlineData(10, "13.113.113.13:34567, 12.112.112.12.23456, 11.111.111.11:12345", "11.111.111.11", 12345, "13.113.113.13:34567,12.112.112.12.23456", true)] // Invalid 2nd value
        [InlineData(2, "13.113.113.13:34567, 12.112.112.12:23456, 11.111.111.11:12345", "12.112.112.12", 23456, "13.113.113.13:34567", false)]
        [InlineData(2, "13.113.113.13:34567, 12.112.112.12:23456, 11.111.111.11:12345", "12.112.112.12", 23456, "13.113.113.13:34567", true)]
        [InlineData(3, "13.113.113.13:34567, 12.112.112.12:23456, 11.111.111.11:12345", "13.113.113.13", 34567, "", false)]
        [InlineData(3, "13.113.113.13:34567, 12.112.112.12:23456, 11.111.111.11:12345", "13.113.113.13", 34567, "", true)]
        public async Task XForwardedForForwardLimit(int limit, string header, string expectedIp, int expectedPort, string remainingHeader, bool requireSymmetry)
        {
            var builder = new WebHostBuilder()
                .Configure(app =>
                {
                    var options = new ForwardedHeadersOptions
                    {
                        ForwardedHeaders = ForwardedHeaders.XForwardedFor,
                        RequireHeaderSymmetry = requireSymmetry,
                        ForwardLimit = limit,
                    };
                    options.KnownProxies.Clear();
                    options.KnownNetworks.Clear();
                    app.UseForwardedHeaders(options);
                });
            var server = new TestServer(builder);

            var context = await server.SendAsync(c =>
            {
                c.Request.Headers["X-Forwarded-For"] = header;
                c.Connection.RemoteIpAddress = IPAddress.Parse("10.0.0.1");
                c.Connection.RemotePort = 99;
            });

            Assert.Equal(expectedIp, context.Connection.RemoteIpAddress.ToString());
            Assert.Equal(expectedPort, context.Connection.RemotePort);
            Assert.Equal(remainingHeader, context.Request.Headers["X-Forwarded-For"].ToString());
        }

        [Theory]
        [InlineData("11.111.111.11", false)]
        [InlineData("127.0.0.1", true)]
        [InlineData("127.0.1.1", true)]
        [InlineData("::1", true)]
        [InlineData("::", false)]
        public async Task XForwardedForLoopback(string originalIp, bool expectForwarded)
        {
            var builder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.UseForwardedHeaders(new ForwardedHeadersOptions
                    {
                        ForwardedHeaders = ForwardedHeaders.XForwardedFor,
                    });
                });
            var server = new TestServer(builder);

            var context = await server.SendAsync(c =>
            {
                c.Request.Headers["X-Forwarded-For"] = "10.0.0.1:1234";
                c.Connection.RemoteIpAddress = IPAddress.Parse(originalIp);
                c.Connection.RemotePort = 99;
            });

            if (expectForwarded)
            {
                Assert.Equal("10.0.0.1", context.Connection.RemoteIpAddress.ToString());
                Assert.Equal(1234, context.Connection.RemotePort);
                Assert.True(context.Request.Headers.ContainsKey("X-Original-For"));
                Assert.Equal(new IPEndPoint(IPAddress.Parse(originalIp), 99).ToString(),
                    context.Request.Headers["X-Original-For"]);
            }
            else
            {
                Assert.Equal(originalIp, context.Connection.RemoteIpAddress.ToString());
                Assert.Equal(99, context.Connection.RemotePort);
                Assert.False(context.Request.Headers.ContainsKey("X-Original-For"));
            }
        }

        [Theory]
        [InlineData(1, "11.111.111.11:12345", "20.0.0.1", "10.0.0.1", 99, false)]
        [InlineData(1, "11.111.111.11:12345", "20.0.0.1", "10.0.0.1", 99, true)]
        [InlineData(1, "", "10.0.0.1", "10.0.0.1", 99, false)]
        [InlineData(1, "", "10.0.0.1", "10.0.0.1", 99, true)]
        [InlineData(1, "11.111.111.11:12345", "10.0.0.1", "11.111.111.11", 12345, false)]
        [InlineData(1, "11.111.111.11:12345", "10.0.0.1", "11.111.111.11", 12345, true)]
        [InlineData(1, "12.112.112.12:23456, 11.111.111.11:12345", "10.0.0.1", "11.111.111.11", 12345, false)]
        [InlineData(1, "12.112.112.12:23456, 11.111.111.11:12345", "10.0.0.1", "11.111.111.11", 12345, true)]
        [InlineData(1, "12.112.112.12:23456, 11.111.111.11:12345", "10.0.0.1,11.111.111.11", "11.111.111.11", 12345, false)]
        [InlineData(1, "12.112.112.12:23456, 11.111.111.11:12345", "10.0.0.1,11.111.111.11", "11.111.111.11", 12345, true)]
        [InlineData(2, "12.112.112.12:23456, 11.111.111.11:12345", "10.0.0.1,11.111.111.11", "12.112.112.12", 23456, false)]
        [InlineData(2, "12.112.112.12:23456, 11.111.111.11:12345", "10.0.0.1,11.111.111.11", "12.112.112.12", 23456, true)]
        [InlineData(1, "12.112.112.12:23456, 11.111.111.11:12345", "10.0.0.1,11.111.111.11,12.112.112.12", "11.111.111.11", 12345, false)]
        [InlineData(1, "12.112.112.12:23456, 11.111.111.11:12345", "10.0.0.1,11.111.111.11,12.112.112.12", "11.111.111.11", 12345, true)]
        [InlineData(2, "12.112.112.12:23456, 11.111.111.11:12345", "10.0.0.1,11.111.111.11,12.112.112.12", "12.112.112.12", 23456, false)]
        [InlineData(2, "12.112.112.12:23456, 11.111.111.11:12345", "10.0.0.1,11.111.111.11,12.112.112.12", "12.112.112.12", 23456, true)]
        [InlineData(3, "13.113.113.13:34567, 12.112.112.12:23456, 11.111.111.11:12345", "10.0.0.1,11.111.111.11,12.112.112.12", "13.113.113.13", 34567, false)]
        [InlineData(3, "13.113.113.13:34567, 12.112.112.12:23456, 11.111.111.11:12345", "10.0.0.1,11.111.111.11,12.112.112.12", "13.113.113.13", 34567, true)]
        [InlineData(3, "13.113.113.13:34567, 12.112.112.12;23456, 11.111.111.11:12345", "10.0.0.1,11.111.111.11,12.112.112.12", "11.111.111.11", 12345, false)] // Invalid 2nd IP
        [InlineData(3, "13.113.113.13:34567, 12.112.112.12;23456, 11.111.111.11:12345", "10.0.0.1,11.111.111.11,12.112.112.12", "11.111.111.11", 12345, true)] // Invalid 2nd IP
        [InlineData(3, "13.113.113.13;34567, 12.112.112.12:23456, 11.111.111.11:12345", "10.0.0.1,11.111.111.11,12.112.112.12", "12.112.112.12", 23456, false)] // Invalid 3rd IP
        [InlineData(3, "13.113.113.13;34567, 12.112.112.12:23456, 11.111.111.11:12345", "10.0.0.1,11.111.111.11,12.112.112.12", "12.112.112.12", 23456, true)] // Invalid 3rd IP
        public async Task XForwardedForForwardKnownIps(int limit, string header, string knownIPs, string expectedIp, int expectedPort, bool requireSymmetry)
        {
            var builder = new WebHostBuilder()
                .Configure(app =>
                {
                    var options = new ForwardedHeadersOptions
                    {
                        ForwardedHeaders = ForwardedHeaders.XForwardedFor,
                        RequireHeaderSymmetry = requireSymmetry,
                        ForwardLimit = limit,
                    };
                    foreach (var ip in knownIPs.Split(',').Select(text => IPAddress.Parse(text)))
                    {
                        options.KnownProxies.Add(ip);
                    }
                    app.UseForwardedHeaders(options);
                });
            var server = new TestServer(builder);

            var context = await server.SendAsync(c =>
            {
                c.Request.Headers["X-Forwarded-For"] = header;
                c.Connection.RemoteIpAddress = IPAddress.Parse("10.0.0.1");
                c.Connection.RemotePort = 99;
            });

            Assert.Equal(expectedIp, context.Connection.RemoteIpAddress.ToString());
            Assert.Equal(expectedPort, context.Connection.RemotePort);
        }

        [Fact]
        public async Task XForwardedForOverrideBadIpDoesntChangeRemoteIp()
        {
            var builder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.UseForwardedHeaders(new ForwardedHeadersOptions
                    {
                        ForwardedHeaders = ForwardedHeaders.XForwardedFor
                    });
                });
            var server = new TestServer(builder);

            var context = await server.SendAsync(c =>
            {
                c.Request.Headers["X-Forwarded-For"] = "BAD-IP";
            });

            Assert.Null(context.Connection.RemoteIpAddress);
        }

        [Fact]
        public async Task XForwardedHostOverrideChangesRequestHost()
        {
            var builder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.UseForwardedHeaders(new ForwardedHeadersOptions
                    {
                        ForwardedHeaders = ForwardedHeaders.XForwardedHost
                    });
                });
            var server = new TestServer(builder);

            var context = await server.SendAsync(c =>
            {
                c.Request.Headers["X-Forwarded-Host"] = "testhost";
            });

            Assert.Equal("testhost", context.Request.Host.ToString());
        }

        [Theory]
        [InlineData(0, "h1", "http")]
        [InlineData(1, "", "http")]
        [InlineData(1, "h1", "h1")]
        [InlineData(3, "h1", "h1")]
        [InlineData(1, "h2, h1", "h1")]
        [InlineData(2, "h2, h1", "h2")]
        [InlineData(10, "h3, h2, h1", "h3")]
        public async Task XForwardedProtoOverrideChangesRequestProtocol(int limit, string header, string expected)
        {
            var builder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.UseForwardedHeaders(new ForwardedHeadersOptions
                    {
                        ForwardedHeaders = ForwardedHeaders.XForwardedProto,
                        ForwardLimit = limit,
                    });
                });
            var server = new TestServer(builder);

            var context = await server.SendAsync(c =>
            {
                c.Request.Headers["X-Forwarded-Proto"] = header;
            });

            Assert.Equal(expected, context.Request.Scheme);
        }

        [Theory]
        [InlineData(0, "h1", "::1", "http")]
        [InlineData(1, "", "::1", "http")]
        [InlineData(1, "h1", "::1", "h1")]
        [InlineData(3, "h1", "::1", "h1")]
        [InlineData(3, "h2, h1", "::1", "http")]
        [InlineData(5, "h2, h1", "::1, ::1", "h2")]
        [InlineData(10, "h3, h2, h1", "::1, ::1, ::1", "h3")]
        [InlineData(10, "h3, h2, h1", "::1, badip, ::1", "h1")]
        public async Task XForwardedProtoOverrideLimitedByXForwardedForCount(int limit, string protoHeader, string forHeader, string expected)
        {
            var builder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.UseForwardedHeaders(new ForwardedHeadersOptions
                    {
                        ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedFor,
                        RequireHeaderSymmetry = true,
                        ForwardLimit = limit,
                    });
                });
            var server = new TestServer(builder);

            var context = await server.SendAsync(c =>
            {
                c.Request.Headers["X-Forwarded-Proto"] = protoHeader;
                c.Request.Headers["X-Forwarded-For"] = forHeader;
            });

            Assert.Equal(expected, context.Request.Scheme);
        }

        [Theory]
        [InlineData(0, "h1", "::1", "http")]
        [InlineData(1, "", "::1", "http")]
        [InlineData(1, "h1", "", "h1")]
        [InlineData(1, "h1", "::1", "h1")]
        [InlineData(3, "h1", "::1", "h1")]
        [InlineData(3, "h1", "::1, ::1", "h1")]
        [InlineData(3, "h2, h1", "::1", "h2")]
        [InlineData(5, "h2, h1", "::1, ::1", "h2")]
        [InlineData(10, "h3, h2, h1", "::1, ::1, ::1", "h3")]
        [InlineData(10, "h3, h2, h1", "::1, badip, ::1", "h1")]
        public async Task XForwardedProtoOverrideCanBeIndependentOfXForwardedForCount(int limit, string protoHeader, string forHeader, string expected)
        {
            var builder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.UseForwardedHeaders(new ForwardedHeadersOptions
                    {
                        ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedFor,
                        RequireHeaderSymmetry = false,
                        ForwardLimit = limit,
                    });
                });
            var server = new TestServer(builder);

            var context = await server.SendAsync(c =>
            {
                c.Request.Headers["X-Forwarded-Proto"] = protoHeader;
                c.Request.Headers["X-Forwarded-For"] = forHeader;
            });

            Assert.Equal(expected, context.Request.Scheme);
        }

        [Theory]
        [InlineData("", "", "::1", false, "http")]
        [InlineData("h1", "", "::1", false, "http")]
        [InlineData("h1", "F::", "::1", false, "h1")]
        [InlineData("h1", "F::", "E::", false, "h1")]
        [InlineData("", "", "::1", true, "http")]
        [InlineData("h1", "", "::1", true, "http")]
        [InlineData("h1", "F::", "::1", true, "h1")]
        [InlineData("h1", "", "F::", true, "http")]
        [InlineData("h1", "E::", "F::", true, "http")]
        [InlineData("h2, h1", "", "::1", true, "http")]
        [InlineData("h2, h1", "F::, D::", "::1", true, "h1")]
        [InlineData("h2, h1", "E::, D::", "F::", true, "http")]
        public async Task XForwardedProtoOverrideLimitedByLoopback(string protoHeader, string forHeader, string remoteIp, bool loopback, string expected)
        {
            var builder = new WebHostBuilder()
                .Configure(app =>
                {
                    var options = new ForwardedHeadersOptions
                    {
                        ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedFor,
                        RequireHeaderSymmetry = true,
                        ForwardLimit = 5,
                    };
                    if (!loopback)
                    {
                        options.KnownNetworks.Clear();
                        options.KnownProxies.Clear();
                    }
                    app.UseForwardedHeaders(options);
                });
            var server = new TestServer(builder);

            var context = await server.SendAsync(c =>
            {
                c.Request.Headers["X-Forwarded-Proto"] = protoHeader;
                c.Request.Headers["X-Forwarded-For"] = forHeader;
                c.Connection.RemoteIpAddress = IPAddress.Parse(remoteIp);
            });

            Assert.Equal(expected, context.Request.Scheme);
        }

        [Fact]
        public void AllForwardsDisabledByDefault()
        {
            var options = new ForwardedHeadersOptions();
            Assert.True(options.ForwardedHeaders == ForwardedHeaders.None);
            Assert.Equal(1, options.ForwardLimit);
            Assert.Single(options.KnownNetworks);
            Assert.Single(options.KnownProxies);
        }

        [Fact]
        public async Task AllForwardsEnabledChangeRequestRemoteIpHostandProtocol()
        {
            var builder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.UseForwardedHeaders(new ForwardedHeadersOptions
                    {
                        ForwardedHeaders = ForwardedHeaders.All
                    });
                });
            var server = new TestServer(builder);

            var context = await server.SendAsync(c =>
            {
                c.Request.Headers["X-Forwarded-Proto"] = "Protocol";
                c.Request.Headers["X-Forwarded-For"] = "11.111.111.11";
                c.Request.Headers["X-Forwarded-Host"] = "testhost";
            });

            Assert.Equal("11.111.111.11", context.Connection.RemoteIpAddress.ToString());
            Assert.Equal("testhost", context.Request.Host.ToString());
            Assert.Equal("Protocol", context.Request.Scheme);
        }

        [Fact]
        public async Task AllOptionsDisabledRequestDoesntChange()
        {
            var builder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.UseForwardedHeaders(new ForwardedHeadersOptions
                    {
                        ForwardedHeaders = ForwardedHeaders.None
                    });
                });
            var server = new TestServer(builder);

            var context = await server.SendAsync(c =>
            {
                c.Request.Headers["X-Forwarded-Proto"] = "Protocol";
                c.Request.Headers["X-Forwarded-For"] = "11.111.111.11";
                c.Request.Headers["X-Forwarded-Host"] = "otherhost";
            });

            Assert.Null(context.Connection.RemoteIpAddress);
            Assert.Equal("localhost", context.Request.Host.ToString());
            Assert.Equal("http", context.Request.Scheme);
        }

        [Fact]
        public async Task PartiallyEnabledForwardsPartiallyChangesRequest()
        {
            var builder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.UseForwardedHeaders(new ForwardedHeadersOptions
                    {
                        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                    });
                });
            var server = new TestServer(builder);

            var context = await server.SendAsync(c =>
            {
                c.Request.Headers["X-Forwarded-Proto"] = "Protocol";
                c.Request.Headers["X-Forwarded-For"] = "11.111.111.11";
            });

            Assert.Equal("11.111.111.11", context.Connection.RemoteIpAddress.ToString());
            Assert.Equal("localhost", context.Request.Host.ToString());
            Assert.Equal("Protocol", context.Request.Scheme);
        }*/
    }
}

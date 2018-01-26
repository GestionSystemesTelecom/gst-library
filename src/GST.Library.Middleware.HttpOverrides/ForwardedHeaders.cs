// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace GST.Library.Middleware.HttpOverrides
{
    [Flags]
    public enum ForwardedHeaders
    {
        None = 0,
        XForwardedFor = 1 << 0,
        XForwardedHost = 1 << 1,
        XForwardedProto = 1 << 2,
        XRealIp = 1 << 3,
        All = XForwardedFor | XForwardedHost | XForwardedProto | XRealIp
    }
}

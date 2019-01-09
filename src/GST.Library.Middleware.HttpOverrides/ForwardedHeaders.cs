// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace GST.Library.Middleware.HttpOverrides
{
    /// <summary>
    /// ForwardedHeaders
    /// </summary>
    [Flags]
    public enum ForwardedHeaders
    {
        /// <summary>
        /// Node
        /// </summary>
        None = 0,
        /// <summary>
        /// XForwardedFor
        /// </summary>
        XForwardedFor = 1 << 0,
        /// <summary>
        /// XForwardedHost
        /// </summary>
        XForwardedHost = 1 << 1,
        /// <summary>
        /// XForwardedProto
        /// </summary>
        XForwardedProto = 1 << 2,
        /// <summary>
        /// XRealIp
        /// </summary>
        XRealIp = 1 << 3,
        /// <summary>
        /// All
        /// </summary>
        All = XForwardedFor | XForwardedHost | XForwardedProto | XRealIp
    }
}

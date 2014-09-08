// --------------------------------------------------------------------------
//  <copyright file="IApiManagementEndpoint.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------

namespace MS.Azure.ApiManagement
{
    using System;

    public interface IApiManagementEndpoint
    {
        string AccessToken { get; }

        Uri BaseAddress { get; }

        string Identifier { get; }

        string Version { get; }

        string CreateAccessToken(DateTime expiry);

        Uri MapPath(string path);

        Uri MapPath(string pathFormat, params object[] args);
    }
}
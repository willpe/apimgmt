// --------------------------------------------------------------------------
//  <copyright file="ApiManagementEndpoint.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------

namespace MS.Azure.ApiManagement
{
    using System;
    using System.Collections.Concurrent;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Security.Cryptography;
    using System.Text;

    using MS.Azure.ApiManagement.Utils;

    public class ApiManagementEndpoint
    {
        private const string EdgeVersion = "2014-02-14-preview";

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        private static readonly ApiManagementEndpoint _default;

        private static readonly ConcurrentDictionary<string, ApiManagementEndpoint> endpointCache;
        private readonly string accessToken;

        private readonly Uri baseAddress;
        private readonly string identifier;
        private readonly string key;
        private readonly string version;

        static ApiManagementEndpoint()
        {
            endpointCache = new ConcurrentDictionary<string, ApiManagementEndpoint>();
            _default = ApiManagementEndpoint.FromConnectionString("ApiManagement");
        }

        public ApiManagementEndpoint(Uri baseAddress, string identifier = null, string key = null, string version = EdgeVersion, string accessToken = null)
        {
            this.baseAddress = baseAddress;
            this.identifier = identifier;
            this.key = key;
            this.version = version;
            this.accessToken = accessToken;
        }

        public static ApiManagementEndpoint Default
        {
            get { return _default; }
        }

        public string AccessToken
        {
            get { return this.accessToken; }
        }

        public Uri BaseAddress
        {
            get { return this.baseAddress; }
        }

        public string Identifier
        {
            get { return this.identifier; }
        }

        public string Version
        {
            get { return this.version; }
        }

        public static ApiManagementEndpoint FromConnectionString(string nameOrConnectionString)
        {
            if (string.IsNullOrEmpty(nameOrConnectionString))
            {
                throw new ArgumentNullException("nameOrConnectionString");
            }

            var connStr = ConfigurationManager.ConnectionStrings[nameOrConnectionString];
            var connectionStringValue = connStr != null ? connStr.ConnectionString : nameOrConnectionString;

            ConnectionString connectionString;
            if (!ConnectionString.TryParse(connectionStringValue, out connectionString) || connectionString == null || (string.IsNullOrEmpty(connectionString["uri"]) && string.IsNullOrEmpty(connectionString["serviceName"])))
            {
                return null;
            }

            ApiManagementEndpoint endpoint;
            if (!endpointCache.TryGetValue(connectionStringValue, out endpoint))
            {
                var uri = new Uri(connectionString["uri"] ?? string.Format("https://{0}.management.azure-api.net", connectionString["serviceName"]), UriKind.Absolute);
                endpoint = new ApiManagementEndpoint(uri, connectionString["identifier"], connectionString["key"], connectionString["version"] ?? EdgeVersion, connectionString["accessToken"]);

                endpointCache.AddOrUpdate(connectionStringValue, endpoint, (s, e) => e);
            }

            return endpoint;
        }

        public string CreateAccessToken(DateTime expiry)
        {
            if (!string.IsNullOrEmpty(this.accessToken))
            {
                return this.accessToken;
            }

            if (string.IsNullOrEmpty(this.identifier) || string.IsNullOrEmpty(this.key))
            {
                return null;
            }

            using (var encoder = new HMACSHA512(Encoding.UTF8.GetBytes(this.key)))
            {
                var dataToSign = this.identifier + "\n" + expiry.ToString("O", CultureInfo.InvariantCulture);
                var hash = encoder.ComputeHash(Encoding.UTF8.GetBytes(dataToSign));
                var signature = Convert.ToBase64String(hash);
                return string.Format("uid={0}&ex={1:o}&sn={2}", this.identifier, expiry, signature);
            }
        }

        public Uri MapPath(string path)
        {
            return new Uri(this.baseAddress, path);
        }

        public Uri MapPath(string pathFormat, params object[] args)
        {
            var path = pathFormat;
            if (args != null && args.Length > 0)
            {
                path = string.Format(pathFormat, args);
            }

            return this.MapPath(path);
        }

        public override string ToString()
        {
            return this.baseAddress.Host;
        }
    }
}
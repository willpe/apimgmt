// --------------------------------------------------------------------------
//  <copyright file="ApiManagementHttpClient.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------

namespace MS.Azure.ApiManagement
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;

    public class ApiManagementHttpClient : HttpClient
    {
        private readonly ApiManagementEndpoint endpoint;

        public ApiManagementHttpClient(ApiManagementEndpoint endpoint)
            : base(new ApiManagementHttpClient.Handler(endpoint))
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException("endpoint");
            }

            this.endpoint = endpoint;

            this.BaseAddress = this.endpoint.BaseAddress;
        }

        public ApiManagementHttpClient(string connectionString)
            : this(ApiManagementEndpoint.FromConnectionString(connectionString))
        {
        }

        public ApiManagementHttpClient()
            : this(ApiManagementEndpoint.Default)
        {
        }

        public ApiManagementEndpoint Endpoint
        {
            get { return this.endpoint; }
        }

        public Task<HttpResponseMessage> ImportAsync(string name, StreamContent body)
        {
            var apiId = Uri.EscapeDataString(name.ToLowerInvariant());
            var uri = this.endpoint.MapPath("/apis/{0}?import=true&path=/{0}", apiId);
            return this.PutAsync(uri, body);
        }

        private class Handler : MessageProcessingHandler
        {
            private readonly ApiManagementEndpoint endpoint;

            private string accessToken;
            private DateTime accessTokenExpires = DateTime.MinValue;

            public Handler(ApiManagementEndpoint endpoint)
                : base(new HttpClientHandler())
            {
                this.endpoint = endpoint;
            }

            protected override HttpRequestMessage ProcessRequest(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                if (this.accessTokenExpires.Subtract(DateTime.UtcNow).TotalMinutes < 1)
                {
                    this.accessTokenExpires = DateTime.UtcNow.AddHours(4);
                    this.accessToken = this.endpoint.CreateAccessToken(this.accessTokenExpires);
                }

                if (this.accessToken != null)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("SharedAccessSignature", this.accessToken);
                }

                if (request.RequestUri.Query.Length == 0 || !request.RequestUri.Query.Contains("api-version="))
                {
                    var builder = new UriBuilder(request.RequestUri)
                    {
                        Query = (request.RequestUri.Query + "&api-version=" + this.endpoint.Version).Trim('?', '&')
                    };

                    request.RequestUri = builder.Uri;
                }

                return request;
            }

            protected override HttpResponseMessage ProcessResponse(HttpResponseMessage response, CancellationToken cancellationToken)
            {
                return response;
            }
        }
    }
}
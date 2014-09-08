// --------------------------------------------------------------------------
//  <copyright file="ApiManagementHttpClient.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------

namespace MS.Azure.ApiManagement
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;

    using MS.Azure.ApiManagement.Utils;

    public class ApiManagementHttpClient : HttpClient, IApiManagementHttpClient
    {
        private readonly IApiManagementEndpoint endpoint;

        public ApiManagementHttpClient(IApiManagementEndpoint endpoint)
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

        public IApiManagementEndpoint Endpoint
        {
            get { return this.endpoint; }
        }

        public async Task<User> GetUserAsync(string id)
        {
            id = User.NormalizeId(id);

            var uri = this.endpoint.MapPath("/users/{0}", id);
            using (var response = await this.GetAsync(uri))
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                response.EnsureSuccessStatusCode();

                var result = await response.Content.DeserializeJsonAsync<User>();
                result.EntityVersion = response.Headers.ETag.ToString();
                return result;
            }
        }

        public async Task<String> GetUserMetadataAsync(string id)
        {
            id = User.NormalizeId(id);

            var uri = this.endpoint.MapPath("/users/{0}", id);
            using (var response = await this.GetAsync(uri))
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                return response.Headers.ETag.ToString();
            }
        }

        public async Task<bool> CreateUserAsync(string id, User properties)
        { 
            id = User.NormalizeId(id);

            if (id.Length > 256)
            {
                throw new ArgumentOutOfRangeException("id", "The maximum length of 'id' is 256 characters");
            }
            
            properties.BeforeCreate(id);

            var uri = this.endpoint.MapPath("/users/{0}", id);
            var content = new JsonSerializedContent(properties);
            using (var response = await this.PutAsync(uri, content))
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }

                response.EnsureSuccessStatusCode();

                var result = await response.Content.DeserializeJsonAsync<User>();
                result.EntityVersion = response.Headers.ETag.ToString();
                return true;
            }
        }

        public async Task<IReadOnlyCollection<User>> GetUsersAsync()
        {
            var uri = this.endpoint.MapPath("/users");
            using (var response = await this.GetAsync(uri))
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.DeserializeJsonAsync<JsonCollection<User>>();
            }
        }

        public Task<HttpResponseMessage> ImportAsync(string name, StreamContent body)
        {
            var apiId = Uri.EscapeDataString(name.ToLowerInvariant());
            var uri = this.endpoint.MapPath("/apis/{0}?import=true&path=/{0}", apiId);
            return this.PutAsync(uri, body);
        }

        private class Handler : MessageProcessingHandler
        {
            private readonly IApiManagementEndpoint endpoint;

            private string accessToken;
            private DateTime accessTokenExpires = DateTime.MinValue;

            public Handler(IApiManagementEndpoint endpoint)
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
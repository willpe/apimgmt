// --------------------------------------------------------------------------
//  <copyright file="ApiManagementHttpClientBase.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------

namespace MS.Azure.ApiManagement
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;

    public abstract class ApiManagementHttpClientBase : HttpClient, IApiManagementHttpClient
    {
        private static readonly HttpMethod Patch = new HttpMethod("PATCH");
        private readonly IApiManagementEndpoint endpoint;

        protected ApiManagementHttpClientBase(IApiManagementEndpoint endpoint)
            : base(new ApiManagementHttpClientBase.Handler(endpoint))
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException("endpoint");
            }

            this.endpoint = endpoint;

            this.BaseAddress = this.endpoint.BaseAddress;
        }

        protected ApiManagementHttpClientBase(string connectionString)
            : this(ApiManagementEndpoint.FromConnectionString(connectionString))
        {
        }

        protected ApiManagementHttpClientBase()
            : this(ApiManagementEndpoint.Default)
        {
        }

        public IApiManagementEndpoint Endpoint
        {
            get { return this.endpoint; }
        }

        public abstract Task<bool> CreateUserAsync(User user);

        public abstract Task<bool> DeleteUserAsync(string id, string entityVersion);

        public async Task<bool> DeleteUserAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return await this.DeleteUserAsync(user.Id, user.EntityVersion);
        }

        public abstract Task<User> GetUserAsync(string id);

        public abstract Task<string> GetUserMetadataAsync(string id);

        public abstract Task<IReadOnlyCollection<User>> GetUsersAsync();

        public async Task<HttpResponseMessage> HeadAsync(Uri requestUri)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Head, requestUri))
            {
                return await this.SendAsync(request);
            }
        }

        public async Task<HttpResponseMessage> HeadAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Head, requestUri))
            {
                return await this.SendAsync(request, cancellationToken);
            }
        }

        public async Task<HttpResponseMessage> HeadAsync(string requestUri)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Head, requestUri))
            {
                return await this.SendAsync(request);
            }
        }

        public async Task<HttpResponseMessage> HeadAsync(string requestUri, CancellationToken cancellationToken)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Head, requestUri))
            {
                return await this.SendAsync(request, cancellationToken);
            }
        }

        public abstract Task<HttpResponseMessage> ImportAsync(string name, StreamContent body);

        public async Task<HttpResponseMessage> PatchAsync(Uri requestUri, HttpContent content)
        {
            using (var request = new HttpRequestMessage(Patch, requestUri)
            {
                Content = content
            })
            {
                return await this.SendAsync(request);
            }
        }

        public async Task<HttpResponseMessage> PatchAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            using (var request = new HttpRequestMessage(Patch, requestUri)
            {
                Content = content
            })
            {
                return await this.SendAsync(request, cancellationToken);
            }
        }

        public async Task<HttpResponseMessage> PatchAsync(string requestUri, HttpContent content)
        {
            using (var request = new HttpRequestMessage(Patch, requestUri)
            {
                Content = content
            })
            {
                return await this.SendAsync(request);
            }
        }

        public async Task<HttpResponseMessage> PatchAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            using (var request = new HttpRequestMessage(Patch, requestUri)
            {
                Content = content
            })
            {
                return await this.SendAsync(request, cancellationToken);
            }
        }

        public abstract Task<bool> UpdateUserAsync(User user);

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
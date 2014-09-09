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
    using System.Threading.Tasks;

    using MS.Azure.ApiManagement.Utils;

    public class ApiManagementHttpClient : ApiManagementHttpClientBase
    {
        public ApiManagementHttpClient(IApiManagementEndpoint endpoint)
            : base(endpoint)
        {
        }

        public ApiManagementHttpClient(string connectionString)
            : base(connectionString)
        {
        }

        public ApiManagementHttpClient()
            : base()
        {
        }

        #region Users

        public override async Task<bool> CreateUserAsync(User user)
        {
            var id = User.NormalizeId(user.Id);

            var uri = this.Endpoint.MapPath("/users/{0}", id);
            var content = new JsonSerializedContent(user);
            using (var response = await this.PutAsync(uri, content))
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }

                if (!response.IsSuccessStatusCode)
                {
                    throw await ApiManagementHttpRequestException.Create(response);
                }

                var result = await response.Content.DeserializeJsonAsync<User>();
                result.EntityVersion = response.Headers.ETag.ToString();
                return true;
            }
        }

        public override async Task<bool> DeleteUserAsync(string id, string entityVersion)
        {
            id = User.NormalizeId(id);

            var uri = this.Endpoint.MapPath("/users/{0}", id);
            using (var message = new HttpRequestMessage(HttpMethod.Delete, uri))
            {
                message.Headers.IfMatch.ParseAdd(entityVersion);

                using (var response = await this.SendAsync(message))
                {
                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        return true;
                    }

                    if (!response.IsSuccessStatusCode)
                    {
                        throw await ApiManagementHttpRequestException.Create(response);
                    }

                    return false;
                }
            }
        }

        public override async Task<User> GetUserAsync(string id)
        {
            id = User.NormalizeId(id);

            var uri = this.Endpoint.MapPath("/users/{0}", id);
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

        public override async Task<String> GetUserMetadataAsync(string id)
        {
            id = User.NormalizeId(id);

            var uri = this.Endpoint.MapPath("/users/{0}", id);
            using (var response = await this.HeadAsync(uri))
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                return response.Headers.ETag.ToString();
            }
        }

        public override async Task<IReadOnlyCollection<User>> GetUsersAsync()
        {
            var uri = this.Endpoint.MapPath("/users");
            using (var response = await this.GetAsync(uri))
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.DeserializeJsonAsync<JsonCollection<User>>();
            }
        }

        public override async Task<bool> UpdateUserAsync(User user)
        {
            var id = User.NormalizeId(user.Id);

            var uri = this.Endpoint.MapPath("/users/{0}", id);
            var content = new JsonSerializedContent(user) { Headers = { { "If-Match", user.EntityVersion } } };
            using (var response = await this.PatchAsync(uri, content))
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }

                if (!response.IsSuccessStatusCode)
                {
                    throw await ApiManagementHttpRequestException.Create(response);
                }

                return true;
            }
        } 

        #endregion

        public override Task<HttpResponseMessage> ImportAsync(string name, StreamContent body)
        {
            var apiId = Uri.EscapeDataString(name.ToLowerInvariant());
            var uri = this.Endpoint.MapPath("/apis/{0}?import=true&path=/{0}", apiId);
            return this.PutAsync(uri, body);
        }
    }
}
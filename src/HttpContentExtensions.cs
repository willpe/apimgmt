// --------------------------------------------------------------------------
//  <copyright file="HttpContentExtensions.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------

namespace Microsoft.Azure.ApiManagement
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public static class HttpContentExtensions
    {
        public static async Task<T> DeserializeJsonAsync<T>(this HttpContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            using (var responseStream = await content.ReadAsStreamAsync())
            {
                using (var streamReader = new StreamReader(responseStream))
                {
                    using (var jsonReader = new JsonTextReader(streamReader))
                    {
                        var serializer = new JsonSerializer();
                        return serializer.Deserialize<T>(jsonReader);
                    }
                }
            }
        }

        public static async Task<JArray> ReadAsJArrayAsync(this HttpContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            using (var responseStream = await content.ReadAsStreamAsync())
            {
                using (var streamReader = new StreamReader(responseStream))
                {
                    using (var jsonReader = new JsonTextReader(streamReader))
                    {
                        return JArray.Load(jsonReader);
                    }
                }
            }
        }

        public static async Task<JObject> ReadAsJObjectAsync(this HttpContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            using (var responseStream = await content.ReadAsStreamAsync())
            {
                using (var streamReader = new StreamReader(responseStream))
                {
                    using (var jsonReader = new JsonTextReader(streamReader))
                    {
                        return JObject.Load(jsonReader);
                    }
                }
            }
        }

        public static async Task<JToken> ReadAsJTokenAsync(this HttpContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            using (var responseStream = await content.ReadAsStreamAsync())
            {
                using (var streamReader = new StreamReader(responseStream))
                {
                    using (var jsonReader = new JsonTextReader(streamReader))
                    {
                        return JToken.Load(jsonReader);
                    }
                }
            }
        }
    }
}
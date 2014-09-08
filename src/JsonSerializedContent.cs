// ------------------------------------------------------------------------------------------------
// <copyright file="JsonSerializedContent.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------------------------
namespace Microsoft.Azure.ApiManagement
{
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    public sealed class JsonSerializedContent : HttpContent
    {
        private static readonly MediaTypeHeaderValue ContentType = MediaTypeHeaderValue.Parse("application/json");
        private readonly object body;

        public JsonSerializedContent(object body)
        {
            this.body = body;
            this.Headers.ContentType = JsonSerializedContent.ContentType;
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            await Task.Run(() =>
            {
                using (var textWriter = new StreamWriter(stream))
                {
                    using (var jsonWriter = new JsonTextWriter(textWriter))
                    {
                        var serializer = new JsonSerializer();
                        serializer.Serialize(jsonWriter, this.body);
                        jsonWriter.Flush();
                    }
                }
            });
        }

        protected override bool TryComputeLength(out long length)
        {
            using (var stream = new MemoryStream())
            {
                using (var textWriter = new StreamWriter(stream))
                {
                    using (var jsonWriter = new JsonTextWriter(textWriter))
                    {
                        var serializer = new JsonSerializer();
                        serializer.Serialize(jsonWriter, this.body);
                        jsonWriter.Flush();

                        length = stream.Length;
                    }
                }
            }

            return true;
        }
    }
}
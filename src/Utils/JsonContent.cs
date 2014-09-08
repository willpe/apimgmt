// --------------------------------------------------------------------------
//  <copyright file="JsonContent.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------

namespace MS.Azure.ApiManagement.Utils
{
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public sealed class JsonContent : HttpContent
    {
        private static readonly MediaTypeHeaderValue ContentType = MediaTypeHeaderValue.Parse("application/json");
        private readonly JToken body;

        public JsonContent(JToken body)
        {
            this.body = body;
            this.Headers.ContentType = JsonContent.ContentType;
#if DEBUG
            this.Formatting = Formatting.Indented;
#else
            this.Formatting = Formatting.None;
#endif
        }

        public Formatting Formatting { get; set; }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            await Task.Run(() =>
            {
                using (var textWriter = new StreamWriter(stream))
                {
                    using (var jsonWriter = new JsonTextWriter(textWriter)
                    {
                        Formatting = this.Formatting
                    })
                    {
                        this.body.WriteTo(jsonWriter);

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
                    using (var jsonWriter = new JsonTextWriter(textWriter)
                    {
                        Formatting = this.Formatting
                    })
                    {
                        this.body.WriteTo(jsonWriter);
                        jsonWriter.Flush();

                        length = stream.Length;
                    }
                }
            }

            return true;
        }
    }
}
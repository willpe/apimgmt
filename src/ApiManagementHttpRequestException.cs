// --------------------------------------------------------------------------
//  <copyright file="ApiManagementHttpRequestException.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------

namespace MS.Azure.ApiManagement
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    using MS.Azure.ApiManagement.Utils;

    public class ApiManagementHttpRequestException : HttpRequestException
    {
        private readonly Error error;
        private readonly HttpResponseMessage response;

        private ApiManagementHttpRequestException(string message, HttpResponseMessage response, Error error = null)
            : base(message)
        {
            this.response = response;
            this.error = error;
        }

        public Error Error
        {
            get { return this.error; }
        }

        public HttpResponseMessage Response
        {
            get { return this.response; }
        }

        public static async Task<ApiManagementHttpRequestException> Create(HttpResponseMessage response)
        {
            if (response == null)
            {
                throw new ArgumentNullException("response");
            }

            var message = string.Format("HTTP-{0}: {1} ({2})", (int)response.StatusCode, response.StatusCode, response.ReasonPhrase);
            if (response.Content != null)
            {
                try
                {
                    var error = await response.Content.DeserializeJsonAsync<Error>();

                    message = string.Format("{0}: {1}", error.Body.Code, error.Body.Message);
                    return new ApiManagementHttpRequestException(message, response, error);
                }
                 // ReSharper disable once EmptyGeneralCatchClause
                catch { }
            }

            return new ApiManagementHttpRequestException(message, response);
        }
    }
}
// --------------------------------------------------------------------------
//  <copyright file="ErrorBody.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------

namespace MS.Azure.ApiManagement
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class ErrorBody
    {
        /// <summary>
        /// Service-defined error code. This code serves as a sub-status for the HTTP error code specified in the response.
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; private set; }

        /// <summary>
        /// In case of validation errors this field contains a list of invalid fields sent in the request.
        /// </summary>
        [JsonProperty("details")]
        public IReadOnlyCollection<ErrorDetail> Details { get; private set; }

        /// <summary>
        /// Description of the error.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; private set; }
    }
}
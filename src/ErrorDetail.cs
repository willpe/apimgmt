// --------------------------------------------------------------------------
//  <copyright file="ErrorDetail.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------

namespace MS.Azure.ApiManagement
{
    using Newtonsoft.Json;

    public class ErrorDetail
    {
        /// <summary>
        /// Property level error code.
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; private set; }

        /// <summary>
        /// Human readable representation of the property-level error.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; private set; }

        /// <summary>
        /// Optional. Property name.
        /// </summary>
        [JsonProperty("target")]
        public string Target { get; private set; }
    }
}
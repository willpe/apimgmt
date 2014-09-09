// --------------------------------------------------------------------------
//  <copyright file="Error.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------

namespace MS.Azure.ApiManagement
{
    using Newtonsoft.Json;

    public sealed class Error
    {
        /// <summary>
        /// The error body containing the details of the error.
        /// </summary>
        [JsonProperty("error")]
        public ErrorBody Body { get; private set; }
    }
}
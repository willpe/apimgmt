// --------------------------------------------------------------------------
//  <copyright file="Api.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------

namespace MS.Azure.ApiManagement
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    public class Api
    {
        /// <summary>
        /// Collection of operations included in this API.
        /// </summary>
        [JsonProperty("operations")]
        private readonly JsonCollection<Operation> operations = new JsonCollection<Operation>();

        /// <summary>
        /// Description of the API. Must not be empty. May include HTML formatting tags. Maximum length is 1000 characters.
        /// </summary>
        [JsonProperty("description"), Required, MaxLength(1000)]
        public string Description { get; set; }

        /// <summary>
        /// Resource identifier. Uniquely identifies the API within the current API Management service instance. The value is a valid relative URL in the format of apis/{id} where {id} is an API identifier. This property is read-only.
        /// </summary>
        [JsonProperty("id"), ReadOnly(true)]
        public string Id { get; private set; }

        /// <summary>
        /// Name of the API. Must not be empty. Maximum length is 100 characters.
        /// </summary>
        [JsonProperty("name"), Required, MaxLength(100)]
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<Operation> Operations
        {
            get { return this.operations; }
        }

        /// <summary>
        /// Relative URL uniquely identifying this API and all of its resource paths within the API Management service instance. It is appended to the API endpoint base URL specified during the service instance creation to form a public URL for this API. 
        /// </summary>
        [JsonProperty("path")]
        public string Path { get; set; }

        /// <summary>
        /// Absolute URL of the backend service implementing this API.
        /// </summary>
        [JsonProperty("serviceUrl")]
        public string ServiceUrl { get; set; }
    }
}
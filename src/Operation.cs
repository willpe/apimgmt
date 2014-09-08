// --------------------------------------------------------------------------
//  <copyright file="Operation.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------

namespace MS.Azure.ApiManagement
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class Operation
    {
        [JsonExtensionData]
        private readonly IDictionary<string, JToken> properties = new Dictionary<string, JToken>();

        /// <summary>
        /// Description of the Operation. Must not be empty. May include HTML formatting tags. Maximum length is 1000 characters.
        /// </summary>
        [JsonProperty("description"), Required, MaxLength(1000)]
        public string Description { get; set; }

        /// <summary>
        /// Resource identifier. Uniquely identifies the Operation within the current API Management service instance. The value is a valid relative URL in the format of apis/{id} where {id} is an API identifier. This property is read-only.
        /// </summary>
        [JsonProperty("id"), ReadOnly(true)]
        public string Id { get; private set; }

        /// <summary>
        /// Name of the Operation. Must not be empty. Maximum length is 100 characters.
        /// </summary>
        [JsonProperty("name"), Required, MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Operation HTTP method.
        /// </summary>
        [JsonProperty("method")]
        public string Method { get; set; }

        /// <summary>
        /// Relative URL template identifying the target resource for this operation. May include parameters. Example: customers/{cid}/orders/{oid}/?date={date}
        /// </summary>
        [JsonProperty("urlTemplate")]
        public string UrlTemplate { get; set; }
    }
}
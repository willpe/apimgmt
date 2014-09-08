// --------------------------------------------------------------------------
//  <copyright file="Product.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------

namespace MS.Azure.ApiManagement
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    public class Product
    {
        private const string NotPublishedState = "notPublished";
        private const string PublishedState = "published";

        /// <summary> 
        /// Specifies whether the product is published or not. Published products are discoverable by developers on the developer portal. Non-published products are visible only to administrators.
        /// The allowable values for product state are published and notPublished.
        /// </summary>
        [JsonProperty("state")]
        private string state;

        /// <summary>
        /// Specifies whether subscription approval is required. If false, new subscriptions will be approved automatically enabling developers to call the product’s APIs immediately after subscribing. If true, administrators must manually approve the subscription before the developer can any of the product’s APIs.
        /// </summary>
        [JsonProperty("approvalRequired")]
        public bool ApprovalRequired { get; set; }

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

        [JsonIgnore]
        public bool IsPublished
        {
            get { return this.state == PublishedState; }
            set { this.state = value ? PublishedState : NotPublishedState; }
        }

        /// <summary>
        /// Name of the Operation. Must not be empty. Maximum length is 100 characters.
        /// </summary>
        [JsonProperty("name"), Required, MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Product terms of use. Developers trying to subscribe to the product will be presented and required to accept these terms before they can complete the subscription process.
        /// </summary>
        [JsonProperty("terms")]
        public string Terms { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1})", this.Name, this.Id);
        }
    }
}
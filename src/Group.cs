// --------------------------------------------------------------------------
//  <copyright file="Group.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------

namespace MS.Azure.ApiManagement
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    public class Group
    {
        /// <summary>
        /// Description of the group. Can contain HTML formatting tags. Maximum length is 1000 characters.
        /// </summary>
        [JsonProperty("description"), MaxLength(1000)]
        public string Description { get; set; }

        /// <summary>
        /// Resource identifier. Uniquely identifies the group within the current API Management service instance. 
        /// </summary>
        [JsonProperty("id"), ReadOnly(true)]
        public string Id { get; private set; }

        /// <summary>
        /// true if the group is one of the three built in groups (Administrators, Developers, or Guests); otherwise false. This property is read-only.
        /// </summary>
        [JsonProperty("builtIn"), ReadOnly(true)]
        public bool IsBuiltIn { get; private set; }

        /// <summary>
        /// Name of the group. Maximum length is 100 characters.
        /// </summary>
        [JsonProperty("name"), MaxLength(100)]
        public string Name { get; set; }
    }
}
// --------------------------------------------------------------------------
//  <copyright file="User.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------

namespace MS.Azure.ApiManagement
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Text.RegularExpressions;

    using Newtonsoft.Json;

    public class User : IVersionedEntity
    {
        private const string ActiveState = "active";
        private const string BlockedState = "blocked";

        private static readonly Regex ResourceIdentifierExpression = new Regex("^/?users/(?<uid>.+)/?$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        private string __id;
        private DateTime __registrationDate;

        public User(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id");
            }
            
            if (id.Length > 256)
            {
                throw new ArgumentOutOfRangeException("id", "The maximum length of 'id' is 256 characters");
            }

            this.Id = id;
        }

        /// <summary> 
        /// Specifies whether the user is active or not. Blocked users are unable to sign into the developer portal or call any APIs of subscribed products.
        /// The allowable values for user state are active and blocked.
        /// </summary>
        [JsonProperty("state", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        private string state = ActiveState;

        /// <summary> 
        /// Email address. Must not be empty and must be unique within the service instance. Maximum length is 254 characters.
        /// </summary>
        [JsonProperty("email", Required = Required.Always), Required, MaxLength(254)]
        public string Email { get; set; }

        [JsonIgnore]
        public string EntityVersion { get; internal set; }

        /// <summary> 
        /// First name. Must not be empty. Maximum length is 100 characters.
        /// </summary>
        [JsonProperty("firstName", Required = Required.Always), Required, MaxLength(100)]
        public string FirstName { get; set; }

        /// <summary> 
        /// Resource identifier. Uniquely identifies the user within the current API Management service instance. The value is a valid relative URL in the format of users/{uid} where {uid} is a user identifier. This property is read-only.
        /// </summary>
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore), ReadOnly(true)]
        public string Id { get; private set; }

        [JsonIgnore]
        public bool IsBlocked
        {
            get { return this.state == BlockedState; }
            set { this.state = value ? BlockedState : ActiveState; }
        }

        /// <summary> 
        /// Last name. Must not be empty. Maximum length is 100 characters.
        /// </summary>
        [JsonProperty("lastName", Required = Required.Always), Required, MaxLength(100)]
        public string LastName { get; set; }

        /// <summary> 
        /// Optional note about a user set by the administrator.
        /// </summary>
        [JsonProperty("note", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string Note { get; set; }

        /// <summary> 
        /// Password for the user. This property is read-only.
        /// </summary>
        [JsonProperty("password", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; }

        /// <summary> 
        /// User registration date, in ISO 8601 format: 2014-06-24T16:25:00Z
        /// </summary>
        [JsonProperty("registrationDate", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime RegistrationDate { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} ({2})", this.FirstName, this.LastName, this.Email);
        }

        internal static string NormalizeId(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id");
            }

            // If the full resource identifier (e.g. users/1) is specified, extract just the id
            var match = ResourceIdentifierExpression.Match(id);
            if (match.Success)
            {
                return match.Groups["uid"].Value;
            }

            return id;
        }

        [OnSerialized]
        internal void OnSerialized(StreamingContext context)
        {
            this.Id = this.__id;
            this.RegistrationDate = this.__registrationDate;
        }

        [OnSerializing]
        internal void OnSerializing(StreamingContext context)
        {          
            this.__id = this.Id;
            this.__registrationDate = this.RegistrationDate;

            this.Id = null;
            this.RegistrationDate = default(DateTime);
        }
    }
}
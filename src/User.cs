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
    using System.Text.RegularExpressions;

    using Newtonsoft.Json;

    public class User : IVersionedEntity
    {
        private const string ActiveState = "active";
        private const string BlockedState = "blocked";

        private static readonly Regex ResourceIdentifierExpression = new Regex("^/?users/(?<uid>.+)/?$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        /// <summary> 
        /// Specifies whether the user is active or not. Blocked users are unable to sign into the developer portal or call any APIs of subscribed products.
        /// The allowable values for user state are active and blocked.
        /// </summary>
        [JsonProperty("state")]
        private string state;

        /// <summary> 
        /// Email address. Must not be empty and must be unique within the service instance. Maximum length is 254 characters.
        /// </summary>
        [JsonProperty("email"), MaxLength(254)]
        public string Email { get; set; }

        /// <summary> 
        /// First name. Must not be empty. Maximum length is 100 characters.
        /// </summary>
        [JsonProperty("firstName"), MaxLength(100)]
        public string FirstName { get; set; }

        /// <summary> 
        /// Resource identifier. Uniquely identifies the user within the current API Management service instance. The value is a valid relative URL in the format of users/{uid} where {uid} is a user identifier. This property is read-only.
        /// </summary>
        [JsonProperty("id"), ReadOnly(true)]
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
        [JsonProperty("lastName"), MaxLength(100)]
        public string LastName { get; set; }

        /// <summary> 
        /// Optional note about a user set by the administrator.
        /// </summary>
        [JsonProperty("note")]
        public string Note { get; set; }

        /// <summary> 
        /// User registration date, in ISO 8601 format: 2014-06-24T16:25:00Z
        /// </summary>
        [JsonProperty("registrationDate")]
        public DateTime RegistrationDate { get; set; }


        /// <summary> 
        /// Password for the user. This property is read-only.
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }

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

        [JsonIgnore]
        public string EntityVersion { get; internal set; }

        internal void BeforeCreate(string id)
        {
            this.Id = id;

            if (this.RegistrationDate == default(DateTime))
            {
                this.RegistrationDate = DateTime.UtcNow;
            }
        }
    }
}
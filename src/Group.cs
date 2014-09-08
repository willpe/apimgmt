using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Azure.ApiManagement
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    public class Group
    {
        /// <summary>
        /// Resource identifier. Uniquely identifies the group within the current API Management service instance. 
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; private set; }

        /// <summary>
        /// Name of the group. Maximum length is 100 characters.
        /// </summary>
        [JsonProperty("name"), MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Description of the group. Can contain HTML formatting tags. Maximum length is 1000 characters.
        /// </summary>
        [JsonProperty("description"), MaxLength(1000)]
        public string Description { get; set; }

        /// <summary>
        /// true if the group is one of the three built in groups (Administrators, Developers, or Guests); otherwise false. This property is read-only.
        /// </summary>
        [JsonProperty("builtIn")]
        public bool IsBuiltIn { get; private set; }
    }
}

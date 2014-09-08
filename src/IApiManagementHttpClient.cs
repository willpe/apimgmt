// --------------------------------------------------------------------------
//  <copyright file="IApiManagementHttpClient.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------

namespace MS.Azure.ApiManagement
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IApiManagementHttpClient
    {
        IApiManagementEndpoint Endpoint { get; }

        /// <summary>
        /// Gets the details of the user specified by its identifier.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns>The specified User entity</returns>
        Task<User> GetUserAsync(string id);

        /// <summary>
        /// Gets a collection of registered users in the specified service instance.
        /// </summary>
        /// <returns></returns>
        Task<IReadOnlyCollection<User>> GetUsersAsync();

        Task<HttpResponseMessage> ImportAsync(string name, StreamContent body);

        /// <summary>
        /// Gets the metadata (Entity Version) for the user specified by its identifier.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns>Current entity state version. Should be treated as opaque and used to make conditional HTTP requests.</returns>
        Task<String> GetUserMetadataAsync(string id);

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="id">User identifier. Must be unique in the current API Management service instance. Maximum length is 256 characters.</param>
        /// <param name="properties">An object determining the properties for the new user to create.</param>
        /// <returns>True if the user was successfully created, or false if a user with the same identifier already exists</returns>
        Task<bool> CreateUserAsync(string id, User properties);
    }
}
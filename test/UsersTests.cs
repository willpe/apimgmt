// --------------------------------------------------------------------------
//  <copyright file="UsersTests.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------

namespace MS.Azure.ApiManagement.Tests
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UsersTests
    {
        private static IApiManagementHttpClient CreateClient()
        {
            return new ApiManagementHttpClient(Config.ConnectionString);
        }

        [TestMethod]
        public async Task ListUsers()
        {
            var client = CreateClient();
            var users = await client.GetUsersAsync();

            Assert.IsNotNull(users);
            Assert.IsTrue(users.Count > 0);

            foreach (var user in users)
            {
                Assert.IsNotNull(user.Id, "Id should not be null");
            }
        }

        [TestMethod]
        public async Task GetUser()
        {
            var client = CreateClient();
            var users = await client.GetUsersAsync();

            Assert.IsNotNull(users);
            Assert.IsTrue(users.Count > 0);

            var userToGet = users.First();
            var user = await client.GetUserAsync(userToGet.Id);

            Assert.AreEqual(user.Id, userToGet.Id);
            Assert.IsNotNull(user.EntityVersion);

            var version = await client.GetUserMetadataAsync(userToGet.Id);
            Assert.AreEqual(user.EntityVersion, version);

            var nonUser = await client.GetUserAsync("33");
            Assert.IsNull(nonUser);
        }

        [TestMethod]
        public async Task CreateUser()
        {
            var client = CreateClient();
            var newUser = new User("test1")
            {
                FirstName = "Testy",
                LastName = "McTesterson",
                Email = "no-reply@noreply.org",
                Password = "P@ssw0rd1"

            };
            
            var created = await client.CreateUserAsync(newUser);
            Assert.IsTrue(created);

            var version = await client.GetUserMetadataAsync("test1");
            Assert.IsNotNull(version);

            var nonUser = await client.GetUserAsync("test1");
            Assert.IsNotNull(nonUser);
        }

        [TestMethod]
        public async Task UpdateUser()
        {
            var client = CreateClient();
            var user = new User("test3")
            {
                FirstName = "Testy",
                LastName = "McTesterson",
                Email = "no-reply2@noreply.org",
                Password = "P@ssw0rd1"

            };

            var created = await client.CreateUserAsync(user);
            Assert.IsTrue(created);

            user.FirstName = "Testita";
            user.LastName = " de Testilio";
            user.Note = "Test";

            var success = await client.UpdateUserAsync(user);
            Assert.IsTrue(success);
        }

        [TestMethod]
        public async Task DeleteUser()
        {
            var client = CreateClient();
            var etag = await client.GetUserMetadataAsync("test1");
            var success = await client.DeleteUserAsync("test1", etag);
            Assert.IsTrue(success);

            etag = await client.GetUserMetadataAsync("test2");
            success = await client.DeleteUserAsync("test2", etag);
            Assert.IsTrue(success);
        }
    }
}
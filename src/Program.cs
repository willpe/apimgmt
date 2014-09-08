namespace Microsoft.Azure.ApiManagement
{
    using System.Collections.Generic;

    class Program
    {
        static void Main(string[] args)
        {
            using (var client = new ApiManagementHttpClient(ApiManagementEndpoint.Default))
            {
                using (var response = client.GetAsync("/groups").Result)
                {
                    response.EnsureSuccessStatusCode();

                    var content = response.Content.DeserializeJsonAsync<IReadOnlyCollection<Group>>().Result;
                }
            }
        }
    }
}

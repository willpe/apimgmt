Azure Api Management Client Library
===================================

To use the [API Managment REST API](http://go.microsoft.com/fwlink/?LinkId=507408), you must ensure it is enabled. To do this, check the relevant box on the System Settings page within your management console (for example, https://contoso.azure-api.net/admin/tenant/access)

## Connection Strings

To simplify configuration, a connection string is used to specify the service name (or url) and any credentials. The connection string is a set of semi-colon delimited key/value pairs and typically looks like this:

	serviceName=contoso;identifier=1234;key=abc123

The following properties are supported:

  - serviceName: The name of your API Management service (for example, if your management URI is https://contoso.management.azure-api.net, your service name is `contoso`)
  - url: If preferred, you may specify the full management URI instead of just the service name. For example, https://contoso.management.azure-api.net.
  - identifier: Your identifier from the System Settings page (like: https://contoso.azure-api.net/admin/tenant/access)
  - key: Either the primary or secondary key that matches the identifier specified.
  - accessToken: If preferred, you may specify an access token rather than passing an identifier and key. Obviously, calls using the library will fail when the token expires
  - version: Specify the version of the API Management api to use. It is not recommended that you modify this parameter. The current default value is `2014-02-14-preview`;

To connect successfully, your connection string must contain:

  - The address of the API Management service:
	- **Either** the serviceName 
    - **Or** url; 
  - **And** a set of credentials:
    - **Either** an identifier and key
    - **Or** an accessToken.


### Default Connection String

In many scenarios, a single connection string will be sufficient. in such cases, the connection string should be specified in `App.config` or `Web.config`:

````
  <connectionStrings>
    <add name="ApiManagement" connectionString="serviceName=XXXX;identifier=XXXX;key=XXXX"/>
  </connectionStrings>
````

When used in this fashion, the connection string will be used to initialized `ApiManagementEndpoint.Default` and with the default, parameterless constructor `new ApiManagementHttpClient()`

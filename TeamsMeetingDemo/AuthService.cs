using Microsoft.Identity.Client;

public class AuthService
{
    private string clientId = "45c8e485-2330-49e5-a1ce-b296d406d634";
    private string clientSecret = "i_88Q~BK8oW9xgoXMX20oeBykoBTKilwjbauNcf6";
    private string tenantId = "2ad72c11-ca14-4743-95d6-fbbe0f001056"; // Commonly looks like a GUID or 'organizations'

    private string[] scopes = new string[] { "https://graph.microsoft.com/.default" };

    public async Task<string> GetToken()
    {
        // Create the MSAL ConfidentialClientApplication
        IConfidentialClientApplication app = ConfidentialClientApplicationBuilder
            .Create(clientId)
            .WithClientSecret(clientSecret)
            .WithAuthority($"https://login.microsoftonline.com/{tenantId}")
            .Build();

        var result = await app.AcquireTokenForClient(scopes).ExecuteAsync();

        // Access token will be available in the "AccessToken" property of the result
        string accessToken = result.AccessToken;
        Console.WriteLine("Access token: " + accessToken);
        return accessToken;
    }



}
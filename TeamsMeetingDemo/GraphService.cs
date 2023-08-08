using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ODataErrors;

public class GraphService
{

    private GraphServiceClient CreateGraphClient()
    {
        var MyConfig = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", false)
        .Build();

        // Values from app registration
        var clientId = MyConfig.GetValue<string>("AzureAd:ClientId");
        var clientSecret = MyConfig.GetValue<string>("AzureAd:ClientSecret");

        //var scopes = new[] { "https://graph.microsoft.com/.default" };
        var scopes = new[] { ".default" };

        //// Multi-tenant apps can use "common",
        //// single-tenant apps must use the tenant ID from the Azure portal
        var tenantId = MyConfig.GetValue<string>("AzureAd:TenantId");

        //// https://learn.microsoft.com/dotnet/api/azure.identity.clientsecretcredential
        var clientSecretCredential = new ClientSecretCredential(
            tenantId, clientId, clientSecret);

        var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

        return graphClient;
    }

    public async Task createMeeting()
    {
        var client = CreateGraphClient();
        // var user = await client.Users["mariofrancis@mariofc.onmicrosoft.com"].GetAsync();
        // Console.WriteLine(user?.DisplayName);
        var requestBody = new OnlineMeeting
        {
            StartDateTime = DateTimeOffset.Now,
            EndDateTime = DateTimeOffset.Now.AddHours(1),
            Subject = "Test Meeting",
            Participants = new MeetingParticipants
            {
                Organizer = new MeetingParticipantInfo
                {
                    Identity = new IdentitySet
                    {
                        User = new Identity
                        {
                            // Id = "mariofrancis@mariofc.onmicrosoft.com",
                            Id = "f1e0840d-c507-4bf1-a31d-0c56c75e7e23",
                            // DisplayName = "Mario Francis"
                        }
                    }
                },
                Attendees = new List<MeetingParticipantInfo>{
                    new MeetingParticipantInfo{
                        Identity=new IdentitySet{
                            User=new Identity{
                                // Id="zubby@mariofc.onmicrosoft.com",
                                Id="7c8d596c-d2b1-4be6-8ecf-711e48dd1c23",
                                // DisplayName="Zubby Dev"
                            }
                        }
                    },
                    new MeetingParticipantInfo{
                        Identity=new IdentitySet{
                            User=new Identity{
                                // Id="zubby@mariofc.onmicrosoft.com",
                                Id="23b82327-eb21-4163-aaaa-c701cbed28a4",
                                // DisplayName="Zubby Dev"
                            }
                        }
                    }
                }
            }
        };
        try
        {
            var result = await client.Users["f1e0840d-c507-4bf1-a31d-0c56c75e7e23"].OnlineMeetings.PostAsync(requestBody);
            Console.WriteLine(result?.JoinWebUrl);
        }
        catch (ODataError ex)
        {
            Console.WriteLine("Error: " + ex.Error?.Message);
            Console.WriteLine("Error: " + ex.Message);
        }
        Console.WriteLine("Complete");
    }
}
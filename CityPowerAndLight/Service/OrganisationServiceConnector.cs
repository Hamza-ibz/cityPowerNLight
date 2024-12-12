using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;

namespace CityPowerAndLight.Service;

internal static class OrganisationServiceConnector
{
    public static IOrganizationService Connect(string serviceUrl, string appId, string clientSecret)
    {
        string connectionString = $@"AuthType=ClientSecret;
                        SkipDiscovery=true;url={serviceUrl};
                        Secret={clientSecret};
                        ClientId={appId};
                        RequireNewInstance=true";

        IOrganizationService service = new ServiceClient(connectionString);
        return service;
    }
}
namespace ApiDemo.Authority;

// In-memory DB for Authority
public static class AppRepository
{
    private static List<Application> _applications = new List<Application>()
    {
        new Application
        {
            ApplicationId = 1,
            ApplicationName = "MVCWeb",
            ClientId = "53DC1E6-4587-4AD5-8C6E-A8E48D59940E",
            Secret = "0673FC70-0514-4011-B4A3-DF9BC03201BC",
            Scopes = "read,write,delete" // "read,write"
        }
    };

    public static bool Authenticate(string clientId, string secret)
    {
        return _applications.Any(x => x.ClientId == clientId && x.Secret == secret);
    }

    public static Application? GetApplicationByClientId(string clientId)
    {
        return _applications.FirstOrDefault(x => x.ClientId == clientId);
    }

}
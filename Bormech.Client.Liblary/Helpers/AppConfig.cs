

namespace Bormech.Client.Liblary.Helpers;

public class AppConfig
{
    public List<Hub> Hubs { get; set; }
    public List<Hub> FindHubs(string name)
    {
        return Hubs.FindAll(x => x.Target == name);
    }
}

public class Hub
{
    public string Target { get; set; }
    public string? Name { get; set; }
    public string Uri { get; set; }
}
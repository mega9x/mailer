using System.Text.Json;
using OutlookHacker.Models.ConstStr;

namespace OutlookHacker.ProxyManager;
public class ProxyProvider
{
    public string ProxyPlainText { get; set; } = "";
    public List<string> ProxyList { get; set; } = new();
    public bool IsIniting { get; private set; }
    public async Task<string> GetProxy()
    {
        if (ProxyList.Count <= 50)
        {
            await FetchProxy();
        }
        if (ProxyList.Count <= 1)
        {
            return ProxyList[0];
        }
        var grab = ProxyList[1];
        ProxyList.RemoveAt(0);
        return grab;
    }
    public ProxyProvider()
    {
        IsIniting = true;
        _ = FetchProxy();
        IsIniting = false;
    }
    private async Task FetchProxy()
    {
        var client = new HttpClient();
        string response = await client.GetStringAsync($"ConstUri.PROXY_API{100}");
        var list = JsonSerializer.Deserialize<string[]>(response);
        if (list is null)
        {
            return;
        }
        ProxyList.AddRange(list);
    }
}

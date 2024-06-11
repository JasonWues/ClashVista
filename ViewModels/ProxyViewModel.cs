using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;

namespace Clash_Vista.ViewModels;

public partial class ProxyViewModel : ViewModelBase
{
    
    readonly private IHttpClientFactory _httpClientFactory;
    
    public ObservableCollection<string> ProxyList { get; set; }

    public ProxyViewModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [RelayCommand]
    public async Task FetchProxyDelay(string name)
    {
        var httpClient = _httpClientFactory.CreateClient();

        var url = $"/proxies/{name}/delay";

        var defaultUrl = "http://1.1.1.1";

        var response = await httpClient.GetAsync(url);
        
        
    }
}
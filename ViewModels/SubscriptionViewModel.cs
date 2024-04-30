using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Clash_Vista.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SukiUI.Controls;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace Clash_Vista.ViewModels;

public partial class SubscriptionViewModel : ViewModelBase
{
    readonly private IHttpClientFactory _httpClientFactory;

    [ObservableProperty]
    private string _subscriptionLink;

    public SubscriptionViewModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        ProfileItems = [];
    }

    public ObservableCollection<ProfileItem> ProfileItems { get; set; }

    [RelayCommand]
    private async Task FetchSubscriptionLink()
    {
        var clashFlag = "&flag=clash";

        try
        {
            if (string.IsNullOrEmpty(SubscriptionLink))
            {
                return;
            }

            var stringBuilder = new StringBuilder(SubscriptionLink.Trim().Length + clashFlag.Length);
            var sublink = stringBuilder.Append(SubscriptionLink.Trim()).Append(clashFlag).ToString();

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Clash-vista");
            var response = await httpClient.GetAsync(
                sublink);

            if (!response.IsSuccessStatusCode)
            {
                await SukiHost.ShowToast("Error", $"failed to fetch remote profile with status {response.StatusCode}");
                return;
            }

            await ParseProfileFromResponse(response);

        }
        catch (HttpRequestException e)
        {
            await SukiHost.ShowToast("Error", $"Network error: {e.Message}");
            throw;
        }
        catch (YamlException ex)
        {
            await SukiHost.ShowToast("Error", $"YAML parsing error: {ex.Message}");
        }
    }

    private async Task ParseProfileFromResponse(HttpResponseMessage response)
    {
        var subInfoExists = response.Headers.TryGetValues("Subscription-Userinfo", out var subInfo);
        var updateIntervalExists =
            response.Headers.TryGetValues("Profile-Update-Interval", out var updateInterval);
        var homeUrlExists = response.Headers.TryGetValues("profile-web-page-url", out var homeUrl);
        var yaml = await response.Content.ReadAsStringAsync();
        var yamlReader = new StringReader(yaml);
        var yamlStream = new YamlStream();
        yamlStream.Load(yamlReader);

        var mapping =
            (YamlMappingNode)yamlStream.Documents[0].RootNode;

        if (!mapping.Children.ContainsKey("proxies"))
        {
            await SukiHost.ShowToast("Error", "profile does not contain `proxies`");
            return;
        }

        //parse the Subscription UserInfo
        var extra = new ProfileItemExtra();
        if (subInfoExists)
        {
            var subInfoDic = subInfo != null && subInfo.Any()
                ? subInfo.FirstOrDefault().Split(';')
                    .ToDictionary(x => x.Split('=')?[0]?.Trim(), x => long.Parse(x.Split('=')?[1]))
                : new Dictionary<string, long>();

            var uploadExists = subInfoDic.TryGetValue("download", out var upload);
            var downloadExists = subInfoDic.TryGetValue("download", out var download);
            var totalExists = subInfoDic.TryGetValue("total", out var total);
            var expireExists = subInfoDic.TryGetValue("expire", out var expire);

            extra.Upload = uploadExists ? upload : default;
            extra.Download = downloadExists ? download : default;
            extra.Total = totalExists ? total : default;
            extra.Expire = expireExists ? expire : default;
        }


        // Content-Disposition
        var filename = response.Content.Headers.ContentDisposition?.FileNameStar;

        var profileItemOption = new ProfileItemOption
        {
            UpdateInterval = updateIntervalExists ? uint.Parse(updateInterval?.First()) * 60 : null
        };

        var guid = Guid.NewGuid();

        ProfileItems.Add(new ProfileItem
        {
            Uid = guid.ToString(),
            Name = filename,
            Type = ProfileItemType.Remote,
            Desc = default,
            Url = SubscriptionLink,
            HomeUrl = homeUrlExists ? homeUrl?.FirstOrDefault() : null,
            File = $"{guid}.yaml",
            Selected = null,
            Extra = extra,
            Updated = DateTimeOffset.Now.ToUnixTimeSeconds(),
            Option = profileItemOption
        });

        await SukiHost.ShowToast("Info", "Import profile success");
    }
}
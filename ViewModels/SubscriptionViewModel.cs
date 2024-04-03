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
using YamlDotNet.RepresentationModel;

namespace Clash_Vista.ViewModels;

public partial class SubscriptionViewModel : ViewModelBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    [ObservableProperty]
    private string _subscriptionLink;

    public ObservableCollection<ProfileItem> ProfileItems { get; set; }

    public SubscriptionViewModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        ProfileItems = [];
    }

    [RelayCommand]
    private async Task FetchSubscriptionLink()
    {
        string clashFlag = "&flag=clash";

        try
        {
            if (!string.IsNullOrEmpty(SubscriptionLink))
            {
                StringBuilder stringBuilder = new StringBuilder(SubscriptionLink.Trim().Length + clashFlag.Length);
                var sublink = stringBuilder.Append(SubscriptionLink.Trim()).Append(clashFlag).ToString();

                var httpClient = _httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Clash-vista");
                var response = await httpClient.GetAsync(
                    sublink);

                if (response.IsSuccessStatusCode)
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
                    Dictionary<string, long> subInfoDic = subInfo != null && subInfo.Any()
                        ? subInfo.FirstOrDefault().Split(';')
                            .ToDictionary(x => x.Split('=')?[0]?.Trim(), x => long.Parse(x.Split('=')?[1]))
                        : new Dictionary<string, long>();

                    var uploadExists = subInfoDic.TryGetValue("download", out var upload);
                    var downloadExists = subInfoDic.TryGetValue("download", out var download);
                    var totalExists = subInfoDic.TryGetValue("total", out var total);
                    var expireExists = subInfoDic.TryGetValue("expire", out var expire);

                    var extra = new ProfileItemExtra
                    {
                        Upload = uploadExists ? upload : default,
                        Download = downloadExists ? download : default,
                        Total = totalExists ? total : default,
                        Expire = expireExists ? expire : default
                    };

                    // Content-Disposition
                    var filename = response.Content.Headers.ContentDisposition?.FileNameStar;

                    ProfileItemOption profileItemOption = new ProfileItemOption();
                    profileItemOption.UpdateInterval =
                        updateIntervalExists ? uint.Parse(updateInterval?.First()) * 60 : null;

                    var guid = Guid.NewGuid();

                    ProfileItems.Add(new ProfileItem
                    {
                        Uid = guid.ToString(),
                        Name = filename,
                        Type = ProfileItemType.Remote,
                        Desc = default,
                        Url = SubscriptionLink,
                        HomeUrl = homeUrlExists ? homeUrl?.First() : null,
                        File = $"{guid}.yaml",
                        Selected = null,
                        Extra = extra,
                        Updated = DateTimeOffset.Now.ToUnixTimeSeconds(),
                        Option = profileItemOption
                    });
                }
                else
                {
                    await SukiHost.ShowToast("Error", $"failed to fetch remote profile with status {response.StatusCode}");
                }

            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
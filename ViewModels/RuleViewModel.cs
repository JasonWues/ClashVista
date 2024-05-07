using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Clash_Vista.ViewModels;

public partial class RuleViewModel : ViewModelBase
{
    [ObservableProperty]
    private string filter;

    public ObservableCollection<RuleItem> RuleItems { get; set; }
}

public class RuleItem
{
    public int Index { get; set; }

    public string Argument { get; set; }

    public string Type { get; set; }

    public string Policy { get; set; }
}
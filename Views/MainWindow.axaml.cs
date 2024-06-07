using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using SukiUI.Controls;

namespace Clash_Vista.Views;

public partial class MainWindow : SukiWindow
{
    readonly public Process CurrentProcess;

    private DispatcherTimer _timer;

    public MainWindow()
    {
        InitializeComponent();
        CurrentProcess = Process.GetCurrentProcess();
        GetUsedMemory();
    }

    public void GetUsedMemory()
    {
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1),
            IsEnabled = true
        };

        _timer.Tick += (sender, args) =>
        {
            var currentProcess = Process.GetCurrentProcess();
            currentProcess.Refresh();
            MemoryText.Text = (currentProcess.WorkingSet64 / 1048576).ToString("F1");
        };

    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        _timer.Stop();
        Hide();
        e.Cancel = true;
        base.OnClosing(e);
    }
    

    public override void Show()
    {
        _timer.Start();
        base.Show();
    }
}
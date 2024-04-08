using System;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Threading;
using SukiUI.Controls;

namespace Clash_Vista.Views
{
    public partial class MainWindow : SukiWindow
    {
        private Process _currentProcess;

        private DispatcherTimer _timer;
        
        public MainWindow()
        {
            InitializeComponent();
            _currentProcess = Process.GetCurrentProcess();
            GetUsedMemory();
        }
        
        public void GetUsedMemory()
        {
            _timer= new DispatcherTimer()
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
            _currentProcess.Dispose();
            _timer.Stop();
            base.OnClosing(e);
        }
    }
}
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SeverstalTestTask.Other;

namespace SeverstalTestTask;

public partial class LogsWindow : Window
{
    public LogsWindow()
    {
        InitializeComponent();
        DataContext = LogManager.Instance;
    }
}
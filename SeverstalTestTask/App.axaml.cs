using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using SeverstalTestTask.ViewModels;
using SeverstalTestTask.Views;
using SeverstalTestTask.Models;
using SeverstalTestTask.Db;
using System;
using System.IO;
using System.Threading.Tasks;
using SeverstalTestTask.Services;
using SeverstalTestTask.Other;

namespace SeverstalTestTask;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        DisableAvaloniaDataAnnotationValidation();

        var dbContext = new MachineDbContext();
        var databaseService = new DatabaseService(dbContext);
        await databaseService.InitializeAsync();

        var model = new SeverstalTestTaskModel(dbContext);
        var viewModel = new SeverstalTestTaskViewModel(model);
        await viewModel.InitializeAsync();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow { DataContext = viewModel };
        }

        LogManager.Instance.AddEvent("Initialize success! User logged in");

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}
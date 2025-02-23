using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System.Security.Cryptography.X509Certificates;
namespace SeverstalTestTask.Other
{
    public class LogManager
    {
        private static readonly Lazy<LogManager> _instance =
            new Lazy<LogManager>(() => new LogManager());

        public static LogManager Instance => _instance.Value;

        public ObservableCollection<LogEntry> Events { get; } = new();
        public ObservableCollection<LogEntry> Errors { get; } = new();

        public void AddEvent(string message)
        {
            AvaloniaUIThreadDispatcher.Dispatch(() =>
            {
                Events.Add(new LogEntry { Message = message, LogType = "Event" });
            });
        }

        public void AddError(string message)
        {
            AvaloniaUIThreadDispatcher.Dispatch(() =>
            {
                Errors.Add(new LogEntry { Message = message, LogType = "Error" });
            });
        }
        public void SaveLogs()
        {
            try
            {
                var filePath = Path.Combine(AppContext.BaseDirectory, $"logs_{Events.Last().Timestamp:HH-mm-ss_dd-MM-yyyy}.txt");

                var logsToSave = new List<string>();

                foreach (var log in Events)
                    logsToSave.Add($"[EVENT] {log.Timestamp:HH:mm:ss} | {log.Message}");

                foreach (var log in Errors)
                    logsToSave.Add($"[ERROR] {log.Timestamp:HH:mm:ss} | {log.Message}");

                File.WriteAllLines(filePath, logsToSave);

                this.AddEvent($"Logs saved to {filePath}");
            }
            catch (Exception ex)
            {
                this.AddError($"Save failed: {ex.Message}");
            }
        }
        public ICommand SaveLogsCommand => new RelayCommand(SaveLogs);

    }
    public static class AvaloniaUIThreadDispatcher
    {
        public static void Dispatch(Action action)
        {
            if (Avalonia.Application.Current.CheckAccess())
                action();
            else
                Avalonia.Threading.Dispatcher.UIThread.Post(action);
        }
    }
}

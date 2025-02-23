using CommunityToolkit.Mvvm.Input;
using SeverstalTestTask.Other;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;
using SeverstalTestTask.Models;
using System.Threading.Tasks;
using SeverstalTestTask.Db;
using System.Collections.Generic;
using System.Linq;
using SeverstalTestTask.Views;
using Avalonia.OpenGL;

namespace SeverstalTestTask.ViewModels
{
    public partial class SeverstalTestTaskViewModel : ViewModelBase
    {
        private readonly SeverstalTestTaskModel _model;
        private LogsWindow _logsWindow;
        public ObservableCollection<MachineRecord> MachineRecords
        {
            get => _machineRecords;
            set
            {
                _machineRecords = value;
                OnPropertyChanged(nameof(MachineRecords));
                UpdateMachineNumbers();
            }
        }
        private ObservableCollection<MachineRecord> _machineRecords = new();

        public ObservableCollection<int> MachineNumbers
        {
            get => _machineNumbers;
            set
            {
                _machineNumbers = value;
                OnPropertyChanged(nameof(MachineNumbers));
            }
        }
        private ObservableCollection<int> _machineNumbers = new();

        public int SelectedMachineNumber
        {
            get => _selectedMachineNumber;
            set
            {
                _selectedMachineNumber = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FilteredMachineRecords));
            }
        }
        private int _selectedMachineNumber;

        public ObservableCollection<MachineRecord> FilteredMachineRecords
        {
            get
            {
                if (SelectedMachineNumber != 0)
                {
                    var filteredRecords = MachineRecords
                        .Where(x => x.MachineNumber == SelectedMachineNumber)
                        .ToList();

                    return [.. filteredRecords];
                }
                else
                {
                    LogManager.Instance.AddEvent("Machine filter is reset");
                    return MachineRecords;
                }
            }
        }

        public decimal GrossWeight
        {
            get => _grossWeight;
            set => SetProperty(ref _grossWeight, value);
        }
        private decimal _grossWeight;

        public decimal TareWeight
        {
            get => _tareWeight;
            set => SetProperty(ref _tareWeight, value);
        }
        private decimal _tareWeight;

        public ICommand AddRecordCommand { get; private set; } = null!;
        public ICommand ResetMachineNumberFilterCommand { get; private set; } = null!;
        public ICommand OpenGraphCommand { get; private set; } = null!;
        public ICommand OpenLogsCommand { get; private set; } = null!;

        public SeverstalTestTaskViewModel(SeverstalTestTaskModel model)
        {
            _model = model;
            InitializeCommands();
        }

        public SeverstalTestTaskViewModel()
        {
            _model = new SeverstalTestTaskModel(new MachineDbContext());
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            AddRecordCommand = new AsyncRelayCommand(AddRecordAsync);
            ResetMachineNumberFilterCommand = new RelayCommand(ResetMachineNumberFilter);
            OpenGraphCommand = new RelayCommand(OpenGraph);
            OpenLogsCommand = new RelayCommand(OpenLogs);
        }

        public async Task InitializeAsync()
        {
            try
            {
                //await _model.GenerateRecordsAsync(255);

                var records = await _model.GetRecordsAsync();
                MachineRecords.Clear();
                foreach (var record in records)
                {
                    MachineRecords.Add(record);
                }
                UpdateMachineNumbers();

                LogManager.Instance.AddEvent($"DataBase is success initialize");
            }
            catch (Exception ex)
            {
                LogManager.Instance.AddError($"Error loading data from DataBase: {ex.Message}");
            }
        }

        private void UpdateMachineNumbers()
        {
            var uniqueMachineNumbers = MachineRecords
                .Select(x => x.MachineNumber)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            MachineNumbers = new ObservableCollection<int>(uniqueMachineNumbers);
            OnPropertyChanged(nameof(SelectedMachineNumber));
            OnPropertyChanged(nameof(FilteredMachineRecords));
        }

        private void ResetMachineNumberFilter()
        {
            SelectedMachineNumber = 0;
            OnPropertyChanged(nameof(FilteredMachineRecords));
            LogManager.Instance.AddEvent("Reset machine filter");
        }
        private void OpenGraph()
        {
            var graph = new GraphWindow(FilteredMachineRecords);
            graph.Show();
        }
        private void OpenLogs()
        {
            if (_logsWindow == null || !_logsWindow.IsVisible)
            {
                _logsWindow = new LogsWindow();
                _logsWindow.Closed += (s, e) => _logsWindow = null;
                _logsWindow.Show();

                LogManager.Instance.AddEvent("Logs are openned");
            }
            else
            {
                _logsWindow.Activate();
            }
        }

        private async Task AddRecordAsync()
        {
            try
            {
                var newRecord = new MachineRecord(
                    machineNumber: SelectedMachineNumber,
                    grossWeight: GrossWeight,
                    tareWeight: TareWeight,
                    netWeight: MachineRecord.CalculateNetWeight(GrossWeight, TareWeight),
                    tareDate: DateTime.Now,
                    grossDate: DateTime.Now);

                await _model.AddRecordAsync(newRecord);
                await _model.SaveChangesAsync();

                MachineRecords.Add(newRecord);
                UpdateMachineNumbers();

                LogManager.Instance.AddEvent("Record was added successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding record: {ex.Message}");
            }
        }
    }
}
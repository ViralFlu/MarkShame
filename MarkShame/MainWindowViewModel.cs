using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
// MVVM Toolkit
using CommunityToolkit.Mvvm.ComponentModel;

namespace MarkShame
{
    public class MainWindowViewModel : ObservableObject
    {
        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                OnPropertyChanged(nameof(FilePath));
            }
        }

        public bool IsProcessing
        {
            get => _isProcessing;
            set
            {
                if (value == _isProcessing) return;
                _isProcessing = value;
                OnPropertyChanged(nameof(IsProcessing));
                OnPropertyChanged(nameof(CanEdit));
            }
        }
        public bool CanEdit
        {
            get { return !IsProcessing; }
        }

        private bool _isProcessing;

        public AsyncRelayCommand ParseFileCommand { get; }
        public RelayCommand BrowseCommand { get; }

        public OverallDataGridViewModel DataGrid { get; }
        public OverallBarChartViewModel BarChart { get; set; }

        public MainWindowViewModel()
        {

            ParseFileCommand = new AsyncRelayCommand(
                ParseFile,
                CanParseFile);
            DataGrid = new 
                OverallDataGridViewModel();
            BarChart = new OverallBarChartViewModel();
            BrowseCommand = new RelayCommand(OnBrowse, CanBrowse);
        }

        private bool CanBrowse()
        {
            return CanEdit;
        }

        private bool CanParseFile()
        {
            return !string.IsNullOrWhiteSpace(FilePath) && CanEdit;
        }

        private async Task ParseFile()
        {
            IsProcessing = true;
            // Parse the log file using the specified file path
            var entries = await Task.Run(()=>MarkParser.Parse(FilePath));
            DataGrid.UpdateEncounters(entries);
            BarChart.UpdateChart(entries.SelectMany(x=>x.SpellCastSuccessLines).ToList());
            IsProcessing = false;
        }

        private void OnBrowse()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Combat log files|WoWCombatLog*.txt";
            dialog.InitialDirectory = @"C:\Program Files (x86)\World of Warcraft\_retail_\Logs";

            if (dialog.ShowDialog() == true)
            {
                FilePath = dialog.FileName;
            }

            ParseFileCommand.NotifyCanExecuteChanged();
        }
    }

}

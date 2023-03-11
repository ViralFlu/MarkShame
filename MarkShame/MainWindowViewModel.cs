using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;

namespace MarkShame
{
    public class MainWindowViewModel : INotifyPropertyChanged
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

        private ObservableCollection<Encounter> _logEntries;
        private bool _isProcessing;

        public ObservableCollection<Encounter> Encounters
        {
            get { return _logEntries; }
            set
            {
                _logEntries = value;
                OnPropertyChanged(nameof(Encounters));
            }
        }

        public AsyncRelayCommand ParseFileCommand { get; }
        public RelayCommand BrowseCommand { get; }

        public MainWindowViewModel()
        {
            Encounters = new ObservableCollection<Encounter>();

            ParseFileCommand = new AsyncRelayCommand(
                ParseFile,
                CanParseFile);
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
            var entries = await LogParser.Parse(FilePath);

            // Update the LogEntries property with the parsed entries
            Encounters.Clear();
            foreach (var entry in entries)
            {
                if(entry.SpellCastSuccessLines.Any())
                    Encounters.Add(entry);
            }

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}

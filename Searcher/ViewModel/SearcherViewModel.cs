using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Searcher.Model;
using Searcher.Model.Dynamics;
using System.Windows;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Searcher.ViewModel
{
    public class SearcherViewModel : DependencyObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties

        #region SettingsStatusMessageText
        private string _SettingsStatusMessageText = string.Empty;
        public string SettingsStatusMessageText
        {
            get
            {
                return _SettingsStatusMessageText;
            }
            set
            {
                if (_SettingsStatusMessageText != value)
                {
                    _SettingsStatusMessageText = value;
                    NotifyPropertyChanged("SettingsStatusMessageText");
                }
            }
        }
        #endregion // SettingsStatusMessageText

        #region PersonSourcePathTextProperty
        private string _PersonSourcePath;
        public string PersonSourcePathText
        {
            get
            {
                return _PersonSourcePath;
            }
            set
            {
                if (_PersonSourcePath != value)
                {
                    OriginalPersons = PersonProvider.FromFile(value);
                    NotifyPropertyChanged("OriginalPersons"); // Not sure if neccesary in all cases

                    _PersonSourcePath = value;
                    NotifyPropertyChanged("PersonSourcePathText"); // Not sure if neccesary in all cases

                    Properties.Settings.Default.LastPersonSource = value;
                }
            }
        }
        #endregion // PersonSourcePathTextProperty

        #region SearchTextProperty
        private string _SearchText;
        public string SearchText
        {
            get
            {
                if (_SearchText == null)
                {
                    return string.Empty;
                }
                return _SearchText;
            }
            set
            {
                if (_SearchText != value)
                {
                    _SearchText = value;
                    NotifyPropertyChanged("SearchText"); // Not sure if neccesary in all cases

                    if (OriginalPersons != null)
                    {
                        DisplayedPersons = OriginalPersons.GetPersonsBySearchTerm(_SearchText);
                        NotifyPropertyChanged("DisplayedPersons"); // Not sure if neccesary in all cases
                    }
                }
            }
        }
        #endregion // SearchTextProperty

        #region DisplayedPersonsProperty
        public DynamicCollection<DynamicPerson> DisplayedPersons
        {
            get { return (DynamicCollection<DynamicPerson>)GetValue(DisplayedPersonsProperty); }
            set { SetValue(DisplayedPersonsProperty, value); }
        }
        public static readonly DependencyProperty DisplayedPersonsProperty = DependencyProperty.Register(
            "DisplayedPersons",
            typeof(DynamicCollection<DynamicPerson>),
            typeof(SearcherViewModel),
            new UIPropertyMetadata(null));
        #endregion // DisplayedPersonsProperty

        #region OriginalPersonsProperty
        public DynamicCollection<DynamicPerson> OriginalPersons
        {
            get
            {
                return (DynamicCollection<DynamicPerson>)GetValue(OriginalPersonsProperty);
            }
            set
            {
                SetValue(OriginalPersonsProperty, value);
                if (OriginalPersons != null)
                {
                    DisplayedPersons = OriginalPersons.GetPersonsBySearchTerm(SearchText);
                }
            }
        }
        public static readonly DependencyProperty OriginalPersonsProperty = DependencyProperty.Register(
            "OriginalPersons",
            typeof(DynamicCollection<DynamicPerson>),
            typeof(SearcherViewModel),
            new UIPropertyMetadata(null));
        #endregion // DisplayedPersonsProperty

        #endregion // Properties

        #region Initialization

        public SearcherViewModel()
        {
            InitializeRunStatus();
            InitializeRelayCommands();

            // Load persons from a file and into the originalPersons
            // Changing SourchPath, automatically loads OriginalPersons, which automatically changes DisplayedPersons
            if (!string.IsNullOrEmpty(Properties.Settings.Default.LastPersonSource))
            {
                PersonSourcePathText = Properties.Settings.Default.LastPersonSource;
            }
        }

        protected void InitializeRunStatus()
        {
            Properties.Settings.Default.FirstRun = false;
        }

        protected void InitializeRelayCommands()
        {
            BrowseSourcePath = new RelayCommand(BrowseSourcePath_Execute);
        }

        #endregion // Initialization

        #region Commands
        public RelayCommand BrowseSourcePath { get; set; }

        internal void BrowseSourcePath_Execute(object parameter)
        {
            try
            {
                if (CommonFileDialog.IsPlatformSupported)
                {
                    PersonSourcePathText = GetFileFromCommonFileDialog();
                }
                else
                {
                    PersonSourcePathText = GetFileFromDefaultFileDialog();
                }

                SettingsStatusMessageText = String.Empty;
            }
            catch (SearcherException e)
            {
                SettingsStatusMessageText = e.Message;
            }
        }

        #endregion

        #region INotifyPropertyChanged
        internal void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        #endregion // INotifyPropertyChanged

        #region File Browsers
        private string GetFileFromCommonFileDialog()
        {
            // Use pretty file dialog picker
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            string selectedFileName = string.Empty;

            if (!string.IsNullOrEmpty(PersonSourcePathText))
            {
                dialog.InitialDirectory = Path.GetDirectoryName(PersonSourcePathText);
            }
            dialog.Filters.Add(new CommonFileDialogFilter("CSV Files", "*.csv"));
            dialog.IsFolderPicker = false;
            dialog.EnsurePathExists = true;

            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                selectedFileName = dialog.FileName;
            }
            else
            {
                throw new NoFileSelectedException();
            }

            return selectedFileName;
        }

        private string GetFileFromDefaultFileDialog()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            string selectedFileName = string.Empty;

            if (string.IsNullOrEmpty(PersonSourcePathText))
            {
                dialog.InitialDirectory = Path.GetDirectoryName(PersonSourcePathText);
            }
            dialog.Filter = "CSV Files|*.csv";
            dialog.CheckPathExists = true;
            dialog.Multiselect = false;

            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                selectedFileName = dialog.FileName;
            }
            return selectedFileName;
        }
        #endregion // File Browsers
    }
}

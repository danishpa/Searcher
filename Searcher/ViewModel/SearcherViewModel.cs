using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Searcher.Model;
using Searcher.Model.Dynamics;
using System.Windows;

namespace Searcher.ViewModel
{
    public class SearcherViewModel : DependencyObject, IClosableViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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
                    _PersonSourcePath = value;
                    NotifyPropertyChanged("PersonSourcePathText"); // Not sure if neccesary in all cases

                    OriginalPersons = PersonProvider.FromFile(_PersonSourcePath);
                    NotifyPropertyChanged("OriginalPersons"); // Not sure if neccesary in all cases
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

        public SearcherViewModel()
        {
            InitializeRunStatus();

            // Load persons from a file and into the originalPersons
            // Changing SourchPath, automatically loads OriginalPersons, which automatically changes DisplayedPersons
            PersonSourcePathText = @"C:\Projects\Searcher\Searcher\samples\people_hebrew.csv";

        }

        private void InitializeRunStatus()
        {
            Properties.Settings.Default.FirstRun = false;
        }

        internal void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using Searcher.Model;
using Searcher.Model.Dynamics;
using System.Windows;

namespace Searcher.ViewModel
{
    public enum PersonProviderSupportedFileTypes
    {
        Unknown = 0,
        CSV
    }

    public class SearcherViewModel : DependencyObject, IClosableViewModel, INotifyPropertyChanged
    {
        #region Members
        internal static char CSVSeperator = ',';
        internal static readonly string SuffixSeperator = ".";

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion // Members

        #region DependenyProperties
        public DynamicCollection<DynamicPerson> DisplayedPersons
        {
            get { return (DynamicCollection<DynamicPerson>)GetValue(DisplayedPersonsProperty); }
            set { SetValue(DisplayedPersonsProperty, value); }
        }

        public static readonly DependencyProperty DisplayedPersonsProperty =
        DependencyProperty.Register(
            "DisplayedPersons",
            typeof(DynamicCollection<DynamicPerson>),
            typeof(SearcherViewModel),
            new UIPropertyMetadata(null));

        internal DynamicCollection<DynamicPerson> OriginalPersons;
        #endregion // DependenyProperties

        #region Static Members
        internal static Dictionary<PersonProviderSupportedFileTypes, Func<string, DynamicCollection<DynamicPerson>>> ProviderTypeToFunc =
            new Dictionary<PersonProviderSupportedFileTypes, Func<string, DynamicCollection<DynamicPerson>>>()
            {
                { PersonProviderSupportedFileTypes.CSV, FromCSVFile }
            };
        #endregion // Static Members

        public SearcherViewModel()
        {
            InitializeRunStatus();

            // Load persons from a file and into the originalPersons
            LoadPersonsFromFile(@"C:\Projects\Searcher\Searcher\samples\people_hebrew.csv");
        }

        private void InitializeRunStatus()
        {
            Properties.Settings.Default.FirstRun = false;
        }

        private void LoadPersonsFromFile(string filePath)
        {
            OriginalPersons = FromFile(filePath);
        }

        internal void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        #region Static Methods
        public static DynamicCollection<DynamicPerson> FromCSVFile(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
            string[] headers = lines[0].Split(CSVSeperator);

            // Skip first line (containing the headers)
            var data = from line in lines.Skip(1)
                       let elements = line.Split(CSVSeperator)
                       select new DynamicPerson(headers, elements);

            return new DynamicCollection<DynamicPerson>(data.ToList());
        }

        public static DynamicCollection<DynamicPerson> FromFile(string filePath)
        {
            PersonProviderSupportedFileTypes fileType = PersonProviderSupportedFileTypes.Unknown;
            string fileSuffix = Path.GetExtension(filePath).Replace(SuffixSeperator, String.Empty).ToUpper();

            if (!Enum.TryParse(fileSuffix, out fileType)
                 || fileType == PersonProviderSupportedFileTypes.Unknown)
            {
                throw new UnrecognizedFileTypeException();
            }
            return ProviderTypeToFunc[fileType](filePath);
        }
        #endregion // Static Methods
    }
}

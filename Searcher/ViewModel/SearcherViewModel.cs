using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using Searcher.Model;
using Searcher.Model.Dynamics;

namespace Searcher.ViewModel
{
    public enum PersonProviderSupportedFileTypes
    {
        Unknown = 0,
        CSV
    }

    public class SearcherViewModel : IClosableViewModel, INotifyPropertyChanged
    {
        #region Members
        internal static char CSVSeperator = ',';
        internal static readonly string SuffixSeperator = ".";

        public DynamicList<DynamicPerson> Persons;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion // Members

        #region Static Members
        internal static Dictionary<PersonProviderSupportedFileTypes, Func<string, DynamicList<DynamicPerson>>> ProviderTypeToFunc =
            new Dictionary<PersonProviderSupportedFileTypes, Func<string, DynamicList<DynamicPerson>>>()
            {
                { PersonProviderSupportedFileTypes.CSV, FromCSVFile }
            };
        #endregion // Static Members

        public SearcherViewModel()
        {
            // Load persons from a file and into the originalPersons
            LoadPersonsFromFile(@"C:\Projects\Searcher\Searcher\samples\people_hebrew.csv");

        }

        private void LoadPersonsFromFile(string filePath)
        {
            Persons = FromFile(filePath);
        }

        internal void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        #region Static Methods
        public static DynamicList<DynamicPerson> FromCSVFile(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
            string[] headers = lines[0].Split(CSVSeperator);

            // Skip first line (containing the headers)
            var data = from line in lines.Skip(1)
                       let elements = line.Split(CSVSeperator)
                       select new DynamicPerson(headers, elements);

            return new DynamicList<DynamicPerson>(data.ToList());
        }

        public static DynamicList<DynamicPerson> FromFile(string filePath)
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

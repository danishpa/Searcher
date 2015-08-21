using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Searcher
{
    public enum PersonProviderSupportedFileTypes
    {
        CSV
    }

    public static class PersonProvider
    {
        public static char CSVSeperator = ',';

        public static Dictionary<string, PersonProviderSupportedFileTypes> suffixToFileType =
            new Dictionary<string, PersonProviderSupportedFileTypes>()
            {
                { PersonProviderSupportedFileTypes.CSV.ToString(), PersonProviderSupportedFileTypes.CSV }
            };

        public static Dictionary<PersonProviderSupportedFileTypes, Func<string, List<Person>>> ProviderTypeToFunc =
            new Dictionary<PersonProviderSupportedFileTypes, Func<string, List<Person>>>()
            {
                { PersonProviderSupportedFileTypes.CSV, FromCSVFile }
            };

        public static List<Person> FromCSVFile(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
            var data = from line in lines
                       let elements = line.Split(CSVSeperator)
                       where elements.Length >= 2
                       select new Person(
                           elements[0],
                           elements[1],
                           elements[2]
                       );
            
            return data.ToList();
        }

        public static List<Person> FromFile(string filePath, PersonProviderSupportedFileTypes type)
        {
            return ProviderTypeToFunc[type](filePath);
        }
    }
}

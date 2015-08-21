using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Searcher
{
    public enum PersonProviderSupportedFileTypes
    {
        Unknown = 0,
        CSV
    }

    public static class PersonProvider
    {
        internal static char CSVSeperator = ',';
        internal static readonly string SuffixSeperator = ".";

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
         
        public static List<Person> FromFile(string filePath)
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
    }
}

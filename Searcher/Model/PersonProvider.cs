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

        public static Dictionary<PersonProviderSupportedFileTypes, Func<string, List<DynamicPerson>>> ProviderTypeToFunc =
            new Dictionary<PersonProviderSupportedFileTypes, Func<string, List<DynamicPerson>>>()
            {
                { PersonProviderSupportedFileTypes.CSV, FromCSVFile }
            };

        public static List<DynamicPerson> FromCSVFile(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
            string[] headers = lines[0].Split(CSVSeperator);

            // Skip first line (containing the headers)
            var data = from line in lines.Skip(1)
                       let elements = line.Split(CSVSeperator)
                       select new DynamicPerson(headers, elements);
            
            return data.ToList();
        }
         
        public static List<DynamicPerson> FromFile(string filePath)
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

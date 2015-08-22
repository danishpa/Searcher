using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Searcher.Model.Dynamics;

namespace Searcher.Model
{
    public enum PersonProviderSupportedFileTypes
    {
        Unknown = 0,
        CSV
    }

    public static class PersonProvider
    {
        #region Static Members
        internal static char CSVSeperator = ',';
        internal static readonly string SuffixSeperator = ".";
        internal static Dictionary<PersonProviderSupportedFileTypes, Func<string, DynamicCollection<DynamicPerson>>> ProviderTypeToFunc =
            new Dictionary<PersonProviderSupportedFileTypes, Func<string, DynamicCollection<DynamicPerson>>>()
            {
                { PersonProviderSupportedFileTypes.CSV, FromCSVFile }
            };
        #endregion // Static Members

        public static DynamicCollection<DynamicPerson> GetPersonsBySearchTerm(this DynamicCollection<DynamicPerson> persons, string searchTerm)
        {
            // TODO: 
            // 1. Smarter search logic - Fuzzy search - Consecutive letters
            // 2. Searching in english tries to convert characters to hebrew and search
            // 3. But if the searched string is in english, search in english
            // 4. Try to order by relevance (SearchAll should return a float which represents how good is the match)

            string[] whiteSpaces = { " " };
            string[] searchTerms = searchTerm.Split(whiteSpaces, StringSplitOptions.RemoveEmptyEntries);

            // Return all persons, for which all the search terms appear in their properties
            return new DynamicCollection<DynamicPerson>(persons
                .Where(person => person.SearchAll(searchTerms))
                .ToList());
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

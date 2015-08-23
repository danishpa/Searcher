using System.Linq;
using Searcher.Model.Dynamics;

namespace Searcher.Model
{
    public interface ISearchable
    {
        int Search(string searchTerm);
    }
    
    public static class SearchableExtension
    {
        public static bool InnerFuzzyContains(string stringToSearch, string searchTerm)
        {
            int i = 0, j = 0;

            if (stringToSearch.Contains(searchTerm))
            {
                return true;
            }

            for (i = 0, j = 0; (i < stringToSearch.Length) && (j < searchTerm.Length); i++)
            {
                if (stringToSearch[i] == searchTerm[j])
                {
                    ++j;
                }
            }
            return (searchTerm.Length <= j);
        }

        /// <summary>
        /// Checks if the string receives fuzzy-ly contains the searchTerm
        /// ABCDEFGHI, BCGH -> true (because BC and GH appear in the string, even if there are other characters in the middle)
        /// ABCDEFGHI, DCBA -> false (because the order is incorrect)
        /// </summary>
        /// <remarks>Extends <typeparamref name="string"/> type</remarks>
        public static bool FuzzyContains(this string stringToSearch, string searchTerm)
        {
            string normalizedSearchTerm, normalizedStringToSearch;

            normalizedStringToSearch = stringToSearch.ToNormalizedString();
            normalizedSearchTerm = searchTerm.ToNormalizedString();

            if (InnerFuzzyContains(normalizedStringToSearch, normalizedSearchTerm))
            {
                return true;
            }

            // LangOver search term, and redo the fuzzy search
            return InnerFuzzyContains(normalizedStringToSearch, normalizedSearchTerm.LangOver());
        }

        public static int Search(this ISearchable searchable, string[] searchTerms)
        {
            return searchTerms.Select(searchTerm => searchable.Search(searchTerm)).Sum();
        }

        // Returns true if each term in searchTerms appears in the properties. false otherwise
        public static bool SearchAll(this ISearchable searchable, string[] searchTerms)
        {
            return searchTerms.Select(searchTerm => searchable.Search(searchTerm)).All(x => x > 0);
        }
    }
}

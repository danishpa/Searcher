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
        public static bool FuzzyContains(this string property, string searchTerm)
        {
            return property.Contains(searchTerm);
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

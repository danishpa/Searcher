using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Searcher;
using Searcher.Model;

namespace Searcher.Model.Tests
{
    [TestClass()]
    public class SearchableExtensionTests
    {
        private class SearchableOneResultSearch : ISearchable
        {
            public int Search(string searchTerm)
            {
                return 1;
            }
        }

        private class SearchableZeroResultSearch : ISearchable
        {
            public int Search(string searchTerm)
            {
                return 0;
            }
        }

        [TestMethod()]
        public void Search_Sanity_Test()
        {
            SearchableOneResultSearch s1 = new SearchableOneResultSearch();
            Assert.AreEqual(1, s1.Search("a"));

            SearchableZeroResultSearch s2 = new SearchableZeroResultSearch();
            Assert.AreEqual(0, s2.Search("a"));
        }

        [TestMethod()]
        public void Search_Valid_Searches_Test()
        {
            SearchableOneResultSearch s1 = new SearchableOneResultSearch();
            Assert.AreEqual(0, s1.Search(new string[] { }));
            Assert.AreEqual(1, s1.Search(new string[] { "a", }));
            Assert.AreEqual(2, s1.Search(new string[] { "a", "b" }));
            Assert.AreEqual(3, s1.Search(new string[] { "a", "b", "c" }));
            Assert.AreEqual(4, s1.Search(new string[] { "a", "b", "c", "d" }));
            Assert.AreEqual(5, s1.Search(new string[] { "a", "b", "c", "d", "e" }));

            SearchableZeroResultSearch s2 = new SearchableZeroResultSearch();
            Assert.AreEqual(0, s2.Search(new string[] { }));
            Assert.AreEqual(0, s2.Search(new string[] { "a" }));
            Assert.AreEqual(0, s2.Search(new string[] { "a", "b" }));
            Assert.AreEqual(0, s2.Search(new string[] { "a", "b", "c" }));
            Assert.AreEqual(0, s2.Search(new string[] { "a", "b", "c", "d" }));
            Assert.AreEqual(0, s2.Search(new string[] { "a", "b", "c", "d", "e" }));
        }

        [TestMethod()]
        public void SearchAll_Valid_Searches_Test()
        {
            SearchableOneResultSearch s1 = new SearchableOneResultSearch();
            Assert.IsTrue(s1.SearchAll(new string[] { })); // Empty search terms is OK!
            Assert.IsTrue(s1.SearchAll(new string[] { "a", }));
            Assert.IsTrue(s1.SearchAll(new string[] { "a", "b" }));
            Assert.IsTrue(s1.SearchAll(new string[] { "a", "b", "c" }));
            Assert.IsTrue(s1.SearchAll(new string[] { "a", "b", "c", "d" }));
            Assert.IsTrue(s1.SearchAll(new string[] { "a", "b", "c", "d", "e" }));

            SearchableZeroResultSearch s2 = new SearchableZeroResultSearch();
            Assert.IsTrue(s2.SearchAll(new string[] { })); // Empty search terms is OK!
            Assert.IsFalse(s2.SearchAll(new string[] { "a" }));
            Assert.IsFalse(s2.SearchAll(new string[] { "a", "b" }));
            Assert.IsFalse(s2.SearchAll(new string[] { "a", "b", "c" }));
            Assert.IsFalse(s2.SearchAll(new string[] { "a", "b", "c", "d" }));
            Assert.IsFalse(s2.SearchAll(new string[] { "a", "b", "c", "d", "e" }));
        }

        [TestMethod()]
        public void InnerFuzzyContains_Contains_Test()
        {
            string stringToSearch = "abcdefghi";

            Assert.IsTrue(SearchableExtension.InnerFuzzyContains(stringToSearch, "a"));
            Assert.IsTrue(SearchableExtension.InnerFuzzyContains(stringToSearch, "i"));
            Assert.IsTrue(SearchableExtension.InnerFuzzyContains(stringToSearch, "d"));
            Assert.IsTrue(SearchableExtension.InnerFuzzyContains(stringToSearch, "ab"));
            Assert.IsTrue(SearchableExtension.InnerFuzzyContains(stringToSearch, "bc"));
            Assert.IsTrue(SearchableExtension.InnerFuzzyContains(stringToSearch, "abc"));
            Assert.IsTrue(SearchableExtension.InnerFuzzyContains(stringToSearch, "cde"));
            Assert.IsTrue(SearchableExtension.InnerFuzzyContains(stringToSearch, "ghi"));
            Assert.IsTrue(SearchableExtension.InnerFuzzyContains(stringToSearch, "defg"));
            Assert.IsTrue(SearchableExtension.InnerFuzzyContains(stringToSearch, "abcde"));
            Assert.IsTrue(SearchableExtension.InnerFuzzyContains(stringToSearch, "bcdefgh"));
            Assert.IsTrue(SearchableExtension.InnerFuzzyContains(stringToSearch, stringToSearch));
        }

        [TestMethod()]
        public void InnerFuzzyContains_Fuzzy_Valid_Terms_Test()
        {
            string stringToSearch = "abcdefghi";

            Assert.IsTrue(SearchableExtension.InnerFuzzyContains(stringToSearch, "ai"));
            Assert.IsTrue(SearchableExtension.InnerFuzzyContains(stringToSearch, "hi"));
            Assert.IsTrue(SearchableExtension.InnerFuzzyContains(stringToSearch, "ah"));
            Assert.IsTrue(SearchableExtension.InnerFuzzyContains(stringToSearch, "bi"));
            Assert.IsTrue(SearchableExtension.InnerFuzzyContains(stringToSearch, "bi"));
            Assert.IsTrue(SearchableExtension.InnerFuzzyContains(stringToSearch, "acdgh"));
            Assert.IsTrue(SearchableExtension.InnerFuzzyContains(stringToSearch, "acefi"));
            Assert.IsTrue(SearchableExtension.InnerFuzzyContains(stringToSearch, "abcghi"));
            Assert.IsTrue(SearchableExtension.InnerFuzzyContains(stringToSearch, "abcdei"));
            Assert.IsTrue(SearchableExtension.InnerFuzzyContains(stringToSearch, "abcdehi"));
            Assert.IsTrue(SearchableExtension.InnerFuzzyContains(stringToSearch, stringToSearch));
        }

        [TestMethod()]
        public void InnerFuzzyContains_Unicode_Test()
        {
            string stringToSearch = "אבגדהו";
            
            Assert.IsTrue(SearchableExtension.InnerFuzzyContains(stringToSearch, "בגד"));
            Assert.IsTrue(SearchableExtension.InnerFuzzyContains(stringToSearch, "אב"));
            Assert.IsTrue(SearchableExtension.InnerFuzzyContains(stringToSearch, "אגה"));
            Assert.IsTrue(SearchableExtension.InnerFuzzyContains(stringToSearch, "דה"));
            Assert.IsTrue(SearchableExtension.InnerFuzzyContains(stringToSearch, "אבדו"));
            Assert.IsTrue(SearchableExtension.InnerFuzzyContains(stringToSearch, stringToSearch));
            Assert.IsFalse(SearchableExtension.InnerFuzzyContains(stringToSearch, "זחט"));
            Assert.IsFalse(SearchableExtension.InnerFuzzyContains(stringToSearch, "שיט"));
        }
        [TestMethod()]
        public void InnerFuzzyContains_Empty_Terms_Test()
        {
            string testString = "abcdefghi";
            string emptyString = "";

            Assert.IsTrue(SearchableExtension.InnerFuzzyContains(testString, emptyString));
            Assert.IsTrue(SearchableExtension.InnerFuzzyContains(emptyString, emptyString));
            Assert.IsFalse(SearchableExtension.InnerFuzzyContains(emptyString, testString));
        }

        [TestMethod()]
        public void FuzzyContains_Valid_Extension_Call_Test()
        {
            string stringToSearch = "abcdefghi";

            Assert.IsTrue(stringToSearch.FuzzyContains("ai"));
            Assert.IsTrue(stringToSearch.FuzzyContains("hi"));
            Assert.IsTrue(stringToSearch.FuzzyContains("ah"));
        }

        [TestMethod()]
        public void FuzzyContains_Capitalization_Test()
        {
            string stringToSearch = "ABcdEfGhiJ";

            Assert.IsTrue(stringToSearch.FuzzyContains("abcd"));
            Assert.IsTrue(stringToSearch.FuzzyContains("ABcD"));
            Assert.IsTrue(stringToSearch.FuzzyContains("EFGJ"));
            Assert.IsTrue(stringToSearch.FuzzyContains("ABCDEF"));
            Assert.IsTrue(stringToSearch.FuzzyContains("cDeFGiJ"));
            Assert.IsTrue(stringToSearch.FuzzyContains("abcdefghij"));
            Assert.IsTrue(stringToSearch.FuzzyContains("ABCDEFGHIJ"));
        }

        [TestMethod()]
        public void FuzzyContains_Valid_End_Characters_Test()
        {
            string stringToSearch = "ארם";

            Assert.IsTrue(stringToSearch.FuzzyContains("מ"));
            Assert.IsTrue(stringToSearch.FuzzyContains("ם"));
            Assert.IsTrue(stringToSearch.FuzzyContains("אמ"));
            Assert.IsTrue(stringToSearch.FuzzyContains("אם"));
            Assert.IsTrue(stringToSearch.FuzzyContains("ארמ"));
            Assert.IsTrue(stringToSearch.FuzzyContains("ארם"));

            Assert.IsTrue("בלהבלהץ".FuzzyContains("בלצ"));
            Assert.IsTrue("בלהבלהף".FuzzyContains("בלפ"));
            Assert.IsTrue("בלהבלהך".FuzzyContains("בלכ"));
            Assert.IsTrue("בלהבלהן".FuzzyContains("בלנ"));
        }

        [TestMethod()]
        public void FuzzyContains_LangOver_Test()
        {
            string stringToSearch = "שדגכעיחל";

            Assert.IsTrue(stringToSearch.FuzzyContains("כעיח"));
            Assert.IsTrue(stringToSearch.FuzzyContains("שדיח"));
            Assert.IsTrue(stringToSearch.FuzzyContains("שדגכעיל"));
            Assert.IsTrue(stringToSearch.FuzzyContains("שדגכעיחל"));
            Assert.IsTrue(stringToSearch.FuzzyContains("a"));
            Assert.IsTrue(stringToSearch.FuzzyContains("as"));
            Assert.IsTrue(stringToSearch.FuzzyContains("ashj"));
            Assert.IsTrue(stringToSearch.FuzzyContains("ashjk"));
            Assert.IsTrue(stringToSearch.FuzzyContains("asdfghjk"));
        }

        [TestMethod()]
        public void FuzzyContains_Empty_Terms_Test()
        {
            string testString = "abcdefghi";
            string emptyString = "";

            Assert.IsTrue(SearchableExtension.FuzzyContains(testString, emptyString));
            Assert.IsTrue(SearchableExtension.FuzzyContains(emptyString, emptyString));
            Assert.IsFalse(SearchableExtension.FuzzyContains(emptyString, testString));
        }
    }
}
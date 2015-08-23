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
        public void FuzzyContains_Contains_Test()
        {
            string stringToSearch = "abcdefghi";

            Assert.IsTrue(stringToSearch.FuzzyContains("a"));
            Assert.IsTrue(stringToSearch.FuzzyContains("i"));
            Assert.IsTrue(stringToSearch.FuzzyContains("d"));
            Assert.IsTrue(stringToSearch.FuzzyContains("ab"));
            Assert.IsTrue(stringToSearch.FuzzyContains("bc"));
            Assert.IsTrue(stringToSearch.FuzzyContains("abc"));
            Assert.IsTrue(stringToSearch.FuzzyContains("cde"));
            Assert.IsTrue(stringToSearch.FuzzyContains("ghi"));
            Assert.IsTrue(stringToSearch.FuzzyContains("defg"));
            Assert.IsTrue(stringToSearch.FuzzyContains("abcde"));
            Assert.IsTrue(stringToSearch.FuzzyContains("bcdefgh"));
            Assert.IsTrue(stringToSearch.FuzzyContains(stringToSearch));
        }

        [TestMethod()]
        public void FuzzyContains_Fuzzy_Valid_Terms_Test()
        {
            string stringToSearch = "abcdefghi";

            Assert.IsTrue(stringToSearch.FuzzyContains("ai"));
            Assert.IsTrue(stringToSearch.FuzzyContains("hi"));
            Assert.IsTrue(stringToSearch.FuzzyContains("ah"));
            Assert.IsTrue(stringToSearch.FuzzyContains("bi"));
            Assert.IsTrue(stringToSearch.FuzzyContains("bi"));
            Assert.IsTrue(stringToSearch.FuzzyContains("acdgh"));
            Assert.IsTrue(stringToSearch.FuzzyContains("acefi"));
            Assert.IsTrue(stringToSearch.FuzzyContains("abcghi"));
            Assert.IsTrue(stringToSearch.FuzzyContains("abcdei"));
            Assert.IsTrue(stringToSearch.FuzzyContains("abcdehi"));
            Assert.IsTrue(stringToSearch.FuzzyContains(stringToSearch));
        }

        [TestMethod()]
        public void FuzzyContains_Unicode_Test()
        {
            string stringToSearch = "אבגדהו";

            Assert.IsTrue(stringToSearch.FuzzyContains("בגד"));
            Assert.IsTrue(stringToSearch.FuzzyContains("אב"));
            Assert.IsTrue(stringToSearch.FuzzyContains("אגה"));
            Assert.IsTrue(stringToSearch.FuzzyContains("דה"));
            Assert.IsTrue(stringToSearch.FuzzyContains("אבדו"));
            Assert.IsTrue(stringToSearch.FuzzyContains(stringToSearch));
            Assert.IsFalse(stringToSearch.FuzzyContains("זחט"));
            Assert.IsFalse(stringToSearch.FuzzyContains("שיט"));
        }

        [TestMethod()]
        public void FuzzyContains_Empty_Terms_Test()
        {
            string testString = "abcdefghi";
            string emptyString = "";

            Assert.IsTrue(testString.FuzzyContains(emptyString));
            Assert.IsTrue(emptyString.FuzzyContains(emptyString));
            Assert.IsFalse(emptyString.FuzzyContains(testString));
        }
    }
}
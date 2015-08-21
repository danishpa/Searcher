using Microsoft.VisualStudio.TestTools.UnitTesting;
using Searcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Searcher.Tests
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
            Assert.AreEqual(2, s1.Search(new string[] { "a", "b"}));
            Assert.AreEqual(3, s1.Search(new string[] { "a", "b", "c" }));
            Assert.AreEqual(4, s1.Search(new string[] { "a", "b", "c", "d" }));
            Assert.AreEqual(5, s1.Search(new string[] { "a", "b", "c", "d", "e"}));

            SearchableZeroResultSearch s2 = new SearchableZeroResultSearch();
            Assert.AreEqual(0, s2.Search(new string[] { }));
            Assert.AreEqual(0, s2.Search(new string[] { "a" }));
            Assert.AreEqual(0, s2.Search(new string[] { "a", "b" }));
            Assert.AreEqual(0, s2.Search(new string[] { "a", "b", "c" }));
            Assert.AreEqual(0, s2.Search(new string[] { "a", "b", "c", "d" }));
            Assert.AreEqual(0, s2.Search(new string[] { "a", "b", "c", "d", "e"}));
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
    }
}
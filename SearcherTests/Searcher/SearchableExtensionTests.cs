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
        [TestMethod]
        public void SearchAll_CorrectSearch_2Terms_Test()
        {
            string[] searchTerms = { "c", "m" };
            DynamicPerson p = Common.CreateDynamicPerson("1", "carl", "marks");

            Assert.IsTrue(p.SearchAll(searchTerms));
        }

        [TestMethod]
        public void SearchAll_CorrectSearch_1Term_Test()
        {
            string[] searchTerms = { "car" };
            DynamicPerson p = Common.CreateDynamicPerson("1", "carl", "marks");

            Assert.IsTrue(p.SearchAll(searchTerms));
        }

        [TestMethod]
        public void SearchAll_CorrectSearch_3Term_Test()
        {
            string[] searchTerms = { "car", "ca", "ma" };
            DynamicPerson p = Common.CreateDynamicPerson("1", "carl", "marks");

            Assert.IsTrue(p.SearchAll(searchTerms));
        }

        [TestMethod]
        public void SearchAll_IncorrectSearch_1Term_Test()
        {
            string[] searchTerms = { "bar", };
            DynamicPerson p = Common.CreateDynamicPerson("1", "carl", "marks");

            Assert.IsFalse(p.SearchAll(searchTerms));
        }
    }
}
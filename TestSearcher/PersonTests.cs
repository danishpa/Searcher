using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Searcher;

namespace TestSearcher
{
    [TestClass]
    public class PersonTests
    {
        [TestMethod]
        public void SearchAll_CorrectSearch_2Terms()
        {
            string[] searchTerms = { "c", "m" };
            Person p = new Person("1", "carl", "marks");

            Assert.IsTrue(p.SearchAll(searchTerms));
        }

        [TestMethod]
        public void SearchAll_CorrectSearch_1Term()
        {
            string[] searchTerms = { "car" };
            Person p = new Person("1", "carl", "marks");

            Assert.IsTrue(p.SearchAll(searchTerms));
        }

        [TestMethod]
        public void SearchAll_CorrectSearch_3Term()
        {
            string[] searchTerms = { "car", "ca", "ma" };
            Person p = new Person("1", "carl", "marks");

            Assert.IsTrue(p.SearchAll(searchTerms));
        }

        [TestMethod]
        public void SearchAll_IncorrectSearch_1Term()
        {
            string[] searchTerms = { "bar", };
            Person p = new Person("1", "carl", "marks");

            Assert.IsFalse(p.SearchAll(searchTerms));
        }
    }
}

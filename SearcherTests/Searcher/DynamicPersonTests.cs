using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.CSharp.RuntimeBinder;
using System.Linq;

namespace Searcher.Tests
{
    [TestClass()]
    public class DynamicPersonTests
    {
        [TestMethod()]
        public void TryGetMember_Valid_Test()
        {
            string[] headers = { "A", "B", "C" };
            string[] fields = { "a", "b", "c" };
            
            dynamic p = Common.CreateDynamicPerson(headers, fields);
            Assert.AreEqual(fields[0], p.A);
            Assert.AreEqual(fields[1], p.B);
            Assert.AreEqual(fields[2], p.C);
        }

        [TestMethod()]
        [ExpectedException(typeof(RuntimeBinderException))]
        public void TryGetMember_Invalid_Test()
        {
            string[] headers = { "A", "B", "C" };
            string[] fields = { "a", "b", "c" };
            
            dynamic p = Common.CreateDynamicPerson(headers, fields);
            var d = p.D;
        }

        [TestMethod()]
        public void GetDynamicMemberNames_Test()
        {
            string[] headers = { "A", "B", "C" };
            string[] fields = { "a", "b", "c" };

            DynamicPerson p = Common.CreateDynamicPerson(headers, fields);
            string[] dynamicMemberNames = p.GetDynamicMemberNames().ToArray();

            // Verify arrays are equal
            Assert.AreEqual(headers.Length, dynamicMemberNames.Length);

            // Sort the arrays (in case returned properties are unordered)
            Array.Sort(headers);
            Array.Sort(dynamicMemberNames);

            // Verify the properties are as required
            for (int i = 0; i < headers.Length; ++i)
            {
                Assert.AreEqual(headers[i], dynamicMemberNames[i]);
            }
        }

        [TestMethod()]
        public void Search_CorrectSearch_Some_Terms_Incorrect_Test()
        {
            string[] searchTerms = null;
            DynamicPerson p = Common.CreateDynamicPerson("1", "carl", "marks");

            searchTerms = new string[] { "c", "x" };
            Assert.AreEqual(1, p.Search(searchTerms));

            searchTerms = new string[] { "c", "x", "y" };
            Assert.AreEqual(1, p.Search(searchTerms));

            searchTerms = new string[] { "x", "c", "y" };
            Assert.AreEqual(1, p.Search(searchTerms));
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_Null_SearchTerms_Test()
        {
            string[] searchTerms = null;
            DynamicPerson p = Common.CreateDynamicPerson("1", "carl", "marks");

            searchTerms = null;
            p.Search(searchTerms);
        }

        [TestMethod()]
        public void Search_Empty_Search_Test()
        {
            string[] searchTerms = null;
            DynamicPerson p = Common.CreateDynamicPerson("1", "carl", "marks");

            searchTerms = new string[] { };
            Assert.AreEqual(0, p.Search(searchTerms));
        }

        [TestMethod()]
        public void Search_CorrectSearches_Test()
        {
            string[] searchTerms = null;
            DynamicPerson p = Common.CreateDynamicPerson("1", "carl", "marks");

            searchTerms = new string[] { "c" };
            Assert.AreEqual(1, p.Search(searchTerms));

            searchTerms = new string[] { "c", "m" };
            Assert.AreEqual(2, p.Search(searchTerms));

            searchTerms = new string[] { "c", "m", "carl" };
            Assert.AreEqual(3, p.Search(searchTerms));

            searchTerms = new string[] { "c", "m", "1" };
            Assert.AreEqual(3, p.Search(searchTerms));

            searchTerms = new string[] { "c", "m", "1", "carl" };
            Assert.AreEqual(4, p.Search(searchTerms));

            searchTerms = new string[] { "c", "m", "1", "marks", "carl" };
            Assert.AreEqual(5, p.Search(searchTerms));
        }

        [TestMethod]
        public void SearchAll_CorrectSearch_1Term_Test()
        {
            string[] searchTerms = { "car" };
            DynamicPerson p = Common.CreateDynamicPerson("1", "carl", "marks");
            Assert.IsTrue(p.SearchAll(searchTerms));
        }

        [TestMethod]
        public void SearchAll_CorrectSearch_2Terms_Test()
        {
            string[] searchTerms = { "c", "m" };
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

        [TestMethod]
        public void SearchAll_IncorrectSearch_2Term_Test()
        {
            string[] searchTerms = { "bar", "baz" };
            DynamicPerson p = Common.CreateDynamicPerson("1", "carl", "marks");
            Assert.IsFalse(p.SearchAll(searchTerms));
        }

    }
}
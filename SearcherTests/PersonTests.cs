using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Searcher.Tests
{
    [TestClass()]
    public class PersonTests
    {
        [TestMethod()]
        public void Person_NumberSet_Test()
        {
            string number = "1", firstName = "carl", lastName = "marks";
            string new_value = "new";
            Person p = new Person(number, firstName, lastName);
            
            p.Number = new_value;

            Assert.AreEqual(new_value, p.Number);
        }

        [TestMethod()]
        public void Person_NumberSet_Null_Test()
        {
            string number = "1", firstName = "carl", lastName = "marks";
            string new_value = null;
            Person p = new Person(number, firstName, lastName);

            p.Number = new_value;

            Assert.AreEqual("", p.Number);
        }

        [TestMethod()]
        public void Person_FirstNameSet_Test()
        {
            string number = "1", firstName = "carl", lastName = "marks";
            string new_value = "new";
            Person p = new Person(number, firstName, lastName);

            p.FirstName = new_value;

            Assert.AreEqual(new_value, p.FirstName);
        }

        [TestMethod()]
        public void Person_FirstNameSet_Null_Test()
        {
            string number = "1", firstName = "carl", lastName = "marks";
            string new_value = null;
            Person p = new Person(number, firstName, lastName);

            p.FirstName = new_value;

            Assert.AreEqual("", p.FirstName);
        }

        [TestMethod()]
        public void Person_LastNameSet_Test()
        {
            string number = "1", firstName = "carl", lastName = "marks";
            string new_value = "new";
            Person p = new Person(number, firstName, lastName);

            p.LastName = new_value;

            Assert.AreEqual(new_value, p.LastName);
        }

        [TestMethod()]
        public void Person_LastNameSet_Null_Test()
        {
            string number = "1", firstName = "carl", lastName = "marks";
            string new_value = null;
            Person p = new Person(number, firstName, lastName);

            p.LastName = new_value;

            Assert.AreEqual("", p.LastName);
        }

        [TestMethod()]
        public void Person_Empty_Valid_Test()
        {
            Person p = new Person();
            Assert.AreEqual("", p.Number);
            Assert.AreEqual("", p.FirstName);
            Assert.AreEqual("", p.LastName);
        }

        [TestMethod()]
        public void Person_Valid_Test()
        {
            string number = "1", firstName = "carl", lastName = "marks";
            Person p = new Person(number, firstName, lastName);
            Assert.AreEqual(number, p.Number);
            Assert.AreEqual(firstName, p.FirstName);
            Assert.AreEqual(lastName, p.LastName);
        }

        [TestMethod()]
        public void Person_Valid_Hebrew_Test()
        {
            string number = "מספר", firstName = "חרטא", lastName = "ברטה";
            Person p = new Person(number, firstName, lastName);
            Assert.AreEqual(number, p.Number);
            Assert.AreEqual(firstName, p.FirstName);
            Assert.AreEqual(lastName, p.LastName);
        }

        [TestMethod()]
        public void Person_Null1_Test()
        {
            string number = null, firstName = "", lastName = "";
            Person p = new Person(number, firstName, lastName);
            Assert.AreEqual("", p.Number);
            Assert.AreEqual(firstName, p.FirstName);
            Assert.AreEqual(lastName, p.LastName);
        }

        [TestMethod()]
        public void Person_Null2_Test()
        {
            string number = null, firstName = null, lastName = "";
            Person p = new Person(number, firstName, lastName);
            Assert.AreEqual("", p.Number);
            Assert.AreEqual("", p.FirstName);
            Assert.AreEqual(lastName, p.LastName);
        }

        [TestMethod()]
        public void Person_Null3_Test()
        {
            string number = null, firstName = null, lastName = null;
            Person p = new Person(number, firstName, lastName);
            Assert.AreEqual("", p.Number);
            Assert.AreEqual("", p.FirstName);
            Assert.AreEqual("", p.LastName);
        }

        [TestMethod()]
        public void CopyPerson_Valid_Test()
        {
            string number = "1", firstName = "carl", lastName = "marks";
            Person p1 = new Person(number, firstName, lastName);
            Person p2 = new Person(p1);

            Assert.AreEqual(p1.Number, p2.Number);
            Assert.AreEqual(p1.FirstName, p2.FirstName);
            Assert.AreEqual(p1.LastName, p2.LastName);
        }

        [TestMethod()]
        public void CopyPerson_Valid_Null_Test()
        {
            string number = null, firstName = "carl", lastName = null;
            Person p1 = new Person(number, firstName, lastName);
            Person p2 = new Person(p1);

            Assert.AreEqual(p1.Number, p2.Number);
            Assert.AreEqual(p1.FirstName, p2.FirstName);
            Assert.AreEqual(p1.LastName, p2.LastName);
        }

        [TestMethod]
        public void SearchAll_CorrectSearch_2Terms_Test()
        {
            string[] searchTerms = { "c", "m" };
            Person p = new Person("1", "carl", "marks");

            Assert.IsTrue(p.SearchAll(searchTerms));
        }

        [TestMethod]
        public void SearchAll_CorrectSearch_1Term_Test()
        {
            string[] searchTerms = { "car" };
            Person p = new Person("1", "carl", "marks");

            Assert.IsTrue(p.SearchAll(searchTerms));
        }

        [TestMethod]
        public void SearchAll_CorrectSearch_3Term_Test()
        {
            string[] searchTerms = { "car", "ca", "ma" };
            Person p = new Person("1", "carl", "marks");

            Assert.IsTrue(p.SearchAll(searchTerms));
        }

        [TestMethod]
        public void SearchAll_IncorrectSearch_1Term_Test()
        {
            string[] searchTerms = { "bar", };
            Person p = new Person("1", "carl", "marks");

            Assert.IsFalse(p.SearchAll(searchTerms));
        }
    }
}
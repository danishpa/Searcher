using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Searcher
{
    public class Person
    {
        private string m_number;
        private string m_firstName;
        private string m_LastName;

        public string Number
        {
            get { return m_number; }
            set { m_number = value ?? string.Empty; }
        }
        public string FirstName
        {
            get { return m_firstName; }
            set { m_firstName = value ?? string.Empty; }
        }
        public string LastName
        {
            get { return m_LastName; }
            set { m_LastName = value ?? string.Empty; }
        }

        public Person()
        : this(string.Empty, string.Empty, string.Empty)
        {

        }

        public Person(
            string number,
            string firstName,
            string lastName)
        {
            Number = number;
            FirstName = firstName;
            LastName = lastName;
        }

        public Person(Person copyFrom)
            : this(copyFrom.Number, copyFrom.FirstName, copyFrom.LastName)
        {

        }

        public int Search(string searchTerm)
        {
            string[] properties = new string[] {
                Number,
                FirstName,
                LastName
            };
            return properties.Where(property => property.Contains(searchTerm)).Count();            
        }

        public int Search(string[] searchTerms)
        {
            return searchTerms.Select(searchTerm => Search(searchTerm)).Sum();
        }

        // Returns true if each term in searchTerms appears in the properties. false otherwise
        public bool SearchAll(string[] searchTerms)
        {
            return searchTerms.Select(searchTerm => Search(searchTerm)).All(x => x > 0);
        }
    }
}

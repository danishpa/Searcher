using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Searcher
{
    public class Person
    {
        public string FirstName {get; set; }
        public string LastName {get; set; }
        public string Number { get; set; }

        public Person() { }

        public Person(
            string number,
            string firstName,
            string lastName)
        {
            this.Number = number;
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public Person(Person copyFrom)
            : this(copyFrom.Number, copyFrom.FirstName, copyFrom.LastName)
        {

        }

        public int Search(string searchTerm)
        {
            string[] properties = new string[] {
                this.Number,
                this.FirstName,
                this.LastName
            };

            return properties.Where(property => property.Contains(searchTerm)).Count();            
        }

        public int Search(string[] searchTerms)
        {
            return searchTerms.Select(searchTerm => this.Search(searchTerm)).Sum();
        }

        // Returns true if each term in searchTerms appears in the properties. false otherwise
        public bool SearchAll(string[] searchTerms)
        {
            return searchTerms.Select(searchTerm => this.Search(searchTerm)).All(x => x > 0);
        }

    }
}

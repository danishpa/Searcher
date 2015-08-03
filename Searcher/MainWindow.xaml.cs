using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Controls.Primitives;
using System.Globalization;
using System.Diagnostics;

namespace Searcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Person> originalPersons;

        private void LoadPersonsFromFile(string filePath)
        {
            this.originalPersons = PersonProvider.FromFile(filePath, PersonProviderSupportedFileTypes.CSV);
        }

        private void PopulateDataGrid(List<Person> persons)
        {
            this.SearchResults.ItemsSource = persons;   
        }

        private List<Person> GetPersonsBySearchTerm(string searchTerm)
        {
            // TODO: 
            // 1. Smarter search logic - Fuzzy search - Consecutive letters
            // 2. Searching in english tries to convert characters to hebrew and search
            // 3. But if the searched string is in english, search in english
            // 4. Try to order by relevance (SearchAll should return a float which represents how good is the match)

            string[] whiteSpaces = { " " };
            string[] searchTerms = searchTerm.Split(whiteSpaces, StringSplitOptions.RemoveEmptyEntries);

            // Return all persons, for which all the search terms appear in their properties
            return originalPersons.Where(person => person.SearchAll(searchTerms)).ToList();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Person> filteredPersons = GetPersonsBySearchTerm(this.SearchTextBox.Text);

            this.PopulateDataGrid(filteredPersons);
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.setCorrectCultureInfoForTextBox();
        }

        public void setCorrectCultureInfoForTextBox()
        {
            string cultureString = "he";
            CultureInfo hebrewCultureInfo = null;

            try
            {
                hebrewCultureInfo = CultureInfo.CreateSpecificCulture(cultureString);
                InputLanguageManager.SetInputLanguage(this.SearchTextBox, hebrewCultureInfo);
                Trace.WriteLine(String.Format("Set Culture to {0}", cultureString));
            }
            catch (CultureNotFoundException)
            {
                Trace.WriteLine("Culture {0} not found", cultureString);
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            // Load persons from a file and into the originalPersons
            LoadPersonsFromFile(@"C:\Projects\Searcher\Searcher\samples\people_hebrew.csv");
            
            // First grid population
            PopulateDataGrid(this.originalPersons);
            this.SearchResults.AutoGenerateColumns = true;

            //Future- popup with key combo
            //this.Visibility = Visibility.Hidden;

            // Focus on the textbox to improve UX
            this.SearchTextBox.Focus();
        }


    }
}

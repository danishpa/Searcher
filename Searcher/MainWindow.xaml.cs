using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Searcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Person> originalPersons;
        private CultureInfo hebrewLanguage = null;
        private CultureInfo previousLanguage = null;

        private void LoadPersonsFromFile(string filePath)
        {
            this.originalPersons = PersonProvider.FromFile(filePath);
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

        /* FUTURE: This is quite a complex code, that acheives just about the default behavior. It's here, since in the future we would like
         * To handle cases were the window is hidden, and when it comes back from being hidden, the new input language is hebrew.
         * This must be done programatically */

        /* This checks if the last used input language isn't hebrew, and sets the current input language accordingly */
        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if ((previousLanguage != null) && !previousLanguage.DisplayName.Equals(hebrewLanguage.DisplayName))
            {
                InputLanguageManager.SetInputLanguage(SearchTextBox, previousLanguage);
            }
            else
            {
                InputLanguageManager.SetInputLanguage(SearchTextBox, hebrewLanguage);
            }
        }

        /* This event handler deals with the changing of input language - it remembers the new language, so if the user
         * switched input language, then search box lost focus, when we regain focus, use the new language the user picked. */
        private void Current_InputLanguageChanged(object sender, InputLanguageEventArgs e)
        {
            if (SearchTextBox.IsFocused)
            {
                previousLanguage = e.NewLanguage;
            }
        }

        private void InitializeLanguageParameters()
        {
            string cultureString = "he";
            
            try
            {
                hebrewLanguage = CultureInfo.CreateSpecificCulture(cultureString);
                Trace.WriteLine(String.Format("Set Culture to {0}", cultureString));

                InputLanguageManager.Current.InputLanguageChanged += Current_InputLanguageChanged;
                Trace.WriteLine(String.Format("Added event handler for Input Language Change"));
            }
            catch (CultureNotFoundException)
            {
                Trace.WriteLine("Culture {0} not found", cultureString);
            }
        }


        public MainWindow()
        {
            InitializeComponent();
            InitializeLanguageParameters();

            // Load persons from a file and into the originalPersons
            LoadPersonsFromFile(@"C:\Projects\Searcher\Searcher\samples\people_hebrew.csv");
            
            // First grid population
            PopulateDataGrid(originalPersons);
            SearchResults.AutoGenerateColumns = true;

            //Future- popup with key combo
            //this.Visibility = Visibility.Hidden;

            // Focus on the textbox to improve UX
            SearchTextBox.Focus();
            
        }
    }
}

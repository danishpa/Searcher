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
using System.ComponentModel;

using Searcher;
using Searcher.ViewModel;
using Searcher.Model;
using Searcher.Model.Dynamics;

namespace Searcher.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CultureInfo hebrewLanguage = null;
        private CultureInfo previousLanguage = null;
        private Settings SettingsWindow = null;
        private new SearcherViewModel DataContext; // Hide DataContext

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
        
        private void InitializeSubWindows()
        {
            SettingsWindow = new Settings();
        }

        private void InitializeRunStatus()
        {
            Properties.Settings.Default.FirstRun = false;
        }

        private void InitializePositionWindow()
        {
            double maxHeight = SystemParameters.WorkArea.Bottom;
            double maxWidth = SystemParameters.WorkArea.Right;
            double lastTopPosition = Properties.Settings.Default.LastPosition_Top;
            double lastLeftPosition = Properties.Settings.Default.LastPosition_Left;

            // Verify that the saved parameters are legal (so that the window won't disappear for good)
            bool isLastTopPositionLegal = (0 <= lastTopPosition && lastTopPosition <= maxHeight);
            bool isLastLeftPositionLegal = (0 <= lastLeftPosition && lastLeftPosition <= maxWidth);

            // If the last known positions are OK, and AutoPositionWindow is disabled (because of a manual user position earlier),
            // the last known positions will be restored
            if (!Properties.Settings.Default.AutoPositionWindow && isLastTopPositionLegal && isLastLeftPositionLegal)
            {
                // Legal Last window position
                Top = lastTopPosition;
                Left = lastLeftPosition;
            }
            else
            {
                // Position set to bottom-right of the screen
                Top = maxHeight - Height;
                Left = maxWidth - Width;
            }

            // If any of the last known positions are illigal, reset them!
            if (!isLastTopPositionLegal || !isLastLeftPositionLegal)
            {
                Properties.Settings.Default.LastPosition_Top = 0;
                Properties.Settings.Default.LastPosition_Left = 0;
            }
        }
        
        private void InitializeKeyBindings()
        {
            //! TODO
            // Ctrl+O -> Open location to load persons from
            // Ctrl+R -> Refresh from file
        }

        internal void InitializeDataContext()
        {
            DataContext = new SearcherViewModel();
        }

        public MainWindow()
        {
            InitializeComponent();
            InitializeSubWindows();
            InitializeLanguageParameters();
            InitializeRunStatus();
            InitializePositionWindow();
            InitializeKeyBindings();
            InitializeDataContext();

            // First grid population
            PopulateDataGrid(DataContext.Persons);

            //!! SearchResults.HeadersVisibility = DataGridHeadersVisibility.All;
            SearchResults.AutoGenerateColumns = true;

            //!! Future - popup with key combo
            //!! this.Visibility = Visibility.Hidden;

            // Focus on the textbox to improve UX
            SearchTextBox.Focus();
        }


        private void PopulateDataGrid(List<DynamicPerson> persons)
        {
            SearchResults.ItemsSource = new DynamicList<DynamicPerson>(persons);
        }

        private List<DynamicPerson> GetPersonsBySearchTerm(string searchTerm)
        {
            // TODO: 
            // 1. Smarter search logic - Fuzzy search - Consecutive letters
            // 2. Searching in english tries to convert characters to hebrew and search
            // 3. But if the searched string is in english, search in english
            // 4. Try to order by relevance (SearchAll should return a float which represents how good is the match)

            string[] whiteSpaces = { " " };
            string[] searchTerms = searchTerm.Split(whiteSpaces, StringSplitOptions.RemoveEmptyEntries);

            // Return all persons, for which all the search terms appear in their properties
            return DataContext.Persons.Where(person => person.SearchAll(searchTerms)).ToList();
        }
        private void SaveWindowPositionToProperties(double top, double left)
        {
            Properties.Settings.Default.LastPosition_Top = top;
            Properties.Settings.Default.LastPosition_Left = left;

            Properties.Settings.Default.Save();
        }

        private void SaveWindowPositionToProperties()
        {
            SaveWindowPositionToProperties(Top, Left);
        }

        private void SearchResults_AutoGeneratedColumns(object sender, EventArgs e)
        {
            // Logic:
            // If user resized the window in the past (past = this run before window hidden!!!):
            //    If we have 1-2 entries:
            //        we will increase it to a minimum threshold (1-2 entries)
            // If user never resized the window
            //    Window size corrosponds to number of entries until a maximum (20?) reached
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PopulateDataGrid(GetPersonsBySearchTerm(this.SearchTextBox.Text));
        }

        /* FUTURE: This is quite a complex code, that acheives just about the default behavior. It's here, since in the future we would like
         * To handle cases were the window is hidden, and when it comes back from being hidden, the new input language is hebrew.
         * This must be done programatically */

        // This checks if the last used input language isn't hebrew, and sets the current input language accordingly
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

        private void Window_Closed(object sender, EventArgs e)
        {
            if (SettingsWindow.IsLoaded)
            {
                SettingsWindow.Close();
            }
            
            SaveWindowPositionToProperties();
            Properties.Settings.Default.Save();
        }

        // Handles moving around the window - This code is not very robust.
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Properties.Settings.Default.AutoPositionWindow = false;

                DragMove();
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (!SettingsWindow.IsLoaded)
            {
                SettingsWindow = new Settings();
            }

            if (SettingsWindow.IsVisible)
            {
                SettingsWindow.Hide();
            }
            else
            {
                SettingsWindow.Show();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Searcher.Model
{
    public static class LanguageUtillities
    {
        public static Dictionary<char, char> HebrewEndWordToNormalCharacters =
            new Dictionary<char, char>()
            {
                {'ם', 'מ'},
                {'ן', 'נ'},
                {'ץ', 'צ'},
                {'ף', 'פ'},
                {'ך', 'כ'},
            };

        public static Dictionary<char, char> HebrewEnglishKeys =
            new Dictionary<char, char>()
            {
                {'1', '1'},
                {'2', '2'},
                {'3', '3'},
                {'4', '4'},
                {'5', '5'},
                {'6', '6'},
                {'7', '7'},
                {'8', '8'},
                {'9', '9'},
                {'0', '0'},
                {'-', '-'},
                {'=', '='},
                {'/', 'q'},
                {'q', '/'},
                {'\'', 'w'},
                {'w', '\''},
                {'ק', 'e'},
                {'e', 'ק'},
                {'ר', 'r'},
                {'r', 'ר'},
                {'א', 't'},
                {'t', 'א'},
                {'ט', 'y'},
                {'y', 'ט'},
                {'ו', 'u'},
                {'u', 'ו'},
                {'ן', 'i'},
                {'i', 'ן'},
                {'ם', 'o'},
                {'o', 'ם'},
                {'פ', 'p'},
                {'p', 'פ'},
                {']', '['},
                {'[', ']'},
                {'ש', 'a'},
                {'a', 'ש'},
                {'ד', 's'},
                {'s', 'ד'},
                {'ג', 'd'},
                {'d', 'ג'},
                {'כ', 'f'},
                {'f', 'כ'},
                {'ע', 'g'},
                {'g', 'ע'},
                {'י', 'h'},
                {'h', 'י'},
                {'ח', 'j'},
                {'j', 'ח'},
                {'ל', 'k'},
                {'k', 'ל'},
                {'ך', 'l'},
                {'l', 'ך'},
                {'ף', ';'},
                {';', 'ף'},
                {'ז', 'z'},
                {'z', 'ז'},
                {'ס', 'x'},
                {'x', 'ס'},
                {'ב', 'c'},
                {'c', 'ב'},
                {'ה', 'v'},
                {'v', 'ה'},
                {'נ', 'b'},
                {'b', 'נ'},
                {'מ', 'n'},
                {'n', 'מ'},
                {'צ', 'm'},
                {'m', 'צ'},
                {'ת', ','},
                {',', 'ת'},
                {'ץ', '.'},
                {'.', 'ץ'},
            };

        public static string NormalizeHebrew(string source)
        {
            string result = null;

            if (source == null)
            {
                return null;
            }
            
            result = string.Copy(source);
            foreach (var pair in HebrewEndWordToNormalCharacters)
            {
                result = result.Replace(pair.Key, pair.Value);
            }
            return result;
        }

        public static string NormalizeEnglish(string source)
        {
            return source.ToLower();
        }

        public static string ToNormalizedString(this string source)
        {
            return NormalizeHebrew(NormalizeEnglish(source));
        }

        public static string LangOver(this string source)
        {
            return string.Join<char>(
                string.Empty,
                source.AsEnumerable().Select(c => HebrewEnglishKeys[c]));   
        }
    }
}

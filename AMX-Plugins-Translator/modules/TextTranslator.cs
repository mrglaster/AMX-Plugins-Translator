using System;
using System.Linq;
using System.Threading.Tasks;
using GTranslate.Translators;

namespace DictionaryGenerator.modules
{
    public class TextTranslator
    {

        //List of ALL languages
        public static string[] _allSupportedLanguages =
        {
            "en", "de", "sr", "tr", "fr", "sv", "da", "pl", "nl", "es", "bp", "cz", "fi", "bg", "ro", "hu", "lt", "sk",
            "mk", "hr", "bs", "ru", "cn", "al", "ua", "lv"
        };

        //List of lang codes supported by Google Translator and other services        
        public static string[] translatorCodes = {
            "en", "de", "sr", "tr", "fr", "sv", "da", "pl", "nl", "es", "pt", "cs", "fi", "bg", "ro", "hu", "lt", "sk",
            "mk", "hr", "bs", "ru", "zh", "sq", "uk", "lv"
        };
        
        /**Checks if input language is supported*/
        public static bool isSupportedLanguage(string language)
        {
            return _allSupportedLanguages.Contains(language);
        }

        /**Translates input string to defined language*/
        public static string getTranslatedText(string sourceText, string resultLanguage)
        {
            if (!isSupportedLanguage(resultLanguage)) throw new Exception($"Language {resultLanguage} is not supported by AMXMODX!");
            if (sourceText.Length == 0) throw new Exception("Unable to process 0-length string!");
            string translatorLang =
                translatorCodes[PluginCodeAnalyzer.getIdByValue(_allSupportedLanguages, resultLanguage)];
            string result = processTranslationApi(sourceText, translatorLang).Result + " ";
            return result.Replace("% с", " %s ").Replace("% д", " %d ").Replace("% и", " %i ").Replace(" %с ", " %s ").Replace( "%д ", " %d ").Replace(" %и ", " %i ");
        }
        
        /**Invokes translation API and translates text*/
        public static async Task<string> processTranslationApi(string sourceText, string outputLang)
        {
            var translator = new AggregateTranslator();
            var result = await translator.TranslateAsync(sourceText, outputLang);
            return result.Translation;
        }
    }
}

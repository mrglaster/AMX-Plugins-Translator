using System;
using System.Linq;
using System.Net.Mime;

namespace DictionaryGenerator.modules
{
    public class UtilityInputsChecker
    {
        /**Checks if input file's extension is .sma*/
        public static bool isSma(string fileName)
        {
            return fileName.EndsWith(".sma");
        }
        
        /**Checks if input translation langs list is ok*/
        public static void langSupportCheck(string[] langList)
        {
            if (langList.Length != 0)
            {
                for (int i = 0; i < langList.Length; i++)
                {
                    if (!TextTranslator.isSupportedLanguage(langList[i]))
                        throw new ArgumentException($"Language {langList[i]} is not supported by AMXMODX!");
                }
            }
        }
        
    }
}
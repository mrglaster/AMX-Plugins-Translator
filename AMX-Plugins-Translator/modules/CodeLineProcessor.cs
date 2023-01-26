using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace DictionaryGenerator.modules
{
    public static class CodeLineProcessor
    {
        
        /**Converts string with message to Amxmodx Dictionary string format*/
        public static string generateDictionaryLine(string messageText)
        {    
            //If length of the message is 0, then something went wrong before
            if (messageText.Length == 0) throw new Exception("Unable to process string with length = 0!");
            
            string resultString = messageText.Replace("%d", " ").Replace("%s", " ").Replace("%i", " ");
            
            
            //Converting text to upper case
            resultString = resultString.ToUpper();
            
            //Replacing all non-alphabet symbols
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            
            //Replacing excess spaces
            resultString = rgx.Replace(resultString, "");
            resultString = Regex.Replace(resultString, @"\s+", " ").Replace(' ', '_');
            if (resultString.EndsWith("_")) resultString = resultString.Remove(resultString.Length - 1);
            return resultString;

        }
        
        /**Checks if id of detected substring is vlaid*/
        public static bool _isValidId(int id)
        {
            return id >= 0 & id <= PluginCodeAnalyzer._codeLineIdentifiers.Length;
        }


        public static String getHardcodedFullPart(string codeLine)
        {
            if (!PluginCodeAnalyzer.isRequiredLine(codeLine))
                throw new Exception("Attempt to get Hardcoded String from code line uncorresponding requirements!");
            var reg = new Regex("\".*?\"");
            var matches = reg.Matches(codeLine);
            var result = matches[0].ToString();
            return result;
        }
        
        /**Returns Hardcoded Text Contained by the Code Line*/
        public static string getHardcodedString(string codeLine)
        {
            return getHardcodedFullPart(codeLine).Substring(1, getHardcodedFullPart(codeLine).Length -2);

        }
        
        
    }
}
using System;
using System.IO;
using System.Linq;

namespace DictionaryGenerator.modules
{
    public class PluginCodeAnalyzer
    {    
        
        //List of Code Lines Identifiers utility will process 
        public static string[] _codeLineIdentifiers =
        {
            "client_print",
            "client_print_color",
            "console_print",
            "show_hudmessage",
            "show_dhudmessage",
            "server_print",
            "engclient_print",
            "log_message",
            "log_amx",
            "log_to_file",
            "menu_create",
            "menu_additem"
        };

        
        /**Checks if Current Plugin Code Line contains required command/identifier */
        public static bool isRequiredLine(string codeLine)
        {
            if (hasDictionaryReference(codeLine) || !hasHardcodedText(codeLine)) return false;
            
            for (int i = 0; i < _codeLineIdentifiers.Length; i++)
            {
                if (codeLine.Contains(_codeLineIdentifiers[i])) return true;
            }
            return false;
        }
        
        /**Returns element's position in the array by element value*/
        public static int getIdByValue(Array array, string identifier)
        {
            return Array.IndexOf(array, identifier);
        }
        
        
        /**Checks if current code line contains hardcoded string attributes and besides, */
        public static bool hasHardcodedText(string codeLine)
        {
            return codeLine.Count(x => x == '"') >= 2;
        }
        
        /**Checks if code line already contains reference to Languages Dictionary*/
        public static bool hasDictionaryReference(string codeLine)
        {
            return codeLine.Contains("%L");
        }

      
        



    }
}

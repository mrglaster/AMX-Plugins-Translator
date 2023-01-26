using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DictionaryGenerator.modules;

namespace DictionaryGenerator
{
    internal class Program
    {
        
        
        public static void Main(string[] args)
        {
            
            if (args.Length == 0) throw new Exception('\n'+ "Arguments not found!" + '\n' + '\n' + "Requested Arguments: " + '\n' + "1) Path to plugin" + '\n' + "2) Language of plugin [check AMXX supported languages]" + '\n' + "3) List of languages you want translate to (optional). If empty, plugin will be translated to every supported language" + '\n' + '\n'  +"Example: AMX-Plugins-Translator.exe myplugin.sma " + "en " + '"' + "en, ru, de" + '"' +'\n' + '\n' );
            if (args.Length > 3) throw new Exception("Too many arguments!");
            modules.DictionaryGenerator dg;
            if (args.Length >= 2)
            {
                string path = "";
                string langList;
                string sourceLang;
                try
                {
                    path = args[0];
                    if (!path.EndsWith(".sma") || !File.Exists(path)) throw new Exception($"Unknown path: {path}");
                    
                }
                catch {
                    throw new Exception($"Can't read file from argument list!");
                }
                
                try
                {
                    sourceLang = args[1];
                    if (!TextTranslator._allSupportedLanguages.Contains(sourceLang))
                    {
                        throw new Exception($"Error! Language {sourceLang} is not supported!");
                    }
                    
                }
                catch {
                    throw new Exception($"Can't read source language!");
                }

                if (args.Length == 3)
                {
                    try
                    {
                        langList = args[2];
                        List<string> listOfLangs = new List<string>(langList.Replace(" ", "").Split(","));
                        if (listOfLangs.Count == 0) throw new Exception("List of Languages not Found!");
                        for (int i = 0; i < listOfLangs.Count; i++) if (!TextTranslator.isSupportedLanguage(listOfLangs[i])) throw new Exception($"Unknown Language: {listOfLangs[i]}");

                        if (listOfLangs.Contains(sourceLang)) listOfLangs.Remove(sourceLang);
                        Console.WriteLine(" ");
                        Console.WriteLine(" ");
                        Console.WriteLine($"Detected {listOfLangs.Count} Languages: {listOfLangs.Aggregate((a, b) => a + ", " + b)}");
                        dg = new modules.DictionaryGenerator(path, sourceLang, listOfLangs.ToArray());
                    }
                    catch {
                        throw new Exception("Some error occured during reading of languages list!");
                    }
                }
                else
                {    
                    Console.WriteLine(" ");
                    Console.WriteLine(" ");
                    Console.WriteLine($"Detected {TextTranslator._allSupportedLanguages.Length} Languages: {TextTranslator._allSupportedLanguages.Aggregate((a, b) => a + ", " + b)}");
                    dg = new modules.DictionaryGenerator(path, sourceLang);
                }

                dg.handlePlugin();
                
            }
        }
    }
}
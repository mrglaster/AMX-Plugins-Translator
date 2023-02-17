using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static DictionaryGenerator.modules.UtilityInputsChecker;

namespace DictionaryGenerator.modules
{    
    
    public class DictionaryGenerator
    {
        private Dictionary<String, Dictionary<String, String>> pairsContainer;
        
        //Name of input sma file
        private string fileName;


        
        //Default language of the plugin
        private string sourceLanguage = "en";


        
        //List of languages for plugin translate (as the default option, plugin'll be translated to all supported languages) 
        private String[] langsTranslateTo = TextTranslator._allSupportedLanguages;
        
        
        /**Returns name for dictionary text file by plugin's file name*/
        public static string generateDictionaryName(string pluginFileName)
        {
            string dictName = pluginFileName;
            dictName = Path.GetFileName(dictName).Replace(".sma", ".txt");
            return dictName;
        }
        
        /**Generates code line for dictionary registration*/
        public string generateDictionaryInitLine(string scriptPath)
        {
            return "register_dictionary(" + '"' + generateDictionaryName(scriptPath) + '"' + ");";
        }

        
        
        
        /**Constrictor for cases when output langs list is given */
        public DictionaryGenerator(string filenameI, string sourceLangI, string[] outputLangsI){
            
            //Input File Check
            if (!isSma(filenameI)) {
                throw new ArgumentException("Input file is not a AMXMODX Script File!");
            }
            
            //Supported Output Languages Array check
            Debug.WriteLine("Output Languages List Check...");
            langSupportCheck(new []{sourceLangI});
            this.sourceLanguage = sourceLangI;
            langSupportCheck(outputLangsI);
            this.fileName = filenameI;
            if (outputLangsI.Length == 0)
            {
                Console.WriteLine("Custom output languages list is empty, so we'll use default languages list");
                this.langsTranslateTo = TextTranslator._allSupportedLanguages;
            } else {
                Console.WriteLine("Problems not found!");
                this.langsTranslateTo = outputLangsI;
            }
        }
        
        
        /**Constructor of the class for default output lang list usage*/
        public DictionaryGenerator(string fileNameI, string sourceLangI)
        {
            if (!isSma(fileNameI))
            {
                throw new ArgumentException("Input file is not a AMXMODX Script File!");
            }
            langSupportCheck(new []{sourceLangI});
            Debug.WriteLine("There is no user-given output languages list, so we'll use DEFAULT setting: ALL SUPPORTED LANGS");
            this.fileName = fileNameI;
            this.sourceLanguage = sourceLangI;
            this.langsTranslateTo = TextTranslator._allSupportedLanguages;

            
        }
        
        

        /**Generates the name for output script*/
        public string generateOutputScriptName(string sourcePath)
        {
            if (!File.Exists(sourcePath)) throw new Exception($"Error! Input file {sourcePath} not found!");
            if (!isSma(sourcePath)) throw new Exception($"Input file {sourcePath} is not an AMX Script path!");
            string dirName = Path.GetDirectoryName(sourcePath)+'\\';
            string fileName = Path.GetFileNameWithoutExtension(sourcePath)+"_translated.sma";
            return dirName + fileName;
        }


        /**Replaces Hardcoded Strings to Dictionary References + Creates Dictionary Identifyers*/
        public void generateHarcodelessPlugin()
        {    
            Console.WriteLine("Replacing plugin's hardcoded strings to Dictionary References");
            Dictionary<String, Dictionary<String, String>> translationContentContainer = new Dictionary<string, Dictionary<string, string>>();
            if (langsTranslateTo.Contains(sourceLanguage)) langsTranslateTo = langsTranslateTo.Where(val => val != sourceLanguage).ToArray();
            for (int i = 0; i < langsTranslateTo.Length; i++)
                try
                {
                    translationContentContainer.Add(langsTranslateTo[i], new Dictionary<string, string>());
                }
                catch 
                {
                    // ignored
                }

            translationContentContainer.Add(sourceLanguage, new Dictionary<string, string>());
            
            
            var outputScriptFileName = generateOutputScriptName(this.fileName);
            if (File.Exists(outputScriptFileName)) File.Delete(outputScriptFileName);
            
            using (StreamWriter sw = File.CreateText(outputScriptFileName))
            {
                sw.WriteLine(
                    "//Plugins was translated with AMX Plugin Translator: https://github.com/mrglaster/AMX-Plugin-Translator");


                foreach (var codeLine in File.ReadAllLines(this.fileName))
                {
                    string lineCopy = codeLine;
                    if (lineCopy.Contains("public plugin_init() {"))
                        lineCopy = lineCopy + '\n' + generateDictionaryInitLine(this.fileName) + '\n';
                    
                    if (PluginCodeAnalyzer.isRequiredLine(lineCopy))
                    {
                        
                        var hardcoded = CodeLineProcessor.getHardcodedString(lineCopy);
                        if (hardcoded.Length != 0)
                        {
                            
                            var hardcodedEnglish = "";
                            if (this.sourceLanguage != "en")
                            {
                                Console.WriteLine($"Generating Dictionary Key for line:  {hardcoded}");
                                hardcodedEnglish = TextTranslator.getTranslatedText(hardcoded, "en");
                            }
                            else hardcodedEnglish = hardcoded;
        
                            var dicName = CodeLineProcessor.generateDictionaryLine(hardcodedEnglish);
                       
                            translationContentContainer[sourceLanguage][dicName] = hardcoded.Replace(@"\", "ы").Replace("ыr", " ").Replace("ыw", " ");
                        
                            if (!lineCopy.Contains("menu_create") && !lineCopy.Contains("menu_additem") &&
                                !lineCopy.Contains("menu_setprop"))
                            {
                                string replacebleString = '"' + "%L" + '"' + ',' + " LANG_PLAYER," + '"' + dicName + '"';
                                lineCopy = lineCopy.Replace(CodeLineProcessor.getHardcodedFullPart(lineCopy),
                                    replacebleString);
                            }

                            string quoteLine = '"'.ToString();
                            Console.WriteLine("COPY: " + lineCopy);
                            if (lineCopy.Contains("menu_create") && lineCopy.Count(x => quoteLine.Contains(x)) >= 4)
                            {
                                sw.WriteLine("new szStringBuf[64]");
                                sw.WriteLine($"formatex(szStringBuf, charsmax(szStringBuf), " + '"' + "%L" + '"' +
                                             $", LANG_PLAYER," + '"' + dicName + '"' + ");");
                                lineCopy = lineCopy.Replace(CodeLineProcessor.getHardcodedFullPart(lineCopy),
                                    "szStringBuf");
                            }

                            if (lineCopy.Contains("menu_additem"))
                            {
                                sw.WriteLine("formatex(szStringBuf, charsmax(szStringBuf)," + '"' + "%L" + '"' +
                                             $", LANG_PLAYER," + '"' + dicName + '"' + ");");
                                lineCopy = lineCopy.Replace(CodeLineProcessor.getHardcodedFullPart(lineCopy), "szStringBuf");
                            }

                        }

                        lineCopy = lineCopy.Replace(@"\w", "").Replace(@"\s", "").Replace(@"\y", "");
                        sw.WriteLine(lineCopy);
                    }
                }
                this.pairsContainer = translationContentContainer;
            }
        }
        
        public void handlePlugin()
        {
            generateHarcodelessPlugin();
            var pluginDictionaryKeys = pairsContainer[sourceLanguage].Keys;
            var pluginDictionaryValues = pairsContainer[sourceLanguage].Values;
            using (StreamWriter sw = File.CreateText(Path.GetDirectoryName(fileName)+ "\\" +generateDictionaryName(fileName)))
            {
                Console.WriteLine("Writing Dictionary on the Source Language");
                Console.WriteLine(" ");
                
                //Write to dictionary source language
                sw.WriteLine('['+sourceLanguage+']');
                for (int i = 0; i < pluginDictionaryKeys.Count; i++)
                    sw.WriteLine($"{pluginDictionaryKeys.ElementAt(i)} = {pluginDictionaryValues.ElementAt(i)}");
                sw.WriteLine(" ");
                
                
                //Translation Loops
                for (int i = 0; i < langsTranslateTo.Length; i++)
                {
                    if (langsTranslateTo[i] != sourceLanguage) {
                        Console.WriteLine($"Translating plugin to: {langsTranslateTo[i]}");
                        Console.WriteLine(" ");
                        sw.WriteLine('['+langsTranslateTo[i]+']');
                        for (int j = 0; j < pluginDictionaryValues.Count; j++)
                        {    
                            Console.WriteLine($"Progress: {(int)((double)j / (double)pluginDictionaryValues.Count * 100)}%");
                            sw.WriteLine($"{pluginDictionaryKeys.ElementAt(j)} = {TextTranslator.getTranslatedText(pluginDictionaryValues.ElementAt(j),langsTranslateTo[i])}");
                        }
                        Console.WriteLine("Progress: 100%");
                        Console.WriteLine(" ");
                        sw.WriteLine(" ");
    
                    }
                    
                }
                Console.WriteLine("Dictionary Was Successfully Generated!");
                
            }
              
        }
    }
}

    

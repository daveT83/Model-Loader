using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Model_Loader.Infrastructure
{
    public class GenerateFlatFile
    {
        /// <summary>
        /// Generates a flat file from a model(s)
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="delimeter"></param>
        /// <param name="typeConverter"></param>
        /// <param name="models"></param>
        /// <param name="type"></param>
        /// <param name="parentClass"></param>
        /// <param name="isHaveHeaders"></param>
        /// <param name="isHaveQuotes"></param>
        /// <returns></returns>
        public static List<string> WriteToFile<T>(string filePath, char delimeter, dynamic typeConverter, IEnumerable<T> mods, bool isHaveHeaders = true, bool isHaveQuotes = true, List<string> headers = null)
        {
            List<string> lines = new List<string>();
            List<T> models = mods.ToList();

            if (models.Count > 0)
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    if (isHaveHeaders)
                    {
                        sw.WriteLine(GenerateHeaders(DictionaryCreator.CreateFromModel<T>(models[0], typeConverter), delimeter, headers));
                    }

                    foreach (Object model in models)
                    {
                        sw.WriteLine(GenerateLine(DictionaryCreator.CreateFromModel<T>(model, typeConverter), delimeter, isHaveQuotes, headers));
                    }
                }
            }

            return lines;
        }

        /// <summary>
        /// Generates a flat file from dictionaries
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="delimeter"></param>
        /// <param name="models"></param>
        /// <param name="isHaveHeaders"></param>
        /// <param name="isHaveQuotes"></param>
        /// <returns></returns>
        public static List<string> WriteToFile(string filePath, char delimeter, List<Dictionary<string, string>> models, bool isHaveHeaders = true, bool isHaveQuotes = true)
        {
            List<string> lines = new List<string>();

            if (models.Count > 0)
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    if (isHaveHeaders)
                    {
                        sw.WriteLine(GenerateHeaders(models[0], delimeter));
                    }

                    foreach (Dictionary<string, string> model in models)
                    {
                        sw.WriteLine(GenerateLine(model, delimeter, isHaveQuotes));
                    }
                }
            }

            return lines;
        }

        /// <summary>
        /// Generates the header line from a dictionary
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="delimeter"></param>
        /// <returns></returns>
        private static string GenerateHeaders(Dictionary<string, string> dict, char delimeter, List<string> headers = null)
        {
            string line = "";

            foreach (string key in dict.Keys)
            {
                if (headers == null || headers.Contains(key))
                {
                    line += key + delimeter;
                }
            }

            return line.TrimEnd(delimeter);
        }

        /// <summary>
        /// Generates a line from a dictionary
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="delimeter"></param>
        /// <returns></returns>
        private static string GenerateLine(Dictionary<string, string> dict, char delimeter, bool isQuotes, List<string> headers = null)
        {
            string line = "";

            foreach (string key in dict.Keys)
            {
                if (headers == null || headers.Contains(key))
                {
                    if (isQuotes)
                    {
                        line += '"' + dict[key] + '"' + delimeter;
                    }
                    else
                    {
                        line += dict[key] + delimeter;
                    }
                }
            }

            return line.TrimEnd(delimeter);
        }
    }
}

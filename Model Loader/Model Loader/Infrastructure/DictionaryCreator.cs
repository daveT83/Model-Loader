using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Model_Loader.Infrastructure
{
    public class DictionaryCreator
    {
        /// <summary>
        /// Creates a dictionary from arguments
        /// Example argument:
        /// Title=New Title
        /// </summary>
        /// <param name="args"></param>
        /// <param name="seperator"></param>
        /// <returns></returns>
        public static Dictionary<string, string> CreateFromArguments(string[] args, string seperator, bool isTrim = true)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (string arg in args)
            {
                string[] argSplit = GetArgument(arg, seperator);

                if (isTrim)
                {
                    dictionary.Add(argSplit[0].Trim(), argSplit[1].Trim());
                }
                else
                {
                    dictionary.Add(argSplit[0], argSplit[1]);
                }
            }

            return dictionary;
        }

        /// <summary>
        /// Creates a dictionary from a flatfile
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="seperator"></param>
        /// <param name="isTrim"></param>
        /// <returns></returns>
        public static List<Dictionary<string, string>> CreateFromFlatFile(string filePath, char seperator, bool isTrim = true)
        {
            List<Dictionary<string, string>> dictionaries = new List<Dictionary<string, string>>();

            using (StreamReader sr = new StreamReader(filePath))
            {
                string line = "";
                List<string> headers = SplitLine(sr.ReadLine(), seperator);

                while ((line = sr.ReadLine()) != null)
                {
                    List<string> lineSplit = SplitLine(line, seperator);
                    Dictionary<string, string> dict = new Dictionary<string, string>();

                    for (int i = 0; i < headers.Count; i++)
                    {
                        if (isTrim)
                        {
                            dict.Add(headers[i].Trim(), lineSplit[i].Trim());
                        }
                        else
                        {
                            dict.Add(headers[i], lineSplit[i]);
                        }
                    }
                    dictionaries.Add(dict);
                }
            }

            return dictionaries;
        }

        /// <summary>
        /// Creates a dictionary from a flatfile
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="seperator"></param>
        /// <param name="headers"></param>
        /// <param name="isTrim"></param>
        /// <returns></returns>
        public static List<Dictionary<string, string>> CreateFromFlatFile(string filePath, char seperator, List<string> headers, bool isTrim = true)
        {
            List<Dictionary<string, string>> dictionaries = new List<Dictionary<string, string>>();

            using (StreamReader sr = new StreamReader(filePath))
            {
                string line = "";

                while ((line = sr.ReadLine()) != null)
                {
                    List<string> lineSplit = SplitLine(line, seperator);
                    Dictionary<string, string> dict = new Dictionary<string, string>();

                    for (int i = 0; i < headers.Count; i++)
                    {
                        if (i >= lineSplit.Count)
                        {
                            if (isTrim)
                            {
                                dict.Add(headers[i].Trim(), "");
                            }
                            else
                            {
                                dict.Add(headers[i], "");
                            }
                        }
                        else
                        {
                            if (isTrim)
                            {
                                dict.Add(headers[i].Trim(), lineSplit[i].Trim());
                            }
                            else
                            {
                                dict.Add(headers[i], lineSplit[i]);
                            }
                        }
                    }
                    dictionaries.Add(dict);
                }
            }

            return dictionaries;
        }

        /// <summary>
        /// Creates a dictionary from a model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="type"></param>
        /// <param name="parentclass"></param>
        /// <param name="typeConverter"></param>
        /// <returns></returns>
        public static Dictionary<string, string> CreateFromModel<T>(Object model, TypeConverter typeConverter)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (PropertyInfo propertyInfo in properties)
            {
                if (propertyInfo.GetValue(model) == null)
                {
                    dict.Add(propertyInfo.Name, "");
                }
                else
                {
                    if (propertyInfo.PropertyType.GetGenericArguments().Length > 0 && propertyInfo.PropertyType.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                    {
                        MethodInfo genericMethod = typeConverter.GetType().GetMethod("ConvertFromIEnumerableType").MakeGenericMethod(propertyInfo.PropertyType.GetGenericArguments()[0]);

                        dict.Add(propertyInfo.Name, (string)genericMethod.Invoke(typeConverter, new object[] { propertyInfo.GetValue(model) }));
                    }
                    else
                    {
                        dict.Add(propertyInfo.Name, typeConverter.ConvertFromType<dynamic>(propertyInfo.GetValue(model)));
                    }
                }
            }

            return dict;
        }

        private static IEnumerable<dynamic> GetIEnumerable(PropertyInfo property, dynamic model)
        {
            return property.GetValue(model);
        }

        private static List<dynamic> GetList(dynamic model)
        {
            return model;
        }

        /// <summary>
        /// Splits a line into a list of strings
        /// </summary>
        /// <param name="line"></param>
        /// <param name="seperator"></param>
        /// <returns></returns>
        private static List<string> SplitLine(string line, char seperator)
        {
            List<string> values = new List<string>();

            if (line != null)
            {
                char[] chars = line.ToCharArray();
                string value = "";
                bool isInQuotes = false;

                foreach (char c in chars)
                {
                    if (c.Equals('"'))
                    {
                        isInQuotes = !isInQuotes;
                    }
                    else if (c.Equals(seperator) && !isInQuotes)
                    {
                        values.Add(value);
                        value = "";
                    }
                    else
                    {
                        value += c;
                    }
                }
                values.Add(value);
            }
            return values;
        }

        /// <summary>
        /// Splits the argument into the key and value
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="delimeter"></param>
        /// <returns></returns>
        private static string[] GetArgument(string argument, string delimeter)
        {
            char[] chars = argument.ToCharArray();
            List<char> stringtoLookFor = new List<char>();
            string key = "";
            string value = "";
            bool isValueFound = false;
            int i = 0;

            foreach (char letter in chars)
            {
                if (i < delimeter.Length)
                {
                    stringtoLookFor.Insert(0, letter);
                }
                else if (!isValueFound)
                {
                    stringtoLookFor.Insert(0, letter);
                    stringtoLookFor.RemoveAt(delimeter.Length);
                }

                if (!isValueFound && delimeter.Equals(String.Join("", stringtoLookFor)))
                {
                    key += letter;
                    isValueFound = true;
                }

                else if (isValueFound)
                {
                    value += letter;
                }
                else
                {
                    key += letter;
                }

                i++;
            }
            return new string[] { key.Replace(delimeter, ""), value };

        }
    }
}

using Model_Loader.Infrastructure;
using Model_Loader_Examples.Examples;
using Model_Loader_Examples.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Model_Loader_Examples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Stopwatch sw = new Stopwatch();

                sw.Start();
                InitJITCompiler();
                sw.Stop();
                Console.WriteLine("Init JIT: " + sw.ElapsedMilliseconds);

                //Load from Arguements
                string delimeter = ":";
                string[] exampleArgs = { "IntExample" + delimeter + "1", "StringExample          " + delimeter + "     String Test          ", "BoolExample" + delimeter + "false", "CharArrayExample" + delimeter + "char array test", "CharExample" + delimeter + "a", "DecimalExample" + delimeter + "2.11", "DoubleExample" + delimeter + "5.55555", "FloatExample" + delimeter + "9.876", "UIntExample" + delimeter + "69", "LongExample" + delimeter + "10000", "ULongExample" + delimeter + "1111111", "ShortExample" + delimeter + "30", "UShortExample" + delimeter + "33","ListExample"+delimeter+"1,1,2,2,3,4,5,6","DateTimeExample"+delimeter+"01/02/2022" };

                sw.Restart();

                ExampleModel exampleModelArguements = Example.LoadFromArguemnts(exampleArgs, delimeter);

                sw.Stop();
                Console.WriteLine("Load From Arguments: "+sw.ElapsedMilliseconds);
                
                //--------------------------------------------------------------------------------------------------------------------
                //Load from Flat Files
                string path = AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug", "ExampleFiles");
                string withQuotesFilePath = Path.Combine(path, "WithQuotes.csv");
                string withoutQuotesFilePath = Path.Combine(path, "WithoutQuotes.csv");
                char fileDelimeter = ',';

                sw.Restart();

                List<ExampleModel> exampleModelsWithQuotes = Example.LoadFromFile(withQuotesFilePath, fileDelimeter);
                sw.Stop();
                Console.WriteLine("Loading from File With Quotes: " + sw.ElapsedMilliseconds);

                sw.Restart();

                List<ExampleModel> exampleModelsWithoutQuotes = Example.LoadFromFile(withoutQuotesFilePath, fileDelimeter);
                sw.Stop();
                Console.WriteLine("Loading from File Without Quotes: " + sw.ElapsedMilliseconds);

                //--------------------------------------------------------------------------------------------------------------------
                //Export to Dictionary

                ExampleModel exampleModel = new ExampleModel();
                exampleModel.BoolExample = false;
                exampleModel.CharArrayExample = new char[] { 'a', 'b', 'c' };
                exampleModel.CharExample = 'c';
                exampleModel.DecimalExample = 2.2M;
                exampleModel.DoubleExample = 2.3434;
                exampleModel.FloatExample = 1.11f;
                exampleModel.IntExample = 7;
                exampleModel.LongExample = 23454325;
                exampleModel.ShortExample = 45;
                exampleModel.UIntExample = 3;
                exampleModel.ULongExample = 67;
                exampleModel.UShortExample = 44;
                exampleModel.StringExample = "String example";
                exampleModel.ListExample = new List<int> { 5, 6, 7 };

                sw.Restart();


                Dictionary<string, string> exampleDictionary = Example.GetDictionaryFromModel(exampleModel);

                sw.Stop();
                Console.WriteLine("Get Dictionary From Model: "+sw.ElapsedMilliseconds);

                //--------------------------------------------------------------------------------------------------------------------
                //Write to CSV file
                string file = @"C:\Users\davet\Desktop\Test.csv";
                List<ExampleModel> models = new List<ExampleModel>();
                for (int i = 0; i < 5; i++)
                {
                    models.Add(exampleModel);
                }

                sw.Restart();

                Example.WriteToFile(file, fileDelimeter, models);

                sw.Stop();
                Console.WriteLine("Write 5 Models to File: "+sw.ElapsedMilliseconds);

                //--------------------------------------------------------------------------------------------------------------------
                //CustomTypeConverter
                sw.Restart();

                Example.OverridingTypeConverter();
                sw.Stop();
                Console.WriteLine("Overriding example: "+sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }

        public static void InitJITCompiler()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug", "ExampleFiles");
            string withQuotesFilePath = Path.Combine(path, "WithQuotes.csv");
            string delimeter = ":";
            string[] exampleArgs = { "IntExample" + delimeter + "1", "StringExample          " + delimeter + "     String Test          ", "BoolExample" + delimeter + "false", "CharArrayExample" + delimeter + "char array test", "CharExample" + delimeter + "a", "DecimalExample" + delimeter + "2.11", "DoubleExample" + delimeter + "5.55555", "FloatExample" + delimeter + "9.876", "UIntExample" + delimeter + "69", "LongExample" + delimeter + "10000", "ULongExample" + delimeter + "1111111", "ShortExample" + delimeter + "30", "UShortExample" + delimeter + "33", "ListExample" + delimeter + "1,1,2,2,3,4,5,6", "DateTimeExample" + delimeter + "01/02/2022" };

            Example.LoadFromArguemnts(exampleArgs, delimeter);
            Example.LoadFromFile(withQuotesFilePath, ':');
            Example.GetDictionaryFromModel(new ExampleModel());
            Example.WriteToFile(@"C:\Users\davet\Desktop\Test.csv", ':', new List<ExampleModel>() { new ExampleModel()});
            Example.OverridingTypeConverter();

        }
    }
}

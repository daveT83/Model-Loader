using Model_Loader_Examples.Examples;
using Model_Loader_Examples.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                //Load from Arguements
                string delimeter = "++---++";
                string[] exampleArgs = { "IntExample" + delimeter + "1", "StringExample          " + delimeter + "     String Test          ", "BoolExample" + delimeter + "false", "CharArrayExample" + delimeter + "char array test", "CharExample" + delimeter + "a", "DecimalExample" + delimeter + "2.11", "DoubleExample" + delimeter + "5.55555", "FloatExample" + delimeter + "9.876", "UIntExample" + delimeter + "69", "LongExample" + delimeter + "10000", "ULongExample" + delimeter + "1111111", "ShortExample" + delimeter + "30", "UShortExample" + delimeter + "33","ListExample"+delimeter+"1,1,2,2,3,4,5,6","DateTimeExample"+delimeter+"01/02/2022" };
                ExampleModel exampleModelArguements = Example.LoadFromArguemnts(exampleArgs, delimeter);

                //--------------------------------------------------------------------------------------------------------------------
                //Load from Flat Files
                string path = AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug", "ExampleFiles");
                string withQuotesFilePath = Path.Combine(path, "WithQuotes.csv");
                string withoutQuotesFilePath = Path.Combine(path, "WithoutQuotes.csv");
                char fileDelimeter = ',';

                List<ExampleModel> exampleModelsWithQuotes = Example.LoadFromFile(withQuotesFilePath, fileDelimeter);
                List<ExampleModel> exampleModelsWithoutQuotes = Example.LoadFromFile(withoutQuotesFilePath, fileDelimeter);

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

                Dictionary<string, string> exampleDictionary = Example.GetDictionaryFromModel(exampleModel);

                //--------------------------------------------------------------------------------------------------------------------
                //Write to CSV file
                string file = @"C:\Users\(yourUser)\Desktop\Test.csv";
                List<ExampleModel> models = new List<ExampleModel>();
                for (int i = 0; i < 5; i++)
                {
                    models.Add(exampleModel);
                }

                Example.WriteToFile(file, fileDelimeter, models);

            }
            catch (Exception ex)
            {
                string s = "";
            }
        }
    }
}

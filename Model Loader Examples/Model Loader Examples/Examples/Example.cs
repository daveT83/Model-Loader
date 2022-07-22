using Model_Loader_Examples.Models;
using Model_Loader.Infrastructure;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace Model_Loader_Examples.Examples
{
    public class Example
    {
        public static ExampleModel LoadFromArguemnts(string[] args, string delimeter)
        {
            //Loads the arguements with the associated variable name into a dictionary<string,string>
            bool isTrim = true; //use this if you do not want to trim the values(false) read in from the arguements. By defualt this will trim the values(true).
            Dictionary<string, string> arguementDictionary = DictionaryCreator.CreateFromArguments(args, delimeter, isTrim);

            //Load the values into the model
            TypeConverter typeConverter = new TypeConverter();  //you can extend this, but we only need the built in c# types
                                                                //if you're unsure what types are included in this, please refer to /Models/ExampleModel.cs
            ModelLoader<ExampleModel> modelLoader = new ModelLoader<ExampleModel>(typeConverter); //typeof(ExampleModel) gets the type of the model we want to load the values into.
            ExampleModel model = modelLoader.LoadModel(arguementDictionary);    //Loads the values into the model
            Console.WriteLine(modelLoader.UnloadModel(model));

            return model;
        }

        public static List<ExampleModel> LoadFromFile(string filePath, char delimeter)
        {
            //Loads the arguements with the associated variable name into a List<Dictionary<string,string>>
            //Each dictionary in this list represents a single line of the file.
            bool isTrim = true; //use this if you do not want to trim the values(false) read in from the arguements. By defualt this will trim the values(true).
            //List<string> headers = new List<string>() {"IntExample","StringExample","BoolExample","CharArrayExample","CharExample","DecimalExample","DoubleExample","FloatExample","UIntExample","LongExample","ULongExample","ShortExample","UShortExample","ListExample","NEVER_USED"};
            List<Dictionary<string, string>> arguementDictionaries = DictionaryCreator.CreateFromFlatFile(filePath, delimeter, isTrim);
            //List<Dictionary<string, string>> arguementDictionariesWitHeaders = DictionaryCreator.CreateFromFlatFile(filePath, delimeter, headers, isTrim);

            //Load the values into the model
            TypeConverter typeConverter = new TypeConverter();  //you can extend this, but we only need the built in c# types
                                                                //if you're unsure what types are included in this, please refer to /Models/ExampleModel.cs
            ModelLoader<ExampleModel> modelLoader = new ModelLoader<ExampleModel>(typeConverter); //typeof(ExampleModel) gets the type of the model we want to load the values into.
            List<ExampleModel> models = new List<ExampleModel>();

            foreach (Dictionary<string, string> line in arguementDictionaries)
            {
                ExampleModel model = modelLoader.LoadModel(line);   //Loads the values into a model
                models.Add(model);  //adds the models to the model list.
                                    //This is a representation of the file after the values have been loaded into several models.
                                    //Each model is equivilant to one line in the file.
            }

            return models;


        }

        public static Dictionary<string, string> GetDictionaryFromModel(ExampleModel model)
        {
            //Load the values into the dictionary
            TypeConverter typeConverter = new TypeConverter();  //you can extend this, but we only need the built in c# types
                                                                //if you're unsure what types are included in this, please refer to /Models/ExampleModel.cs

            return DictionaryCreator.CreateFromModel<ExampleModel>(model, typeConverter);
        }

        public static void WriteToFile(string filePath, char delimeter, List<ExampleModel> models)
        {
            TypeConverter typeConverter = new TypeConverter();  //you can extend this, but we only need the built in c# types
                                                                //if you're unsure what types are included in this, please refer to /Models/ExampleModel.cs

            GenerateFlatFile.WriteToFile(filePath, delimeter, typeConverter, models, isHaveHeaders: true, isHaveQuotes: true);
        }

        public static void OverridingTypeConverter()
        {
            CustomTypeConverter customTypeConvert = new CustomTypeConverter();
            ModelLoader<ExampleModelWithCustomType> modelLoader = new ModelLoader<ExampleModelWithCustomType>(customTypeConvert);
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            ExampleModelWithCustomType exampleModelWithCustomType;
            Dictionary<string, string> returnDictionary;

            //Adding elements to the dictionary to build the model.
            dictionary.Add("MyCustomType", "Sierra 117");
            dictionary.Add("Label", "Chief");
            dictionary.Add("Date", "11/15/2001");

            exampleModelWithCustomType = modelLoader.LoadModel(dictionary);

            returnDictionary = DictionaryCreator.CreateFromModel<ExampleModelWithCustomType>(exampleModelWithCustomType, customTypeConvert);
        }

    }
}

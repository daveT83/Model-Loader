using System;
using System.Collections.Generic;
using System.Reflection;

namespace Model_Loader.Infrastructure
{
    public class ModelLoader
    {
        public Type Type { get; private set; }
        public bool IsCaseSensitive { get; set; }
        public TypeConverter TypeConverter { get; private set; }

        public ModelLoader(Type modelType, TypeConverter typeConverter)
        {
            Type = modelType;
            IsCaseSensitive = false;
            TypeConverter = typeConverter;
        }

        /// <summary>
        /// Loads the values in the dictionary into the given model. 
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public dynamic LoadModel(Dictionary<string, string> elements)
        {
            object model = Activator.CreateInstance(Type);

            foreach (KeyValuePair<string, string> element in elements)
            {
                LoadField(element, model);
            }

            return model;
        }

        /// <summary>
        /// Loads a single fieldinto the model.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="model"></param>
        private void LoadField(KeyValuePair<string, string> element, object model)
        {
            PropertyInfo[] properties = Type.GetProperties();

            foreach (PropertyInfo propertyInfo in properties)
            {
                if (IsCaseSensitive && propertyInfo.Name.Equals(element.Key))
                {
                    SetValue(propertyInfo, model, propertyInfo.PropertyType, element.Value);
                }
                else if (!IsCaseSensitive && propertyInfo.Name.ToLower().Equals(element.Key.ToLower()))
                {
                    SetValue(propertyInfo, model, propertyInfo.PropertyType, element.Value);
                }
            }
        }

        /// <summary>
        /// Sets the value for a given field
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="model"></param>
        /// <param name="fieldType"></param>
        /// <param name="fieldValue"></param>
        private void SetValue(PropertyInfo propertyInfo, object model, Type fieldType, string fieldValue)
        {
            propertyInfo.SetValue(model, TypeConverter.ConvertToType(fieldValue, fieldType, typeof(TypeConverter)));
        }
    }
}

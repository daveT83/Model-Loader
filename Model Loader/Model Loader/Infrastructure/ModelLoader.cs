using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Model_Loader.Infrastructure
{
    public class ModelLoader
    {
        public Type Type { get; private set; }
        public bool IsCaseSensitive { get; set; }
        public dynamic Type_Converter { get; private set; }

        public ModelLoader(Type modelType, dynamic typeConverter)
        {
            Type = modelType;
            IsCaseSensitive = false;
            Type_Converter = typeConverter;
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
            PropertyInfo[] properties;

            if (IsCaseSensitive)
            {
                properties = Type.GetProperties().Where(x => x.Name.Equals(element.Key)).ToArray();
            }
            else
            {
                properties = Type.GetProperties().Where(x => x.Name.ToLower().Equals(element.Key.ToLower())).ToArray();
            }

            if (properties.Length > 0)
            {
                SetValue(properties[0], model, properties[0].PropertyType, GetValue(element.Value));
            }
        }

        /// <summary>
        /// defaults a value to null if it isn't present
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetValue(string value)
        {
            if (value.ToLower().Equals("null") || String.IsNullOrWhiteSpace(value) || String.IsNullOrEmpty(value))
            {
                return "";
            }
            return value;
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
            if (fieldType.GetGenericArguments().Length > 0)
            {
                MethodInfo generic = typeof(TypeConverter).GetMethod("ConvertToTypedListFromStringList").MakeGenericMethod(fieldType.GetGenericArguments().Single());
                List<string> value = Type_Converter.ConvertToType(fieldValue, fieldType);

                propertyInfo.SetValue(model, generic.Invoke(Type_Converter, new object[] { value }));
            }
            else
            {
                propertyInfo.SetValue(model, Type_Converter.ConvertToType(fieldValue, fieldType));
            }
        }
    }
}

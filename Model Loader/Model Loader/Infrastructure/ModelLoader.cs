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
        public TypeConverter TypeConverter { get; private set; }
        public List<string> AdditionalNameSpacesToReference { get; set; }

        public ModelLoader(Type modelType, TypeConverter typeConverter)
        {
            Type = modelType;
            IsCaseSensitive = false;
            TypeConverter = typeConverter;
            AdditionalNameSpacesToReference = new List<string>();
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
                return null;
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

                MethodInfo method = typeof(TypeConverter).GetMethod("ConvertToTypedListFromStringList");
                MethodInfo generic = method.MakeGenericMethod(fieldType.GetGenericArguments().Single());

                Console.WriteLine(fieldType.GetGenericArguments().Single());
                propertyInfo.SetValue(model, generic.Invoke(TypeConverter, new object[] { TypeConverter.ConvertToType(fieldValue, fieldType, typeof(TypeConverter)) }));
            }
            else
            {
                propertyInfo.SetValue(model, TypeConverter.ConvertToType(fieldValue, fieldType, typeof(TypeConverter)));
            }
        }
    }
}

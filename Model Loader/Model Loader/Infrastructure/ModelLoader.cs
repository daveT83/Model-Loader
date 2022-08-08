using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Model_Loader.Infrastructure
{
    public class ModelLoader<T>
    {
        public bool IsCaseSensitive { get; set; }
        public dynamic Type_Converter { get; private set; }

        public ModelLoader(dynamic typeConverter)
        {
            IsCaseSensitive = false;
            Type_Converter = typeConverter;
        }

        /// <summary>
        /// Converts the model to a human readable string
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string UnloadModel(T model, bool isIncludePropertyNames = true)
        {
            string modelString = "";
            foreach (PropertyInfo property in model.GetType().GetProperties())
            {
                Type type = property.PropertyType;

                if (type.GetGenericArguments().Length > 0)
                {
                    if (type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
                    {
                        Type[] genericTypes = property.PropertyType.GetGenericArguments();
                        MethodInfo genericMethod = Type_Converter.GetType().GetMethod("ConvertFromIDictionaryType").MakeGenericMethod(genericTypes[0], genericTypes[1]);

                        if (isIncludePropertyNames)
                        {
                            modelString += property.Name + ": " + (string)genericMethod.Invoke(Type_Converter, new object[] { property.GetValue(model) });
                        }
                        else
                        {
                            modelString += (string)genericMethod.Invoke(Type_Converter, new object[] { property.GetValue(model) });
                        }
                        modelString += Environment.NewLine;
                    }
                    else if (type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                    {
                        MethodInfo genericMethod = Type_Converter.GetType().GetMethod("ConvertFromIEnumerableType").MakeGenericMethod(property.PropertyType.GetGenericArguments()[0]);

                        if (isIncludePropertyNames)
                        {
                            modelString += property.Name + ": " + (string)genericMethod.Invoke(Type_Converter, new object[] { property.GetValue(model) });
                        }
                        else
                        {
                            modelString += (string)genericMethod.Invoke(Type_Converter, new object[] { property.GetValue(model) });
                        }
                        modelString += Environment.NewLine;
                    }
                }
                else
                {
                    MethodInfo genericMethod = Type_Converter.GetType().GetMethod("ConvertFromType").MakeGenericMethod(property.PropertyType);

                    if (isIncludePropertyNames)
                    {
                        modelString += property.Name + ": " + (string)genericMethod.Invoke(Type_Converter, new object[] { property.GetValue(model) });
                    }
                    else
                    {
                        modelString += (string)genericMethod.Invoke(Type_Converter, new object[] { property.GetValue(model) });
                    }
                    modelString += Environment.NewLine;
                }
            }

            return modelString.TrimEnd(Environment.NewLine.ToCharArray());
        }

        /// <summary>
        /// Loads the values in the dictionary into the given model. 
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public T LoadModel(Dictionary<string, string> elements)
        {
            T model = (T)Activator.CreateInstance(typeof(T));

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
                properties = typeof(T).GetProperties().Where(x => x.Name.Equals(element.Key)).ToArray();
            }
            else
            {
                properties = typeof(T).GetProperties().Where(x => x.Name.ToLower().Equals(element.Key.ToLower())).ToArray();
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
        private void SetValue(PropertyInfo propertyInfo, object model, Type type, string fieldValue)
        {
            MethodInfo generic;

            if (type.GetGenericArguments().Length > 0)
            {
                if (type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
                {
                    Type[] genericTypes = type.GetGenericArguments();
                    generic = typeof(TypeConverter).GetMethod("ConvertToIDictionaryType").MakeGenericMethod(genericTypes[0], genericTypes[1]);

                    propertyInfo.SetValue(model, generic.Invoke(Type_Converter, new object[] { fieldValue }));
                }
                else if (type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                {
                    generic = typeof(TypeConverter).GetMethod("ConvertToIEnumerableType").MakeGenericMethod(type.GetGenericArguments().Single());

                    propertyInfo.SetValue(model, generic.Invoke(Type_Converter, new object[] { fieldValue }));
                }

            }
            else
            {
                generic = typeof(TypeConverter).GetMethod("ConvertToType").MakeGenericMethod(type);

                propertyInfo.SetValue(model, generic.Invoke(Type_Converter, new object[] { fieldValue }));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Model_Loader.Infrastructure
{
    public class TypeConverter
    {
        private MethodInfo[] methodInfos;

        public TypeConverter()
        {
            methodInfos = TrimMethodInfos(this.GetType().GetMethods());
        }

        /// <summary>
        /// Converts a string to the appropriate type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// 
        public T ConvertToType<T>(string value)
        {
            if (String.IsNullOrEmpty(value) || String.IsNullOrWhiteSpace(value))
            {
                return default(T);
            }
            else if (!typeof(T).Equals(value.GetType()))
            {
                if (typeof(T).IsArray)
                {
                    MethodInfo genericMethod = this.GetType().GetMethod("ConvertToArray").MakeGenericMethod(typeof(T).GetElementType());

                    return (T)genericMethod.Invoke(this, new object[] { value });
                }
                else
                {
                    List<MethodInfo> filteredMethodInfos = methodInfos.Where(x => x.ReturnType.Name.Equals(typeof(T).Name)).ToList();
                    if (filteredMethodInfos.Count > 0)
                    {
                        return (T)filteredMethodInfos[0].Invoke(Activator.CreateInstance(this.GetType()), new object[] { value });
                    }
                }
            }
            else
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }

            return default(T);
        }

        /// <summary>
        /// Converts a string to IEnumerable type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public IEnumerable<T> ConvertToIEnumerableType<T>(string value)
        {
            if (String.IsNullOrEmpty(value) || String.IsNullOrWhiteSpace(value))
            {
                return default(IEnumerable<T>);
            }
            Type type = typeof(IEnumerable<>);
            IEnumerable<MethodInfo> filteredMethodInfos = methodInfos.Where(x => (x.ReturnType.Namespace + "." + x.ReturnType.Name).StartsWith(type.Namespace + "." + type.Name) && !x.Name.EndsWith("Type"));

            if (filteredMethodInfos.Count(x => true) > 0)
            {
                MethodInfo genericMethod = this.GetType().GetMethod("ConvertToIEnumerable").MakeGenericMethod(typeof(T));

                return (IEnumerable<T>)genericMethod.Invoke(this, new object[] { value });
            }
            return default(IEnumerable<T>);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public IDictionary<T, U> ConvertToIDictionaryType<T, U>(string value)
        {
            if (String.IsNullOrEmpty(value) || String.IsNullOrWhiteSpace(value))
            {
                return default(IDictionary<T, U>);
            }
            Type type = typeof(IDictionary<,>);
            IEnumerable<MethodInfo> filteredMethodInfos = methodInfos.Where(x => (x.ReturnType.Namespace + "." + x.ReturnType.Name).StartsWith(type.Namespace + "." + type.Name) && !x.Name.EndsWith("Type"));

            if (filteredMethodInfos.Count(x => true) > 0)
            {
                return ConvertToIDictionary<T, U>(value);
            }
            return default(IDictionary<T, U>);
        }

        /// <summary>
        /// Converts from a IEnumerable<type> to string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="parentClass"></param>
        /// <returns></returns>
        public string ConvertFromIEnumerableType<T>(IEnumerable<T> value)
        {
            if (value == null)
            {
                return "";
            }
            else
            {
                Type type = value.GetType().GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)).FirstOrDefault();
                IEnumerable<MethodInfo> filteredMethodInfos = methodInfos.Where(x => (x.GetParameters()[0].ParameterType.Namespace + "." + x.GetParameters()[0].ParameterType.Name).StartsWith(type.Namespace + "." + type.Name) && !x.Name.EndsWith("Type"));

                if (filteredMethodInfos.Count(x => true) > 0)
                {

                    MethodInfo genericMethod = this.GetType().GetMethod("ConvertFromIEnumerable").MakeGenericMethod(typeof(T));

                    return (string)genericMethod.Invoke(this, new object[] { value });
                }
            }

            return value.ToString();
        }

        /// <summary>
        /// Converts from a IDictionary<T,U> to string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="value"></param>
        /// <param name="parentClass"></param>
        /// <returns></returns>
        public string ConvertFromIDictionaryType<T, U>(IDictionary<T, U> value)
        {
            if (value == null)
            {
                return "";
            }
            else
            {
                Type type = value.GetType().GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDictionary<,>)).FirstOrDefault();
                IEnumerable<MethodInfo> filteredMethodInfos = methodInfos.Where(x => (x.GetParameters()[0].ParameterType.Namespace + "." + x.GetParameters()[0].ParameterType.Name).StartsWith(type.Namespace + "." + type.Name) && !x.Name.EndsWith("Type"));

                if (filteredMethodInfos.Count(x => true) > 0)
                {

                    MethodInfo genericMethod = this.GetType().GetMethod("ConvertFromIDictionary").MakeGenericMethod(typeof(T), typeof(U));

                    return (string)genericMethod.Invoke(this, new object[] { value });
                }
            }

            return value.ToString();
        }

        /// <summary>
        /// Converts from a type to string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="parentClass"></param>
        /// <returns></returns>
        public string ConvertFromType<T>(T value)
        {
            if (value == null)
            {
                return "";
            }

            else if (!typeof(String).Equals(value.GetType()))
            {
                if (typeof(T).IsArray)
                {
                    MethodInfo genericMethod = this.GetType().GetMethod("ConvertFromArray").MakeGenericMethod(typeof(T).GetElementType());

                    return (string)genericMethod.Invoke(this, new object[] { value });
                }
                else
                {
                    List<MethodInfo> filteredMethodInfos = methodInfos.Where(x => x.GetParameters()[0].ParameterType.Equals(value.GetType())).ToList();
                    if (filteredMethodInfos.Count > 0)
                    {
                        return filteredMethodInfos[0].Invoke(Activator.CreateInstance(this.GetType()), new object[] { value }).ToString();
                    }
                }
            }
            else
            {
                return (string)Convert.ChangeType(value, typeof(string));
            }
            return "";
        }

        /// <summary>
        /// Strips out unwanted methods
        /// </summary>
        /// <param name="methodInfos"></param>
        /// <returns></returns>
        private MethodInfo[] TrimMethodInfos(MethodInfo[] methodInfos)
        {
            string[] names = new string[] { "ConvertToType", "ConvertToTypedListFromStringList", "Equals", "GetHashCode", "GetType", "ToString", "ConvertFromIEnumerableHelper" };
            return methodInfos.Where(x => !names.Contains(x.Name)).ToArray();
        }

        public virtual bool ConvertToBool(string value)
        {
            return Convert.ToBoolean(value);
        }

        public virtual byte ConvertToByte(string value)
        {
            return Convert.ToByte(value);
        }

        public virtual sbyte ConvertToSbyte(string value)
        {
            return Convert.ToSByte(value);
        }

        public virtual char ConvertToChar(string value)
        {
            return value.ToCharArray()[0];
        }

        public virtual decimal ConvertToDecimal(string value)
        {
            return Decimal.Parse(value);
        }

        public virtual double ConvertToDouble(string value)
        {
            return Double.Parse(value);
        }

        public virtual float ConvertToFloat(string value)
        {
            return Single.Parse(value);
        }

        public virtual int ConvertToInt(string value)
        {
            return Int32.Parse(value);
        }

        public virtual uint ConvertToUint(string value)
        {
            return UInt32.Parse(value);
        }

        public virtual long ConvertToLong(string value)
        {
            return Int64.Parse(value);
        }

        public virtual ulong ConvertToUlong(string value)
        {
            return UInt64.Parse(value);
        }

        public virtual short ConvertToShort(string value)
        {
            return Int16.Parse(value);
        }

        public virtual ushort ConvertToUshort(string value)
        {
            return UInt16.Parse(value);
        }

        public virtual string ConvertToString(string value)
        {
            return value;
        }

        public virtual T[] ConvertToArray<T>(string value)
        {
            return ConvertToIEnumerable<T>(value).ToArray();
        }

        public virtual IEnumerable<T> ConvertToIEnumerable<T>(string value)
        {
            List<T> list = new List<T>();

            foreach (string val in SplitIEnumerableElement(value))
            {
                if (typeof(T).GetGenericArguments().Length > 0)
                {
                    if (typeof(T).GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
                    {
                        Type[] generics = typeof(T).GetGenericArguments();
                        MethodInfo genericMethod = this.GetType().GetMethod("ConvertToIDictionary").MakeGenericMethod(generics[0], generics[1]);

                        list.Add((T)genericMethod.Invoke(this, new object[] { val }));
                    }
                    else if (typeof(T).GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                    {
                        MethodInfo genericMethod = this.GetType().GetMethod("ConvertToIEnumerable").MakeGenericMethod(typeof(T).GetGenericArguments().Single());

                        list.Add((T)genericMethod.Invoke(this, new object[] { val }));
                    }
                }
                else
                {
                    list.Add(ConvertToType<T>(val));
                }
            }

            return list;
        }

        public virtual IDictionary<T, U> ConvertToIDictionary<T, U>(string value)
        {
            IDictionary<T, U> dict = new Dictionary<T, U>();
            string[] strings = SplitIDictionary(value);
            List<T> keyList = new List<T>();
            List<U> valueList = new List<U>();

            foreach (string val in SplitIDictionaryElement(strings[0]))
            {
                if (typeof(T).GetGenericArguments().Length > 0)
                {
                    if (typeof(T).GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
                    {
                        Type[] generics = typeof(T).GetGenericArguments();
                        MethodInfo genericMethod = this.GetType().GetMethod("ConvertToIDictionary").MakeGenericMethod(generics[0], generics[1]);

                        keyList.Add((T)genericMethod.Invoke(this, new object[] { val }));
                    }
                    else if (typeof(T).GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                    {
                        MethodInfo genericMethod = this.GetType().GetMethod("ConvertToIEnumerable").MakeGenericMethod(typeof(T).GetGenericArguments().Single());

                        keyList.Add((T)genericMethod.Invoke(this, new object[] { val }));
                    }
                }
                else
                {
                    keyList.Add(ConvertToType<T>(val));
                }
            }

            foreach (string val in SplitIDictionaryElement(strings[1]))
            {
                if (typeof(U).GetGenericArguments().Length > 0)
                {
                    if (typeof(U).GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
                    {
                        Type[] generics = typeof(U).GetGenericArguments();
                        MethodInfo genericMethod = this.GetType().GetMethod("ConvertToIDictionary").MakeGenericMethod(generics[0], generics[1]);

                        valueList.Add((U)genericMethod.Invoke(this, new object[] { val }));
                    }
                    else if (typeof(U).GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                    {
                        Type[] generics = typeof(U).GetGenericArguments();
                        MethodInfo genericMethod = this.GetType().GetMethod("ConvertToIEnumerable").MakeGenericMethod(generics[0], generics[1]);

                        valueList.Add((U)genericMethod.Invoke(this, new object[] { val }));
                    }
                }
                else
                {
                    valueList.Add(ConvertToType<U>(val));
                }
            }

            for (int i = 0; i < keyList.Count; i++)
            {
                dict.Add(keyList[i], valueList[i]);
            }

            return dict;

        }

        private IEnumerable<string> SplitIEnumerableElement(string value)
        {
            if (value.Count(x => x.Equals(':') || x.Equals('<')) == 0)
            {
                return value.Split(',');
            }
            List<string> elements = new List<string>();
            int openArrowCount = 0;
            int closeArrowCount = 0;
            string str = "";


            foreach (char c in value.ToCharArray())
            {
                if (c.Equals('<'))
                {
                    openArrowCount++;


                }
                else if (c.Equals('>'))
                {
                    closeArrowCount++;
                }
                else
                {

                    if (openArrowCount - 1 == closeArrowCount && c.Equals(','))
                    {
                        elements.Add(str.Trim());
                        openArrowCount = 1;
                        closeArrowCount = 0;
                        str = "";
                    }
                    else if (openArrowCount != closeArrowCount)
                    {
                        str += c;
                    }
                }
            }

            if (!String.IsNullOrEmpty(str))
            {
                elements.Add(str);
            }

            return elements;
        }

        private IEnumerable<string> SplitIDictionaryElement(string value)
        {
            if (value.Count(x => x.Equals(':') || x.Equals('<')) == 0)
            {
                return value.Split(',');
            }
            List<string> elements = new List<string>();
            int openArrowCount = 0;
            int closeArrowCount = 0;
            string str = "";


            foreach (char c in value.ToCharArray())
            {
                if (c.Equals('<'))
                {
                    openArrowCount++;


                }
                else if (c.Equals('>'))
                {
                    closeArrowCount++;
                }
                else
                {
                    if (openArrowCount == closeArrowCount && c.Equals(':'))
                    {
                        elements.Add(str.Trim());
                        openArrowCount = 0;
                        closeArrowCount = 0;
                        str = "";
                    }
                    else if (openArrowCount != closeArrowCount)
                    {
                        str += c;
                    }
                }
            }

            if (!String.IsNullOrEmpty(str))
            {
                elements.Add(str);
            }

            return elements;
        }
        private string[] SplitIDictionary(string value)
        {
            if (value.Count(x => x.Equals(':')) == 1)
            {
                return value.Split(':');
            }
            string[] split = new string[2] { "", "" };
            int openArrowCount = 0;
            int closeArrowCount = 0;
            bool isLeftString = true;

            foreach (char c in value.ToCharArray())
            {
                if (c.Equals('<'))
                {

                    if (openArrowCount == 1)
                    {
                        continue;
                    }
                    else
                    {
                        openArrowCount++;
                    }

                }
                else if (c.Equals('>'))
                {
                    closeArrowCount++;

                    if (closeArrowCount > openArrowCount)
                    {
                        continue;
                    }
                }

                if (openArrowCount == closeArrowCount && c.Equals(':'))
                {
                    openArrowCount = 0;
                    closeArrowCount = 0;
                    isLeftString = false;
                }
                else if (isLeftString)
                {
                    split[0] += c;
                }
                else
                {
                    split[1] += c;
                }
            }

            return split;
        }

        public virtual DateTime ConvertToDateTime(string value)
        {
            return DateTime.Parse(value);
        }

        public virtual string ConvertFromBool(bool value)
        {
            return Convert.ToString(value);
        }

        public virtual string ConvertFromByte(byte value)
        {
            return value.ToString();
        }

        public virtual string ConvertFromSbyte(sbyte value)
        {
            return value.ToString();
        }

        public virtual string ConvertFromChar(char value)
        {
            return Convert.ToString(value);
        }

        public virtual string ConvertFromDecimal(decimal value)
        {
            return Convert.ToString(value);
        }

        public virtual string ConvertFromDouble(double value)
        {
            return Convert.ToString(value);
        }

        public virtual string ConvertFromFloat(float value)
        {
            return Convert.ToString(value);
        }

        public virtual string ConvertFromInt(int value)
        {
            return Convert.ToString(value);
        }

        public virtual string ConvertFromUint(uint value)
        {
            return Convert.ToString(value);
        }

        public virtual string ConvertFromLong(long value)
        {
            return Convert.ToString(value);
        }

        public virtual string ConvertFromUlong(ulong value)
        {
            return Convert.ToString(value);
        }
        public virtual string ConvertFromShort(short value)
        {
            return Convert.ToString(value);
        }

        public virtual string ConvertFromUshort(ushort value)
        {
            return Convert.ToString(value);
        }

        public virtual string ConvertFromString(string value)
        {
            return value;
        }

        public virtual string ConvertFromArray<T>(T[] value)
        {
            string array = ConvertFromIEnumerable<T>(value);
            return array;
        }

        public virtual string ConvertFromIEnumerable<T>(IEnumerable<T> value)
        {
            string str = "<";
            foreach (var val in value)
            {
                if (typeof(T).GetGenericArguments().Length > 0)
                {
                    if (typeof(T).GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
                    {
                        Type[] generics = typeof(T).GetGenericArguments();
                        MethodInfo genericMethod = this.GetType().GetMethod("ConvertFromIDictionary").MakeGenericMethod(generics[0], generics[1]);

                        str += genericMethod.Invoke(this, new object[] { val }) + ",";
                    }
                    else if (typeof(T).GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                    {
                        Type[] generics = typeof(T).GetGenericArguments();

                        MethodInfo genericMethod = this.GetType().GetMethod("ConvertFromIEnumerable").MakeGenericMethod(typeof(T).GetGenericArguments().Single());

                        str += genericMethod.Invoke(this, new object[] { val }) + ",";
                    }
                }
                else
                {
                    str += ConvertFromType<T>(val) + ",";
                }
            }
            return str.TrimEnd(',') + ">";
        }

        public virtual string ConvertFromIDictionary<T, U>(IDictionary<T, U> value)
        {
            string str = "<";
            foreach (var val in value.Keys)
            {
                if (typeof(T).GetGenericArguments().Length > 0)
                {
                    if (typeof(T).GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
                    {
                        Type[] generics = typeof(T).GetGenericArguments();
                        MethodInfo genericMethod = this.GetType().GetMethod("ConvertFromIDictionary").MakeGenericMethod(generics[0], generics[1]);

                        str += genericMethod.Invoke(this, new object[] { val }) + ",";
                    }
                    else if (typeof(T).GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                    {
                        Type[] generics = typeof(T).GetGenericArguments();

                        MethodInfo genericMethod = this.GetType().GetMethod("ConvertFromIEnumerable").MakeGenericMethod(typeof(T).GetGenericArguments().Single());

                        str += genericMethod.Invoke(this, new object[] { val }) + ",";
                    }
                }
                else
                {
                    str += ConvertFromType<T>(val) + ",";
                }
            }

            str = str.Trim(',') + ":";

            foreach (var val in value.Values)
            {
                if (typeof(U).GetGenericArguments().Length > 0)
                {
                    if (typeof(U).GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
                    {
                        Type[] generics = typeof(U).GetGenericArguments();
                        MethodInfo genericMethod = this.GetType().GetMethod("ConvertFromIDictionary").MakeGenericMethod(generics[0], generics[1]);

                        str += genericMethod.Invoke(this, new object[] { val }) + ",";
                    }
                    else if (typeof(U).GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                    {
                        Type[] generics = typeof(U).GetGenericArguments();

                        MethodInfo genericMethod = this.GetType().GetMethod("ConvertFromIEnumerable").MakeGenericMethod(typeof(T).GetGenericArguments().Single());

                        str += genericMethod.Invoke(this, new object[] { val }) + ",";
                    }
                }
                else
                {
                    str += ConvertFromType<U>(val) + ",";
                }
            }

            return str.TrimEnd(',') + ">";
        }

        public virtual string ConvertFromDateTime(DateTime value)
        {
            return value.ToString();
        }
    }
}

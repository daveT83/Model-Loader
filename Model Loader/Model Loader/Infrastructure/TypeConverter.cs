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
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public T ConvertToType<T>(string value)
        {
            if (String.IsNullOrEmpty(value) || String.IsNullOrWhiteSpace(value))
            {
                return default(T);
            }
            else if (!typeof(T).Equals(value.GetType()))
            {
                List<MethodInfo> filteredMethodInfos = methodInfos.Where(x => x.ReturnType.Name.Equals(typeof(T).Name)).ToList();
                if (filteredMethodInfos.Count > 0)
                {
                    return (T)filteredMethodInfos[0].Invoke(Activator.CreateInstance(this.GetType()), new object[] { value });
                }
            }
            return default(T);
        }

        /// <summary>
        /// Converts a string to the appropriate type.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public dynamic ConvertToType(string value, Type type)
        {
            if (String.IsNullOrEmpty(value) || String.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else if (!type.Equals(value.GetType()))
            {
                List<MethodInfo> filteredMethodInfos = methodInfos.Where(x => x.ReturnType.Name.Equals(type.Name)).ToList();
                if (filteredMethodInfos.Count > 0)
                {
                    return filteredMethodInfos[0].Invoke(Activator.CreateInstance(this.GetType()), new object[] { value });
                }
            }
            return value;
        }

        /// <summary>
        /// Converts from a IEnumerable<type> to string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="parentClass"></param>
        /// <returns></returns>
        public string ConvertFromIEnumerableType<T>(IEnumerable<T> value)
        {
            if (value == null)
            {
                return "";
            }
            else if (!typeof(String).Equals(value.GetType()))
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
        /// Converts from a type to string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
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
                List<MethodInfo> filteredMethodInfos = methodInfos.Where(x => x.GetParameters()[0].ParameterType.Equals(value.GetType())).ToList();
                if (filteredMethodInfos.Count > 0)
                {
                    return filteredMethodInfos[0].Invoke(Activator.CreateInstance(this.GetType()), new object[] { value }).ToString();
                }
            }
            return "";
        }

        /// <summary>
        /// Strips out unwanted methods
        /// </summary>
        /// <param name="methodInfos"></param>
        /// <returns></returns>

        /// <summary>
        /// Converts a List of string to a different type.
        /// </summary>
        /// <param name="stringList"></param>
        /// <returns></returns>
        public IEnumerable<T> ConvertToTypedListFromStringList<T>(IEnumerable<string> stringList)
        {
            List<T> list = new List<T>();

            if (stringList != null)
            {
                foreach (string str in stringList)
                {
                    list.Add(ConvertToType<T>(str));
                }
            }

            return list;
        }

        private MethodInfo[] TrimMethodInfos(MethodInfo[] methodInfos)
        {
            string[] names = new string[] { "ConvertToType", "ConvertToTypedListFromStringList", "Equals", "GetHashCode", "GetType", "ToString", "ConvertFromIEnumerableHelper" };
            return methodInfos.Where(x => !names.Contains(x.Name)).ToArray();
        }

        public virtual bool ConvertToBool(string value)
        {
            return Convert.ToBoolean(value);
        }

        public virtual byte[] ConvertToByteArray(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        public virtual byte ConvertToByte(string value)
        {
            return Convert.ToByte(value);
        }

        public virtual sbyte ConvertToSbyte(string value)
        {
            return Convert.ToSByte(value);
        }

        public virtual char[] ConvertToCharArray(string value)
        {
            return value.ToCharArray();
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

        public virtual List<string> ConvertToList(string value)
        {
            List<string> list = new List<string>();

            foreach (string val in value.Split(','))
            {
                list.Add(val);
            }

            return list;
        }

        public virtual DateTime ConvertToDateTime(string value)
        {
            return DateTime.Parse(value);
        }

        public virtual string ConvertFromBool(bool value)
        {
            return Convert.ToString(value);
        }

        public virtual string ConvertFromByteArray(byte[] value)
        {
            return String.Join("", value);
        }

        public virtual string ConvertFromByte(byte value)
        {
            return value.ToString();
        }

        public virtual string ConvertFromSbyte(sbyte value)
        {
            return value.ToString();
        }

        public virtual string ConvertFromCharArray(char[] value)
        {
            string chars = "";

            foreach (char c in value)
            {
                chars += c;
            }
            return chars;
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

        public virtual string ConvertFromIEnumerable<T>(IEnumerable<T> value)
        {
            string str = "";
            foreach (var val in value)
            {
                str += ConvertFromType(val) + ",";
            }
            return str.TrimEnd(',');
        }

        public virtual string ConvertFromDateTime(DateTime value)
        {
            return value.ToString();
        }
    }
}

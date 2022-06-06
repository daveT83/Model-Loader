using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Model_Loader.Infrastructure
{
    public class TypeConverter
    {

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
            else
            {
                MethodInfo[] methodInfos = TrimMethodInfos(this.GetType().GetMethods());

                if (!type.Equals(value.GetType()))
                {
                    foreach (MethodInfo methodInfo in methodInfos)
                    {
                        if (methodInfo.ReturnType.Name.Equals(type.Name))
                        {
                            return methodInfo.Invoke(Activator.CreateInstance(this.GetType()), new object[] { value });
                        }
                    }
                }
                return value;
            }
        }

        /// <summary>
        /// Converts a string to the appropriate type.
        /// Use this if you need to inherit this class.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="parentClass"></param>
        /// <returns></returns>
        public dynamic ConvertToType(string value, Type type, Type parentClass)
        {
            if (String.IsNullOrEmpty(value) || String.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                MethodInfo[] methodInfos = TrimMethodInfos(parentClass.GetMethods());

                if (!type.Equals(value.GetType()))
                {
                    foreach (MethodInfo methodInfo in methodInfos)
                    {
                        if (methodInfo.ReturnType.Name.Equals(type.Name))
                        {
                            return methodInfo.Invoke(Activator.CreateInstance(parentClass), new object[] { value });
                        }
                    }
                }
                return value;
            }
        }

        /// <summary>
        /// Converts from a type to string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="parentClass"></param>
        /// <returns></returns>
        public string ConvertFromType(dynamic value, Type type, Type parentClass)
        {
            if (value == null)
            {
                return "";
            }
            else
            {
                MethodInfo[] methodInfos = TrimMethodInfos(parentClass.GetMethods());

                if (!typeof(String).Equals(value.GetType()))
                {
                    foreach (MethodInfo methodInfo in methodInfos)
                    {
                        if (methodInfo.GetParameters()[0].ParameterType.Equals(value.GetType()))
                        {
                            return methodInfo.Invoke(Activator.CreateInstance(parentClass), new object[] { value }).ToString();
                        }
                    }
                }
                return value;
            }
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
        public List<T> ConvertToTypedListFromStringList<T>(List<string> stringList)
        {
            Type type = typeof(T);
            List<T> list = new List<T>();

            if (stringList != null) {
                foreach (string str in stringList)
                {
                    list.Add(ConvertToType(str, type));
                }
            }

            return list;
        }

        private MethodInfo[] TrimMethodInfos(MethodInfo[] methodInfos)
        {
            string[] names = new string[] { "ConvertToType", "ConvertToTypedListFromStringList", "Equals", "GetHashCode", "GetType", "ToString" };
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

        public virtual string ConvertFromList(List<dynamic> value)
        {
            return String.Join(",", value);
        }

        public virtual string ConvertFromDateTime(DateTime value)
        {
            return value.ToString();
        }
    }
}

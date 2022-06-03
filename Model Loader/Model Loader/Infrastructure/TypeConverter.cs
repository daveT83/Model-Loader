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
        public List<T> ConvertToTypedListFromStringList<T>(List<string>stringList)
        {
            Type type = typeof(T);
            List<T> list = new List<T>();

            foreach (string str in stringList)
            {
                list.Add(ConvertToType(str,type));
            }

            return list;
        }
        
        private MethodInfo[] TrimMethodInfos(MethodInfo[] methodInfos)
        {
            string[] names = new string[] { "ConvertToType", "ConvertToTypedListFromStringList", "Equals", "GetHashCode", "GetType", "ToString" };
            return methodInfos.Where(x => !names.Contains(x.Name)).ToArray();
        }

        public bool ConvertToBool(string value)
        {
            return Convert.ToBoolean(value);
        }

        public byte[] ConvertToByteArray(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        public char[] ConvertToCharArray(string value)
        {
            return value.ToCharArray();
        }

        public char ConvertToChar(string value)
        {
            return value.ToCharArray()[0];
        }

        public decimal ConvertToDecimal(string value)
        {
            return Decimal.Parse(value);
        }

        public double ConvertToDouble(string value)
        {
            return Double.Parse(value);
        }

        public float ConvertToFloat(string value)
        {
            return Single.Parse(value);
        }

        public int ConvertToInt(string value)
        {
            return Int32.Parse(value);
        }

        public uint ConvertToUint(string value)
        {
            return UInt32.Parse(value);
        }

        public long ConvertToLong(string value)
        {
            return Int64.Parse(value);
        }

        public ulong ConvertToUlong(string value)
        {
            return UInt64.Parse(value);
        }
        public short ConvertToShort(string value)
        {
            return Int16.Parse(value);
        }

        public ushort ConvertToUshort(string value)
        {
            return UInt16.Parse(value);
        }

        public List<string> ConvertToList(string value)
        {
            List<string> list = new List<string>();

            foreach (string val in value.Split(','))
            {
                list.Add(val);
            }

            return list;
        } 

        public string ConvertFromBool(bool value)
        {
            return Convert.ToString(value);
        }

        public string ConvertFromByteArray(byte[] value)
        {
            return String.Join("", value);
        }

        public string ConvertFromCharArray(char[] value)
        {
            string chars = "";

            foreach (char c in value)
            {
                chars += c;
            }
            return chars;
        }

        public string ConvertFromChar(char value)
        {
            return Convert.ToString(value);
        }

        public string ConvertFromDecimal(decimal value)
        {
            return Convert.ToString(value);
        }

        public string ConvertFromDouble(double value)
        {
            return Convert.ToString(value);
        }

        public string ConvertFromFloat(float value)
        {
            return Convert.ToString(value);
        }

        public string ConvertFromInt(int value)
        {
            return Convert.ToString(value);
        }

        public string ConvertFromUint(uint value)
        {
            return Convert.ToString(value);
        }

        public string ConvertFromLong(long value)
        {
            return Convert.ToString(value);
        }

        public string ConvertFromUlong(ulong value)
        {
            return Convert.ToString(value);
        }
        public string ConvertFromShort(short value)
        {
            return Convert.ToString(value);
        }

        public string ConvertFromUshort(ushort value)
        {
            return Convert.ToString(value);
        }
    
        public string ConvertFromList(List<dynamic> value)
        {
            return String.Join(",",value);
        }
    }
}

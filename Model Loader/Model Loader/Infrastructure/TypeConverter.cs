using System;
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
                        if (methodInfo.ReturnType.Equals(type))
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
                        if (methodInfo.ReturnType.Equals(type))
                        {
                            return methodInfo.Invoke(Activator.CreateInstance(parentClass), new object[] { value });
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
        private MethodInfo[] TrimMethodInfos(MethodInfo[] methodInfos)
        {
            string[] names = new string[] { "ConvertToType", "Equals", "GetHashCode", "GetType", "ToString" };
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
    }
}

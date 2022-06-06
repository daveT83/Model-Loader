using Model_Loader.Infrastructure;
using Model_Loader_Examples.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model_Loader_Examples.Examples
{
    public class CustomTypeConverter:TypeConverter
    {
        //Overriding how the date is output
        public override string ConvertFromDateTime(DateTime value)
        {
            return value.ToString("MM-dd-yyyy");
        }

        //Adding support for my CustomType
        public CustomType ConvertToCustomType(string value)
        {
            CustomType type = new CustomType();
            string[] split = value.Split(' ');
             type.ID = ConvertToInt(split[1]);
            type.ID_Prefix = split[0];

            return type;
        }

        //adding support for my CustomType
        public string ConvertFromCustomType(CustomType value)
        {
            string id = ConvertFromInt(value.ID);
            string idPrefix = value.ID_Prefix;
            return idPrefix + " " + id;
        }
    }
}

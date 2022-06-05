# Model-Loader

## Summary:
Dynamically loads values into a model, pulls values out of a model, loads a flat file file into a ```List<T>``` and writes a flat file based on a ```List<T>```

This accepts a ```Dictionary<string,string> ``` with the Keys being the field names and the values being the values to load.
Support for auto loading from a flat file and from the arguments is built in.
For an example please see the included example project. [Examples](https://github.com/daveT83/Model-Loader/blob/main/Model%20Loader%20Examples/Model%20Loader%20Examples/Examples/Example.cs)

This has support for all built in types for c#, but if you need to add additional types you can extend the TypeConverter class to add your own conversions from a string value to another ```Type```. Best practice would be to follow the existing naming convention of ```public Type ConvertToType(string value)``` and ```public string ConvertFromType(Type value)```. Please see a list below for a list of all supported types.
  
## Supported Types:
  - bool
  - byte[]
  - byte
  - sbyte
  - char[]
  - char
  - decimal
  - double
  - float
  - int
  - ~~nint~~
  - uint
  - ~~nuint~~
  - long
  - short
  - ushort
  - string
  - ```List<T>```
  - DateTime
  
## What should I download?
  - If you want to download the source code and/or examples you can download them [here](https://github.com/daveT83/Model-Loader/archive/refs/heads/main.zip).
  - If you only want the dll you can get that [here](https://github.com/daveT83/Model-Loader/raw/main/Model%20Loader/Model%20Loader/bin/Debug/Model%20Loader.dll).

## What do I do if I need to dynamically load an unsupported type?
  There is a simple solution to this that is built into the basic functionality of the dll. If you need to add a new ```Type``` all you need to do is extend the ```TypeConverter``` class. Please see the code below.
  
  ```c#
    public class CustomTypeConverter : TypeConverter
    {
        //Convert from a string to your new type
        //**NOTE** the naming convertion for this method must be ConvertTo{Type} and must return Type and accept one parameter being a string value
        public MyNewType ConvertToMyNewType(string value)
        {
            return Convert.ToMyNewType(value);
        }
        
        //convert from your new type to a string
        //**NOTE** the naming convertion for this method must be ConvertFrom{Type} and must return string and accept one parameter being a {Type} value
        public string ConvertFromMyNewType(MyNewType value)
        {
            return value.ToString();
        }
    }
  
    public class OtherClass
    {
        public void CallConvertToType(string value, Type type)
        {
            CustomTypeConverter customTypeConverter = new CustomTypeConverter();

            customTypeConverter.ConvertToType(value, type, typeof(CustomTypeConverter));
        }

        public void CallConvertFromType(string value, Type type)
        {
            CustomTypeConverter customTypeConverter = new CustomTypeConverter();

            customTypeConverter.ConvertFromType(value, type, typeof(CustomTypeConverter));
        }
    }
  ```
  
  
## What if I don't like how an already supported type is handled?
  In this case you would just need to override that method and implement that logic you want it to use to either convert to a given type or convert a given type to a string. See below.
  
  ```c#
    public class CustomTypeConverter : TypeConverter
    {
        //Overrides the ConvertToInt method
        public override int ConvertToInt(string value)
        {
            return Int32.Parse(value);
        }
        
        //Overrides the ConverFromInt method
        public override string ConvertFromInt(Int value)
        {
            return value.ToString();
        }
    }
  
    public class OtherClass
    {
        public void CallConvertToType(string value, Type type)
        {
            CustomTypeConverter customTypeConverter = new CustomTypeConverter();

            customTypeConverter.ConvertToType(value, type, typeof(CustomTypeConverter));
        }

        public void CallConvertFromType(string value, Type type)
        {
            CustomTypeConverter customTypeConverter = new CustomTypeConverter();

            customTypeConverter.ConvertFromType(value, type, typeof(CustomTypeConverter));
        }
    }
```

# Model-Loader

## Summary:
Dynamically loads values into a model, pulls values out of a model, loads a flat file file into a List<T> and writes a flat file based on a List<T>

This accepts a Dictionary<string,string> with the Keys being the field names and the values being the values to load.
Support for auto loading from a flat file and from the arguments is built in.
For an example please see the included example project.

This has support for all built in type for c#, but if you need to add additional Types you can extend the TypeConverter class to add your own conversions from a string value to another Type. Best practice would be to follow the existing naming convention of ConvertTo<T> and only requiring one parameter, a string value. Please see a list below for a list of all supported types.
  
  
## Supported Types:
  - [Microsoft's Official Built in Types](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/built-in-types)
  - Lists
  - DateTime

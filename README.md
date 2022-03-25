# Model-Loader
Dynamically loads values into a model, pulls values out of a model, loads a csv file into a List<Models> and writes a csv based on a List<Model>

This accepts a Dictionary<string,string> with the Keys being the field names and the values being the values to load.
Support for auto loading from a flat file and from the arguments is built in.
For an Example plese see the included example project.

This has support for all built in type for c#, but if you need to add additional Types you can extend the TypeConverter class to add your own conversions from a string value to another Type. Best practice would be to follow the existing naming convention of ConvertTo<Type> and only requiring one parameter, a string value.

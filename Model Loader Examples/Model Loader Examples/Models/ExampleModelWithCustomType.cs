using System;

namespace Model_Loader_Examples.Models
{
    public class ExampleModelWithCustomType
    {
        public string Label { get; set; }
        public DateTime Date { get; set; }
        public CustomType MyCustomType { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace NovelApp.Models.BookGwModels
{
    public class RequestParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public RequestParameter(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}

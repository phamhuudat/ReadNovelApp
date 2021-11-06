using System;
using System.Collections.Generic;
using System.Text;

namespace NovelApp.Models.BookGwModels
{
    public class RequestParameter
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public RequestParameter(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}

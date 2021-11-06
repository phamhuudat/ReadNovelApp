using System;
using System.Collections.Generic;
using System.Text;

namespace NovelApp.Models.BookGwModels
{
    public class ResponseObject<TData>
    {
        public TData Data { get; set; }
    }
}

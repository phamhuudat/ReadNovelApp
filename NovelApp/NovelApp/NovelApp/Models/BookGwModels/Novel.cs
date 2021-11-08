﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NovelApp.Configurations;

namespace NovelApp.Models.BookGwModels
{
    public class Novel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public int Views { get; set; }
        public string Image { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public string LastChapter { get; set; }
        public DateTime UpdeTime { get; set; }
        public List<string> Tags { get; set; }
        public string PathImage => AppSettings.BookGatewayUrl + "/" + Image;
        public string TagString
        {
            get
            {
                string tags="";
                if(Tags!=null && Tags.Any())
                {
                    foreach(var tag in Tags)
                    tags += $"#{tag}";
                }
                return tags;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using NovelApp.Models.Enums;

namespace NovelApp.Helpers
{
    public class TextSizeHelper
    {
        public TextSizeHelper()
        {
        }
        /// <summary>
        /// Định nghĩa size trong ứng dụng
        /// </summary>
        public static Dictionary<TextSize, Dictionary<CharSize, int>> TextSizeMode { get; } = new Dictionary<TextSize, Dictionary<CharSize, int>>()
        {
            { TextSize.Smallest, new Dictionary<CharSize, int>(){

                {CharSize.Normal, 15},
                {CharSize.Small, 10}
            }},
            { TextSize.Smaller, new Dictionary<CharSize, int>(){

                {CharSize.Normal, 20},
                {CharSize.Small, 15}
            }},
            { TextSize.Small, new Dictionary<CharSize, int>(){

                {CharSize.Normal, 25},
                {CharSize.Small, 20}
            }},
            { TextSize.Normal, new Dictionary<CharSize, int>(){

                {CharSize.Normal, 30},
                {CharSize.Small, 25}
            }},
            { TextSize.Large, new Dictionary<CharSize, int>(){

                {CharSize.Normal, 35},
                {CharSize.Small, 30}
            }},
            { TextSize.Largest, new Dictionary<CharSize, int>(){

                {CharSize.Normal, 40},
                {CharSize.Small, 35}
            }},
        };
    }
}

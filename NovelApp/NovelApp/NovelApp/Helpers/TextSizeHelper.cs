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

                {CharSize.Normal, 12},
                {CharSize.Small, 10},
                {CharSize.Buff,0}
            }},
            { TextSize.Smaller, new Dictionary<CharSize, int>(){

                {CharSize.Normal, 13},
                {CharSize.Small, 15},
                {CharSize.Buff,8}
            }},
            { TextSize.Small, new Dictionary<CharSize, int>(){

                {CharSize.Normal, 15},
                {CharSize.Small, 20},
                {CharSize.Buff,10}
            }},
            { TextSize.Normal, new Dictionary<CharSize, int>(){

                {CharSize.Normal, 20},
                {CharSize.Small, 25},
                 {CharSize.Buff,25}
            }},
            { TextSize.Large, new Dictionary<CharSize, int>(){

                {CharSize.Normal, 25},
                {CharSize.Small, 30},
                 {CharSize.Buff,30}
            }},
            { TextSize.Largest, new Dictionary<CharSize, int>(){

                {CharSize.Normal, 30},
                {CharSize.Small, 35},
                 {CharSize.Buff,40}
            }},
        };
    }
}

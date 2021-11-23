using System;
using System.Collections.Generic;
using System.Diagnostics;
using NovelApp.Models.Enums;

namespace NovelApp.Helpers
{
    public class TextSizeHelper
    {
        public TextSizeHelper()
        {
        }
        /// <summary>
        /// Size = 13 smaller
        /// </summary>
        public static string CharUpWidthLargest = "W"; // = 12.2
        public static string CharUpWidthLarger = "Mm"; // = 11.43
        //public static string CharUpWidthLarge1 = "l"; //=10.7
        public static string CharUpWidthLarge = "O"; // =10.3
        public static string CharUpWidthNormal = "GQ"; // =9.9
        public static string CharUpWidthSmall = "CHNUw"; //=9.5
        public static string CharUpWidthSmaller = "DR"; //=9.14
        public static string CharUpWidthSmallest = "ABEKSVXY"; //=8.76
        public static string CharUpWidthTiny = "P";//=8.38
        public static string CharUpWidthTinier = "FTZ";//=8
        /// <summary>
        /// Char Down
        /// </summary>
        public static string CharDownWidthLargest = "Ladeghnopq01234789?";//=7.24

        public static string CharDownWidthLarger = "bu56";//=6.9

        public static string CharDownWidthLarge = "Jcksvxyz";//=6.5

        public static string CharDownWidthNormal = "\"";//=4.6

        public static string CharDownWidthSmall = "r-";//=4.2

        public static string CharDownWidthSmaller = "Ift.,!:";//=3.4

        public static string CharDownWidthSmallest = "ijl ";//=3.04

        public static string CharDownWidthTiny = "\';"; //2.7



        /// <summary>
        /// Tỉ lệ độ rộng của kí tự tương ứng với font
        /// </summary>
        public static Dictionary<TextFont, Dictionary<string, double>> TextWidthRatio { get; } =

        new Dictionary<TextFont, Dictionary<string, double>>()
        {
            {  TextFont.Arial, new Dictionary<string, double>()
               {
                //UpChar
                {CharUpWidthLargest, 12.2/13 },
                {CharUpWidthLarger, 11.43/13 },
                //{CharUpWidthLarge1, 10.7/13 },
                {CharUpWidthLarge, 10.3/13 },
                {CharUpWidthNormal, 9.9/13 },
                {CharUpWidthSmall, 9.5/13},
                {CharUpWidthSmaller, 9.14/13 },
                {CharUpWidthSmallest, 8.76/13 },
                {CharUpWidthTiny, 8.38/13 },
                {CharUpWidthTinier, 8/13 },
                //DownChar
                {CharDownWidthLargest, 7.24/13 },
                {CharDownWidthLarger, 6.9/13 },
                {CharDownWidthLarge, 6.5/13 },
                {CharDownWidthNormal, 4.6/13 },
                {CharDownWidthSmall, 4.2/13},
                {CharDownWidthSmaller, 3.4/13 },
                {CharDownWidthSmallest, 3.04/13 },
                {CharDownWidthTiny, 2.7/13 },
               } },
               { TextFont.Roboto , new Dictionary<string, double>()
               {
                {CharUpWidthLargest, 0.96*12.2/13 },
                {CharUpWidthLarger, 0.96*11.43/13 },
                //{CharUpWidthLarge1, 0.96*10.7/13 },
                {CharUpWidthLarge, 0.96*10.3/13 },
                {CharUpWidthNormal, 0.96*9.9/13 },
                {CharUpWidthSmall, 0.96*9.5/13},
                {CharUpWidthSmaller, 0.96*9.14/13 },
                {CharUpWidthSmallest, 0.96*8.76/13 },
                {CharUpWidthTiny, 0.96*8.38/13 },
                {CharUpWidthTinier, 0.96*8/13 },
                //DownChar
                {CharDownWidthLargest, 0.96*7.24/13 },
                {CharDownWidthLarger, 0.96*6.9/13 },
                {CharDownWidthLarge, 0.96*6.5/13 },
                {CharDownWidthNormal, 0.96*4.6/13 },
                {CharDownWidthSmall, 0.96*4.2/13},
                {CharDownWidthSmaller, 0.96*3.4/13 },
                {CharDownWidthSmallest, 0.96*3.04/13 },
                {CharDownWidthTiny, 0.96*2.7/13 },
                ////UpChar
                //{CharUpWidthLargest, .9 },
                //{CharUpWidthLarger, .85 },
                //{CharUpWidthLarge1, .79 },
                //{CharUpWidthLarge, .76 },
                //{CharUpWidthNormal, .73 },
                //{CharUpWidthSmall, .7},
                //{CharUpWidthSmaller, .7 },
                //{CharUpWidthSmallest, .67 },
                //{CharUpWidthTiny, .61 },
                //{CharUpWidthTinier, .6 },
                ////DownChar
                //{CharDownWidthLargest, .54 },
                //{CharDownWidthLarger, .51 },
                //{CharDownWidthLarge, .48 },
                //{CharDownWidthNormal, .34 },
                //{CharDownWidthSmall, .31},
                //{CharDownWidthSmaller, .25 },
                //{CharDownWidthSmallest, .22 },
                //{CharDownWidthTiny, .2 },
               } },
               { TextFont.VnTime , new Dictionary<string, double>()
               {
                   {CharUpWidthLargest, 1.09*12.2/13 },
                {CharUpWidthLarger, 1.09*11.43/13 },
                //{CharUpWidthLarge1, 1.09*10.7/13 },
                {CharUpWidthLarge, 1.09*10.3/13 },
                {CharUpWidthNormal, 1.09*9.9/13 },
                {CharUpWidthSmall, 1.09*9.5/13},
                {CharUpWidthSmaller, 1.09*9.14/13 },
                {CharUpWidthSmallest, 1.09*8.76/13 },
                {CharUpWidthTiny, 1.09*8.38/13 },
                {CharUpWidthTinier, 1.09*8/13 },
                //DownChar
                {CharDownWidthLargest, 1.09*7.24/13 },
                {CharDownWidthLarger, 1.09*6.9/13 },
                {CharDownWidthLarge, 1.09*6.5/13 },
                {CharDownWidthNormal, 1.09*4.6/13 },
                {CharDownWidthSmall, 1.09*4.2/13},
                {CharDownWidthSmaller, 1.09*3.4/13 },
                {CharDownWidthSmallest, 1.09*3.04/13 },
                {CharDownWidthTiny, 1.09*2.7/13 },
                ////UpChar
                //{CharUpWidthLargest, 1.03 },
                //{CharUpWidthLarger, .96 },
                //{CharUpWidthLarge1, .89 },
                //{CharUpWidthLarge, .86 },
                //{CharUpWidthNormal, .83 },
                //{CharUpWidthSmall, .8},
                //{CharUpWidthSmaller, .76 },
                //{CharUpWidthSmallest, .73 },
                //{CharUpWidthTiny, .7 },
                //{CharUpWidthTinier, .68 },
                ////DownChar
                //{CharDownWidthLargest, .61 },
                //{CharDownWidthLarger, .58 },
                //{CharDownWidthLarge, .55 },
                //{CharDownWidthNormal, .38 },
                //{CharDownWidthSmall, .35},
                //{CharDownWidthSmaller, .28 },
                //{CharDownWidthSmallest, .25 },
                //{CharDownWidthTiny, .23 },
               }

            }
        };
        /// <summary>
        /// tỉ lệ chiều cao của kí tự tương ứng font
        /// </summary>
        public static Dictionary<TextFont, double> TextHeightRatio =

        new Dictionary<TextFont, double>()
        {
            {  TextFont.Arial, 1.33},
            {  TextFont.Roboto, 1.26},
            {  TextFont.VnTime, 1.14},
        };
        /// <summary>
        /// Tính độ rộng của từ
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static double GetWidthWord(string word, TextSize textSize, TextFont font)
        {
            var chars = word.ToCharArray();
            var widthRadito = TextWidthRatio[font];
            var size = TextSizeMode[textSize][CharSize.Normal];
            double widthWord = 0;
            foreach (var item in chars)
            {
                var isContaints = false;
                double width = 0;
                foreach (var radito in widthRadito)
                {

                    if (radito.Key.Contains(item.ToString()))
                    {
                        isContaints = true;
                        widthWord += radito.Value * size;
                        width = radito.Value * size;
                        break;
                    }
                }
                if (string.IsNullOrWhiteSpace(item.ToString()))
                {
                    Debug.WriteLine("Kí tự space " + width);
                }
                if (!isContaints)
                {
                    Debug.WriteLine("Kí tự chưa định nghĩa: " + item);
                }
            }
            return widthWord;
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

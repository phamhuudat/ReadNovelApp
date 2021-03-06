using System;
namespace NovelApp.Models.Enums
{
    public enum SettingMode
    {
        ReadMode,
        Brightness,
        ReadModeColor,
        TextSize,
        Font
    }
    public enum ReadMode
    {
        Scrolling,
        Paging,
        Tapping
    }
    public enum ReadModelColor
    {
        White,
        YellowLight,
        GreenLight,
        GrayLight,
        OrangeLight,
        Black
    }
    public enum TextSize
    {
        Smallest,
        Smaller,
        Small,
        Normal,
        Large,
        Largest
    }
    public enum CharSize
    {
        Small,
        Normal,
        Buff
    }
    public enum TextFont
    {
        Arial,
        Roboto,
        VnTime
    }
    public enum PageType
    {
        OnePage,
        TwoPages,
        ThreePages
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace NovelApp.Views.ReadBook
{
    public partial class ReadBookPageView : ContentView
    {
        /// <summary>
        /// Scroll to
        /// </summary>
        public static readonly BindableProperty ScrollToProperty = BindableProperty.Create(
            propertyName: nameof(ScrollTo),
            returnType: typeof(object),
            declaringType: typeof(ReadBookPageView),
            defaultValue: 0,
            defaultBindingMode: BindingMode.OneWay,
            propertyChanged: ScrollToPropertyChanged
        );
        public object ScrollTo
        {
            get { return base.GetValue(ScrollToProperty); }
            set { base.SetValue(ScrollToProperty, value); }
        }
        private async static void ScrollToPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        { 
            var control = bindable as ReadBookPageView;
            if (control != null)
            {
                if (double.TryParse((newValue + "").ToString(), out var value))
                   await  control.ContentNovel.TranslateTo(0,- value);
                else
                   await control.ContentNovel.TranslateTo(0, 0);
            }
        }

        /// <summary>
        /// Set background page
        /// </summary>
        public static readonly BindableProperty PageBackgroundColorProperty = BindableProperty.Create(
            propertyName: nameof(PageBackgroundColor),
            returnType: typeof(object),
            declaringType: typeof(ReadBookPageView),
            defaultValue: Color.White,
            defaultBindingMode: BindingMode.OneWay,
            propertyChanged: PageBackgroundColorPropertyChanged
        );
        public object PageBackgroundColor
        {
            get { return base.GetValue(PageBackgroundColorProperty); }
            set { base.SetValue(PageBackgroundColorProperty, value); }
        }
        private static void PageBackgroundColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as ReadBookPageView;
            if (control != null)
            {
                control.Page.BackgroundColor = (Color) newValue;
            }
        }
        /// <summary>
        /// Set textcolor
        /// </summary>
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
            propertyName: nameof(TextColor),
            returnType: typeof(object),
            declaringType: typeof(ReadBookPageView),
            defaultValue: Color.Black,
            defaultBindingMode: BindingMode.OneWay,
            propertyChanged: TextColorPropertyChanged
        );
        public object TextColor
        {
            get { return base.GetValue(TextColorProperty); }
            set { base.SetValue(TextColorProperty, value); }
        }
        private static void TextColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as ReadBookPageView;
            if (control != null)
            {
                control.ContentNovel.TextColor = (Color)newValue;

            }
        }
        /// <summary>
        /// Set TextFontFamily
        /// </summary>
        public static readonly BindableProperty TextFontFamilyProperty = BindableProperty.Create(
           propertyName: nameof(TextFontFamily),
           returnType: typeof(string),
           declaringType: typeof(ReadBookPageView),
           defaultValue: "AC",
           defaultBindingMode: BindingMode.OneWay,
           propertyChanged: TextFontFamilyPropertyChanged
       );
        public string TextFontFamily
        {
            get { return base.GetValue(TextFontFamilyProperty).ToString(); }
            set { base.SetValue(TextFontFamilyProperty, value); }
        }
        private static void TextFontFamilyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as ReadBookPageView;
            if (control != null)
            {
                control.NameChapter.FontFamily = newValue.ToString();
            }
        }
        /// <summary>
        /// Set Title
        /// </summary>
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(
           propertyName: nameof(Title),
           returnType: typeof(string),
           declaringType: typeof(ReadBookPageView),
           defaultValue: "",
           defaultBindingMode: BindingMode.OneWay,
           propertyChanged: TitlePropertyChanged
       );
        public string Title
        {
            get { return base.GetValue(TitleProperty).ToString(); }
            set { base.SetValue(TitleProperty, value); }
        }
        private static void TitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as ReadBookPageView;
            if (control != null)
            {
                control.NameChapter.Text = newValue.ToString();
            }
        }
        /// <summary>
        /// FontSize
        /// </summary>
        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
           propertyName: nameof(FontSize),
           returnType: typeof(object),
           declaringType: typeof(ReadBookPageView),
           defaultValue: 15,
           defaultBindingMode: BindingMode.OneWay,
           propertyChanged: FontSizeChanged
       );
        public object FontSize
        {
            get { return base.GetValue(FontSizeProperty).ToString(); }
            set { base.SetValue(FontSizeProperty, value); }
        }
        private static void FontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as ReadBookPageView;
            if (control != null)
            {
                control.ContentNovel.FontSize = int.Parse(newValue.ToString());
            }
        }
        /// <summary>
        /// CountPage
        /// </summary>
        public static readonly BindableProperty CountPageProperty = BindableProperty.Create(
           propertyName: nameof(CountPage),
           returnType: typeof(object),
           declaringType: typeof(ReadBookPageView),
           defaultValue: 1,
           defaultBindingMode: BindingMode.OneWay,
           propertyChanged: CountPagePropertyChanged
       );
        public object CountPage
        {
            get { return base.GetValue(CountPageProperty).ToString(); }
            set { base.SetValue(CountPageProperty, value); }
        }
        private static void CountPagePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as ReadBookPageView;
            if (control != null)
            {
                control.CountPageLb.Text = newValue.ToString();
            }
        }

        public ReadBookPageView()
        {
            InitializeComponent();
        }

        void ScrollBook_SizeChanged(System.Object sender, System.EventArgs e)
        {
            var scroll = sender as ScrollView;
            Debug.WriteLine($"Height Scroll to {scroll.Height}");
            Debug.WriteLine($"Width Scroll to {scroll.Width}");

        }

        void ContentNovel_SizeChanged(System.Object sender, System.EventArgs e)
        {
            var label = sender as Label;

            Debug.WriteLine($"Height Lable {label.Text} to {label.Height}");
            Debug.WriteLine($"Width Label {label.Text} to {label.Width}");

        }
    }
}

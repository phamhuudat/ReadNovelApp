using NovelApp.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NovelApp.DependencyServices
{
    public interface IAppTheme
    {
        void SetAppTheme(Theme theme);
    }
}

using NovelApp.Models.Enums;
using System.Drawing;

namespace NovelApp.DependencyServices
{
    public interface IAppTheme
    {
        void SetAppTheme(Theme theme);
        void SetStatusBarColor(Color color, bool darkStatusBarTint);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForestalCasablancaApp.Helpers
{
    public static class ThemePicker
    {
        public static void SetTheme() 
        {                 
            switch (Settings.Theme)
            {
                case 0:
                    Application.Current.UserAppTheme = AppTheme.Unspecified;
                    break;
                case 1:
                    Application.Current.UserAppTheme = AppTheme.Light;
                    break;
                case 2:
                    Application.Current.UserAppTheme = AppTheme.Dark;
                    break;
            }
        }
    }
}

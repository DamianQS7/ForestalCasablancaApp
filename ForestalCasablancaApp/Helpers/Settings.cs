using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForestalCasablancaApp.Helpers
{
    public static class Settings
    {

        // Default value for Theme is 0 (System), Light = 1, Dark = 2
        const int theme = 0;

        public static int Theme
        {
            get => Preferences.Get(nameof(Theme), theme);
            set => Preferences.Set(nameof(Theme), value);
        }
    }
}

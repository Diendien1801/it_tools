using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace it_tools.BusinessLogic.Services
{
    class SettingsService
    {
        private const string TokenKey = "AuthToken";

        public static void SaveToken(string token)
        {
            ApplicationData.Current.LocalSettings.Values[TokenKey] = token;
        }

        public static string GetToken()
        {
            return ApplicationData.Current.LocalSettings.Values[TokenKey] as string;
        }

        public static void ClearToken()
        {
            ApplicationData.Current.LocalSettings.Values.Remove(TokenKey);
        }
    }
}

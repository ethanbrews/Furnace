﻿using Windows.UI.Xaml;
using Furnace.Helpers.LocalSettings;
using System.Linq;

namespace Furnace.Settings
{
    public static class AppSettings
    {
        private static LocalSettingsValue<string> _SelectedTheme = new LocalSettingsValue<string>("SelectedTheme", "System");

        private static LocalSettingsValue<string> selectedAccountEmail = new LocalSettingsValue<string>("SelectedAccountEmail", null);

        public static Yggdrasil.Json.Account.MojangAccount SelectedAccount
        {
            get => App.AccountManager.MojangAccounts.Where(x => (x.User.Email == (selectedAccountEmail.Value ?? "")) || (x.User.Username == (selectedAccountEmail.Value ?? ""))).FirstOrDefault();
            set => selectedAccountEmail.Value = value?.User?.Email ?? value?.User?.Username;
        }

        public static ElementTheme SelectedTheme
        {
            get
            {
                switch (_SelectedTheme.Value)
                {
                    case "Light":
                        return ElementTheme.Light;
                    case "Dark":
                        return ElementTheme.Dark;
                    default:
                        return ElementTheme.Default;
                }
            }
            set
            {
                switch (value)
                {
                    case ElementTheme.Dark:
                        _SelectedTheme.Value = "Dark";
                        break;
                    case ElementTheme.Light:
                        _SelectedTheme.Value = "Light";
                        break;
                    default:
                        _SelectedTheme.Value = "System";
                        break;
                }

                ((App)Application.Current).ReloadTheme();
            }
        }
    }
}
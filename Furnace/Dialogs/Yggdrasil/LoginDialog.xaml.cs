using Furnace.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Furnace.Dialogs.Yggdrasil
{

    public sealed partial class LoginDialog : ContentDialog
    {

        private enum ErrorMessages
        {
            CONNECTION, CREDENTIALS, NONE, UNEXPECTED
        }

        public LoginDialog()
        {
            this.InitializeComponent();
        }

        public LoginDialog(string email) : base()
        {
            EmailBox.Text = email;
        }

        private void ShowError(ErrorMessages error)
        {
            InvalidCredentialsMessage.Visibility = Visibility.Collapsed;
            CantConnectMessage.Visibility = Visibility.Collapsed;
            UnexpectedErrorMessage.Visibility = Visibility.Collapsed;
            CredentialsSecureMessage.Visibility = Visibility.Collapsed;
            switch (error)
            {
                case ErrorMessages.CONNECTION:
                    CantConnectMessage.Visibility = Visibility.Visible;
                    break;
                case ErrorMessages.CREDENTIALS:
                    InvalidCredentialsMessage.Visibility = Visibility.Visible;
                    break;
                case ErrorMessages.UNEXPECTED:
                    UnexpectedErrorMessage.Visibility = Visibility.Visible;
                    break;
                default:
                    CredentialsSecureMessage.Visibility = Visibility.Visible;
                    break;
            }
        }

        private async void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            PasswordBox.IsEnabled = false;
            EmailBox.IsEnabled = false;
            LoginProgressRing.IsActive = true;
            LoginButton.IsEnabled = false;

            Furnace.Yggdrasil.Json.Account.MojangAccount account;

            try
            {
                account = await App.AccountManager.API_Authenticate_Async(EmailBox.Text, PasswordBox.Password);
            }
            catch (Exception ex)
            {

                if (ex is Furnace.Yggdrasil.YggdrasilError)
                    ShowError(ErrorMessages.CREDENTIALS);
                else if (ex is System.Net.Http.HttpRequestException)
                    ShowError(ErrorMessages.CONNECTION);
                else
                    ShowError(ErrorMessages.UNEXPECTED);

                PasswordBox.IsEnabled = true;
                EmailBox.IsEnabled = true;
                LoginButton.IsEnabled = true;
                LoginProgressRing.IsActive = false;


                return;
            }


            // Remove existing account by comparing emails and ensuring they are not null
            var existing = App.AccountManager.MojangAccounts.Where(x => x.User?.Email == account.User?.Email).FirstOrDefault();
            if (existing != null && account.User?.Email != null)
                App.AccountManager.MojangAccounts.Remove(existing);

            // Add account to account manager. Then save it to local storage
            App.AccountManager.MojangAccounts.Add(account);
            await App.AccountManager.SaveMojangAccountsToFileAsync();
            this.Hide();

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) => this.Hide();

        private void Email_PasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != Windows.System.VirtualKey.Enter)
                return;

            if (sender == EmailBox)
                PasswordBox.Focus(FocusState.Keyboard);
            else if (sender == PasswordBox)
                LogInButton_Click(null, null);
        }
    }
}

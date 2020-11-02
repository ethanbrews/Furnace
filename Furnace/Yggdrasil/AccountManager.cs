using Furnace.Helpers;
using Furnace.Yggdrasil.Json.Authenticate;
using Furnace.Yggdrasil.Json.Error;
using Microsoft.AppCenter.Crashes;
using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Web.Http;

namespace Furnace.Yggdrasil
{
    public class AccountManager
    {
        private static Uri ApiEndpoint = new Uri("https://authserver.mojang.com");
        public static Uri AuthenticateEndpoint = new Uri(ApiEndpoint, "authenticate");
        public static Uri RefreshEndpoint = new Uri(ApiEndpoint, "refresh");
        public static Uri ValidateEndpoint = new Uri(ApiEndpoint, "validate");
        public static Uri InvalidateEndpoint = new Uri(ApiEndpoint, "invalidate");

        public ObservableCollection<Yggdrasil.Json.Account.MojangAccount> MojangAccounts = new ObservableCollection<Json.Account.MojangAccount>();

        public async Task<Json.Account.MojangAccount> API_Authenticate_Async(string username, string password)
        {
            var data = (new Json.Authenticate.AuthenticateRequest
            {
                Agent = new Agent(), // Use defaults
                Username = username,
                Password = password,
                RequestUser = true
            }).ToJson();

            var response = await AppHttpClient.Client.PostAsync(
                AuthenticateEndpoint,
                new HttpStringContent(data, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json")
            );

            if (!response.IsSuccessStatusCode)
                throw new YggdrasilError(Json.Error.YggdrasilError.FromJson(await response.Content.ReadAsStringAsync()));

            return Json.Account.MojangAccount.FromJson(await response.Content.ReadAsStringAsync());
        }

        public async Task LoadMojangAccountsFromFileAsync()
        {
            try
            {
                MojangAccounts = JsonConvert.DeserializeObject<ObservableCollection<Yggdrasil.Json.Account.MojangAccount>>(await FileIO.ReadTextAsync(await StorageHelper.GetLocalFileAsync(@"Yggdrasil.json")));
            } catch (Exception ex)
            {
                if (!(ex is FileNotFoundException))
                    Crashes.TrackError(ex);
            }
        }

        public async Task SaveMojangAccountsToFileAsync() =>
            await FileIO.WriteTextAsync(await StorageHelper.GetLocalFileAsync(@"Yggdrasil.json"), JsonConvert.SerializeObject(MojangAccounts));
        
    }
}

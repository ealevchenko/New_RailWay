using MessageLog;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace WebApiClient
{
    public class WebApi
    {
        private eventID eventID = eventID.WebApi;

        private string APP_PATH;
        private string userName;
        private string password;
        private static string token;

        public WebApi(string url, string userName, string password) {
            this.APP_PATH = url;
            this.userName = userName;
            this.password = password;
            Dictionary<string, string> tokenDictionary = GetTokenDictionary(userName, password);
            token = tokenDictionary["access_token"];
        }

        // получение токена
        public Dictionary<string, string> GetTokenDictionary(string userName, string password)
        {
            if (String.IsNullOrWhiteSpace(APP_PATH)) return null;
            var pairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>( "grant_type", "password" ), 
                    new KeyValuePair<string, string>( "username", userName ), 
                    new KeyValuePair<string, string> ( "Password", password )
                };
            var content = new FormUrlEncodedContent(pairs);

            using (var client = new HttpClient())
            {
                var response =
                    client.PostAsync(APP_PATH + "/Token", content).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                // Десериализация полученного JSON-объекта
                Dictionary<string, string> tokenDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                return tokenDictionary;
            }
        }

        // создаем http-клиента с токеном 
        public HttpClient CreateClient(string accessToken = "")
        {
            var client = new HttpClient();
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }
            return client;
        }

        // получаем информацию о клиенте 
        public string GetUserInfo(string token)
        {
            if (String.IsNullOrWhiteSpace(APP_PATH)) return null;
            using (var client = CreateClient(token))
            {
                var response = client.GetAsync(APP_PATH + "/api/Account/UserInfo").Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        // обращаемся по маршруту api/values 
        public string GetValues(string token)
        {
            if (String.IsNullOrWhiteSpace(APP_PATH)) return null;
            using (var client = CreateClient(token))
            {
                var response = client.GetAsync(APP_PATH + "/api/WagonsTracking?note&st_form&nsost&st_nazn&from=2&st_disl&dt1&dt2&vagonlg_id").Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public string GetApiValues(string api_comand)
        {
            if (String.IsNullOrWhiteSpace(APP_PATH)) return null;
            using (var client = CreateClient(token))
            {
                var response = client.GetAsync(APP_PATH + api_comand).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public T JSONStringToClass<T>(string JSONString)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(JSONString)) return default(T);
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(T));
                byte[] byteArray = Encoding.UTF8.GetBytes(JSONString);
                //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
                MemoryStream stream = new MemoryStream(byteArray);
                return (T)jsonSerializer.ReadObject(stream);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("JSONStringToClass<T:{0}>(JSONString={1})", typeof(T).Name, JSONString), eventID);
                return default(T);
            }
        }

        public T GetJSONSelect<T>(string api_comand)
        {
            return JSONStringToClass<T>(GetApiValues(api_comand));
        }

    }
}

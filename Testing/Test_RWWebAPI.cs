using EFMT.Concrete;
using EFMT.Entities;
using Newtonsoft.Json;
using RWWebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApiClient;

namespace Testing
{
    public class Test_RWWebAPI
    {
        private const string APP_PATH = "http://umtrans.com.ua:81";
        private static string token;

        
        public Test_RWWebAPI() { }

        public void RWReference_CorrectCargo() {
            RWReference rwr = new RWReference();
            ReferenceCargo new_rc = rwr.GetReferenceCargoOfCodeETSNG(32203);
            Console.WriteLine("cargo = {0}, corect cargo = {1}",32203,new_rc.etsng);
        }

        public void Reference_CorrectCargo() {
            Reference r = new Reference();
            Cargo new_rc = r.GetCargoOfCodeETSNG(32203);
            Console.WriteLine("cargo = {0}, corect cargo = {1}", 32203, new_rc.code_etsng);
        }

        public void Reference_StationsOfCode()
        {
            Reference r = new Reference();
            Stations stat = r.GetStationsOfCode(46700);
            Console.WriteLine("Stations = {0}, Stations = {1}", 46700, stat.code_cs);
        }

        public void Wagons_GetKometaVagonSob()
        {
            Wagons w = new Wagons();
            List<KometaVagonSob> stat = w.GetKometaVagonSob(68823137);
            Console.WriteLine("Stations = {0}, Count = {1}", 68823137, stat.Count());
        }

        public void Wagons_GetKometaVagonSob(int num, DateTime dt)
        {
            Wagons w = new Wagons();
            KometaVagonSob stat = w.GetKometaVagonSob(num, dt);
            Console.WriteLine("Stations = {0}, Count = {1}", num, stat.N_VAGON);
        }

        public void Wagons_GetSobstvForNakl()
        {
            Wagons w = new Wagons();
            List<KometaSobstvForNakl> stat = w.GetSobstvForNakl();
            Console.WriteLine("Count = {0}", stat.Count());
        }

        public void Wagons_GetSobstvForNakl(int nak)
        {
            Wagons w = new Wagons();
            KometaSobstvForNakl stat = w.GetSobstvForNakl(nak);
            Console.WriteLine("NPLAT = {0}", stat.NPLAT);
        }

        public void Wagons_GetGruzSP(int kod)
        {
            Wagons w = new Wagons();
            PromGruzSP stat = w.GetGruzSP(kod);
            Console.WriteLine("NAME_GR = {0}", stat.NAME_GR);
        }

        public void Wagons_GetGruzSPToTarGR(int? kod, bool corect)
        {
            Wagons w = new Wagons();
            PromGruzSP stat = w.GetGruzSPToTarGR(kod, corect);
            Console.WriteLine("NAME_GR = {0}", stat.NAME_GR);
        }

        public void Wagons_GetSTPR1GR()
        {
            Wagons w = new Wagons();
            List<NumVagStpr1Gr> stat = w.GetSTPR1GR();
            Console.WriteLine("Count = {0}", stat.Count());
        }

        public void Wagons_GetSTPR1GR(int kod)
        {
            Wagons w = new Wagons();
            NumVagStpr1Gr stat = w.GetSTPR1GR(kod);
            Console.WriteLine("Count = {0}", stat.GR);
        }

        public void Reference_GetCountryOfCodeSNG()
        {
            Reference r = new Reference();
            Countrys stat = r.GetCountryOfCodeSNG(20);
            Console.WriteLine("Stations = {0}, Stations = {1}", "Маскали", stat.country);
        }

        public void WebAPIToken()
        {

            //Console.WriteLine("Введите логин:");
            //string userName = Console.ReadLine();
 
            //Console.WriteLine("Введите пароль:");
            //string password = Console.ReadLine();
 
            //var registerResult = Register(userName, password);
 
            //Console.WriteLine("Статусный код регистрации: {0}", registerResult);
            string userName ="RailWayAMKR";
            string password ="Lbvrf_2709";
 
            Dictionary<string, string> tokenDictionary = GetTokenDictionary(userName, password);
            token = tokenDictionary["access_token"];
 
            Console.WriteLine();
            Console.WriteLine("Access Token:");
            Console.WriteLine(token);
 
            Console.WriteLine();
            string userInfo = GetUserInfo(token);
            Console.WriteLine("Пользователь:");
            Console.WriteLine(userInfo);
 
            Console.WriteLine();
            string values = GetValues(token);
            Console.WriteLine("Values:");
            Console.WriteLine(values);
 
            Console.Read();

        }

        // регистрация
        static string Register(string email, string password)
        {
            var registerModel = new
            {
                Email = email,
                Password = password,
                ConfirmPassword = password
            };
            using (var client = new HttpClient())
            {
                var response = client.PostAsJsonAsync(APP_PATH + "/api/Account/Register", registerModel).Result;
                return response.StatusCode.ToString();
            }
        }

        // получение токена
        static Dictionary<string, string> GetTokenDictionary(string userName, string password)
        {
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
        static HttpClient CreateClient(string accessToken = "")
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
        static string GetUserInfo(string token)
        {
            using (var client = CreateClient(token))
            {
                var response = client.GetAsync(APP_PATH + "/api/Account/UserInfo").Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        // обращаемся по маршруту api/values 
        static string GetValues(string token)
        {
            using (var client = CreateClient(token))
            {
                var response = client.GetAsync(APP_PATH + "/api/WagonsTracking?note&st_form&nsost&st_nazn&from=2&st_disl&dt1&dt2&vagonlg_id").Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public void WebAPIMT()
        {
            WebApiClientMetallurgTrans clientMT = new WebApiClientMetallurgTrans();
            //Console.WriteLine(clientMT.wapi.to);
            EFMetallurgTrans efmt = new EFMetallurgTrans();
            Console.WriteLine("Запрос....");
            List<WagonsTrackingMT> list1 =  clientMT.GetWagonsTracking();
            //List<WagonsTrackingMT> list2 = clientMT.GetWagonsTracking(56858111);
            ////List<WagonsTrackingMT> list3 = clientMT.GetWagonsTracking(56858111, DateTime.Now.AddMonths(-1));
            ////List<WagonsTrackingMT> list4 = clientMT.GetWagonsTracking(56858111, DateTime.Now.AddDays(-15), DateTime.Now.AddDays(-5));
            foreach (WagonsTrackingMT wt in list1)
            {
                Console.WriteLine(wt.nvagon);
                //efmt.SaveWagonsTracking(wt);
            }
        }

        public void WebAPIMTStart()
        {

            for (int i = 1; i <= 100; i++)
            {
                //Console.WriteLine("Press any key to start...");
                //Console.ReadKey();
                WebAPIMT();
            }
        }

    }
}

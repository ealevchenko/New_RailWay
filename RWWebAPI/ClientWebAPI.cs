using MessageLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WebAPI
{
    public class WebAPIResponse {
        public string response { get; set; }
        public Exception error { get; set; }
        public WebAPIResponse(string response) {
            this.response = response; this.error = null;
        }
        public WebAPIResponse(Exception error)
        {
            this.response = null; this.error = error;
        }
    }

    public class ClientWebAPI
    {
        protected string url_primary =null;
        protected string url_secondary =null;

        private eventID eventID = eventID.RWWebAPI_ClientWebAPI;

        public ClientWebAPI()
        {
            try
            {
                this.url_primary = ConfigurationManager.AppSettings["url_rwwebapi_primary"].ToString();
                this.url_secondary = ConfigurationManager.AppSettings["url_rwwebapi_secondary"].ToString();
            }
            catch (Exception e)
            {
                e.WriteError(String.Format("Ошибка чтения url_rwwebapi из app.config"), eventID);
            }
        }

        public ClientWebAPI(string url) {
            this.url_primary = url;
        }

        public ClientWebAPI(string url_primary, string url_secondary)
        {
            this.url_primary = url_primary; 
            this.url_secondary = url_secondary;    
        }
        /// <summary>
        /// Выполнить запрос к Web Api по указанному url
        /// </summary>
        /// <param name="api_comand"></param>
        /// <param name="metod"></param>
        /// <param name="accept"></param>
        /// <returns></returns>
        public WebAPIResponse Select(string url, string api_comand, string metod, string accept)
        {
            try
            {
                //String.Format("Выполняем запрос к WebAPI, url:{0}, api_comand {1}, metod {2}, accept {3}", url, api_comand, metod, accept).WriteInformation(eventID);
                HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url + api_comand);
                request.Method = metod;
                request.PreAuthenticate = true;
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Accept = accept;
                try
                {
                    using (System.Net.WebResponse response = request.GetResponse())
                    {
                        try
                        {
                            using (System.IO.StreamReader rd = new System.IO.StreamReader(response.GetResponseStream()))
                            {
                                return new WebAPIResponse(rd.ReadToEnd());
                            }
                        }
                        catch (Exception e)
                        {
                            e.WriteError(String.Format("Ошибка создания StreamReader ответа, команда {0}, метод {1}, accept {2}",api_comand,metod,accept),eventID);
                            return new WebAPIResponse(e);
                        }
                    }
                }
                catch (Exception e)
                {
                    e.WriteError(String.Format("Ошибка получения ответа WebResponse, команда {0}, метод {1}, accept {2}", api_comand, metod, accept), eventID);
                    return new WebAPIResponse(e);
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("Ошибка выполнения метода Select(api_comand={0}, metod={1}, accept={2}", api_comand, metod, accept), eventID);
                return new WebAPIResponse(e);
            }
        }
        /// <summary>
        /// Выполнить запрос к Web Api по доступному url (primary, secondary)
        /// </summary>
        /// <param name="api_comand"></param>
        /// <param name="metod"></param>
        /// <param name="accept"></param>
        /// <returns></returns>
        public string Select(string api_comand, string metod, string accept)
        {
            WebAPIResponse respone;
              respone  = Select(this.url_primary, api_comand, metod, accept);
            if (respone.error != null & this.url_secondary != null) {
                respone = Select(this.url_secondary, api_comand, metod, accept);
            }
            return respone != null ? respone.response : null;
        }
        /// <summary>
        /// Выполнить запрос к Web Api типа Get получить формат JSON
        /// </summary>
        /// <param name="api_comand"></param>
        /// <returns></returns>
        public string GetJSONSelect(string api_comand)
        {
            return Select(api_comand, "GET", "application/json");
        }
        /// <summary>
        /// Преобразовать строку JSON в класс
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="JSONString"></param>
        /// <returns></returns>
        public T JSONStringToClass<T>(string JSONString)
        {
            try { 
            if (String.IsNullOrWhiteSpace(JSONString)) return default(T);
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(T));
            byte[] byteArray = Encoding.UTF8.GetBytes(JSONString);
            //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
            MemoryStream stream = new MemoryStream(byteArray);
            return (T)jsonSerializer.ReadObject(stream);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("JSONStringToClass<T:{0}>(JSONString={1})", typeof(T).Name ,JSONString), eventID);
                return default(T);
            }
        }

        public T GetJSONSelect<T>(string api_comand)
        {
            return JSONStringToClass<T>(GetJSONSelect(api_comand));
        }



    }
}

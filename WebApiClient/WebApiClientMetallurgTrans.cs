using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageLog;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Runtime.Serialization;
using RWConversionFunctions;
using EFMT.Entities;

namespace WebApiClient
{
    //[Serializable]
    //public class WagonsTracking : ISerializable
    //{
    //    public int nvagon { get; set; }
    //    public int? st_disl { get; set; }
    //    public string nst_disl { get; set; }
    //    public int? kodop { get; set; }
    //    public string nameop { get; set; }
    //    public DateTime? dt { get; set; }
    //    public string nst_form { get; set; }
    //    public int? st_form { get; set; }
    //    public string nsost { get; set; } //? int
    //    public int? st_nazn { get; set; }
    //    public string nst_nazn { get; set; }
    //    public int? ntrain { get; set; }
    //    public int? st_end { get; set; }
    //    public string nst_end { get; set; }
    //    public int? idsost { get; set; }
    //    public int? kgr { get; set; }
    //    public string nkgr { get; set; }
    //    public int? kgrp { get; set; }
    //    public decimal? ves { get; set; }
    //    public DateTime? updated { get; set; }
    //    //public string note { get; set; }
    //    public string full_nameop { get; set; }
    //    //public string nquest { get; set; }
    //    public int? kgro { get; set; }
    //    public int? km { get; set; }



    //    public WagonsTracking(SerializationInfo info, StreamingContext context)
    //    {
    //        this.nvagon = (int)info.GetValue("nvagon", typeof(int));//
    //        this.st_disl = (int?)info.GetValue("st_disl", typeof(int?));//
    //        this.nst_disl = (string)info.GetValue("nst_disl", typeof(string));
    //        this.kodop = (int?)info.GetValue("kodop", typeof(int?));//
    //        this.nameop = (string)info.GetValue("nameop", typeof(string));//
    //        this.dt = ((string)info.GetValue("dt", typeof(string))).DateNullConversion();//
    //        this.nst_form = (string)info.GetValue("nst_form", typeof(string));//
    //        this.st_form = (int?)info.GetValue("st_form", typeof(int?));
    //        this.nsost = (string)info.GetValue("nsost", typeof(string));//
    //        this.st_nazn = (int?)info.GetValue("st_nazn", typeof(int?));//
    //        this.nst_nazn = (string)info.GetValue("nst_nazn", typeof(string));//
    //        this.ntrain = (int?)info.GetValue("ntrain", typeof(int?));//
    //        this.st_end = (int?)info.GetValue("st_end", typeof(int?));//
    //        this.nst_end = (string)info.GetValue("nst_end", typeof(string));//
    //        this.idsost = (int?)info.GetValue("idsost", typeof(int?));//
    //        this.kgr = (int?)info.GetValue("kgr", typeof(int?));//
    //        this.nkgr = (string)info.GetValue("nkgr", typeof(string));//
    //        this.kgrp = (int?)info.GetValue("kgrp", typeof(int?));
    //        this.ves = (decimal?)info.GetValue("ves", typeof(decimal?));
    //        this.updated = ((string)info.GetValue("updated", typeof(string))).DateNullConversion();
    //        //this.note = (string)info.GetValue("note", typeof(string));
    //        this.full_nameop = (string)info.GetValue("full_nameop", typeof(string));
    //        //this.nquest = (string)info.GetValue("nquest", typeof(string));
    //        this.kgro = (int?)info.GetValue("kgro", typeof(int?));
    //        this.km = (int?)info.GetValue("km", typeof(int?));
    //    }

    //    public void GetObjectData(SerializationInfo info, StreamingContext context)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public class WebApiClientMetallurgTrans 
    {
        private eventID eventID = eventID.WebApi_ClientMetallurgTrans;
        public WebApi wapi;
        private string url;
        private string user;
        private string psw;
        private string api; 
       
        public WebApiClientMetallurgTrans(){
            try
            {
                this.url = ConfigurationManager.AppSettings["WebApiMTURL"].ToString();
                this.user = ConfigurationManager.AppSettings["WebApiMTUser"].ToString();
                this.psw = ConfigurationManager.AppSettings["WebApiMTPSW"].ToString();
                this.api = ConfigurationManager.AppSettings["WebApiMTApi"].ToString();
                wapi = new WebApi(url, user, psw);
             }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("WebApiClientMetallurgTrans()"), eventID);
            }
        }

        public WebApiClientMetallurgTrans(string url, string user, string psw, string api){
            try
            {
                this.url = url;
                this.user = user;
                this.psw = psw;
                this.api = api;
                wapi = new WebApi(url, user, psw);
             }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("WebApiClientMetallurgTrans(url={0},user={1},psw={2},api={3})",url,user,psw,api), eventID);
            }
        }
        /// <summary>
        /// Получить все вагоны
        /// </summary>
        /// <returns></returns>
        public List<WagonsTracking> GetWagonsTracking()
        {
            return wapi.GetJSONSelect<List<WagonsTracking>>(this.api+"?nvagon=0");
        }

        public List<WagonsTracking> GetWagonsTracking(int num_vag)
        {
            return wapi.GetJSONSelect<List<WagonsTracking>>(this.api + "?nvagon=" + num_vag.ToString());
        }

        public List<WagonsTracking> GetWagonsTracking(int num_vag, DateTime date_start)
        {
            string select = String.Format(this.api + "?nvagon={0}&dt1={1}", num_vag, date_start.ToString("yyyy-MM-dd HH:mm:ss"));
            return wapi.GetJSONSelect<List<WagonsTracking>>(select);
        }

        public List<WagonsTracking> GetWagonsTracking(int num_vag, DateTime date_start, DateTime date_stop)
        {
            string select = String.Format(this.api + "?nvagon={0}&dt1={1}&dt2={2}", num_vag, date_start.ToString("yyyy-MM-dd HH:mm:ss"), date_stop.ToString("yyyy-MM-dd HH:mm:ss"));
            return wapi.GetJSONSelect<List<WagonsTracking>>(select);
        }

        public string GetJSONWagonsTracking()
        {
            return wapi.GetApiValues(this.api + "?nvagon=0");
        }
    }
}

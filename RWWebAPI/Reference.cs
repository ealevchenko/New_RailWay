using MessageLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebAPI;

namespace RWWebAPI
{
    [Serializable]
    public class Cargo: ISerializable
    {
        public int id { get; set; }
        public int code_etsng { get; set; }
        public string name_etsng { get; set; }
        public int code_gng { get; set; }
        public string name_gng { get; set; }
        public int? id_sap { get; set; }

        public Cargo(SerializationInfo info, StreamingContext context)
        {
            this.id = (int)info.GetValue("id", typeof(int));
            this.code_etsng = (int)info.GetValue("code_etsng", typeof(int));
            this.name_etsng = (string)info.GetValue("name_etsng", typeof(string));
            this.code_gng = (int)info.GetValue("code_etsng", typeof(int));
            this.name_gng = (string)info.GetValue("name_etsng", typeof(string));
            this.id_sap = (int?)info.GetValue("id_sap", typeof(int?));
        }

    
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
 	        throw new NotImplementedException();
        }
    }

    [Serializable]
    public class Stations: ISerializable
    {
        public int id { get; set; }
        public int code { get; set; }
        public int? code_cs { get; set; }
        public string station { get; set; }
        public int? id_ir { get; set; }

        public Stations(SerializationInfo info, StreamingContext context)
        {
            this.id = (int)info.GetValue("id", typeof(int));
            this.code = (int)info.GetValue("code", typeof(int));
            this.code_cs = (int?)info.GetValue("code_cs", typeof(int?));
            this.station = (string)info.GetValue("station", typeof(string));
            this.id_ir = (int?)info.GetValue("id_ir", typeof(int?));
        }

    
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
 	        throw new NotImplementedException();
        }
    }

    public class Countrys: ISerializable
    {
        public int id { get; set; }
        public string country { get; set; }
        public string alpha_2 { get; set; }
        public string alpha_3 { get; set; }
        public int code { get; set; }
        public string iso3166_2 { get; set; }
        public int? id_state { get; set; }
        public int? code_europe { get; set; }

        public Countrys(SerializationInfo info, StreamingContext context)
        {
            this.id = (int)info.GetValue("id", typeof(int));
            this.country = (string)info.GetValue("country", typeof(string));
            this.alpha_2 = (string)info.GetValue("alpha_2", typeof(string));
            this.alpha_3 = (string)info.GetValue("alpha_3", typeof(string));
            this.code = (int)info.GetValue("code", typeof(int));
            this.iso3166_2 = (string)info.GetValue("iso3166_2", typeof(string));
            this.id_state = (int?)info.GetValue("id_state", typeof(int?));
            this.code_europe = (int?)info.GetValue("code_europe", typeof(int?));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
    
    public class Reference : ClientWebAPI
    {
        private eventID eventID = eventID.RWWebAPI_Reference;

        public Reference():base(){
        
        }

        public Cargo GetCargoOfCodeETSNG(int code_etsng) {
            return GetJSONSelect<Cargo>(@"reference/cargo/etsng_code/" + code_etsng.ToString());
        }

        public Stations GetStationsOfCode(int code)
        {
            return GetJSONSelect<Stations>(@"reference/stations/code/" + code.ToString());
        }

        public Countrys GetCountryOfCodeSNG(int code_sng)
        {
            return GetJSONSelect<Countrys>(@"reference/countrys/code_sng/" + code_sng.ToString());
        }

        public Countrys GetCountryOfCode(int code_iso)
        {
            return GetJSONSelect<Countrys>(@"reference/countrys/code_iso/" + code_iso.ToString());
        }

    }
}

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
    
    public class Reference : ClientWebAPI
    {
        private eventID eventID = eventID.RWWebAPI_RWReference;

        public Reference():base(){
        
        }

        public Cargo GetCargoOfCodeETSNG(int code_etsng) {
            return GetJSONSelect<Cargo>(@"reference/cargo/etsng_code/" + code_etsng.ToString());
        }

        public Stations GetStationsOfCode(int code)
        {
            return GetJSONSelect<Stations>(@"reference/stations/code/" + code.ToString());
        }
    }
}

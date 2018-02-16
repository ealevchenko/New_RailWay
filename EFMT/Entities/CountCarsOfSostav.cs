using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using RWConversionFunctions;

namespace EFMT.Entities
{
    public class CountCarsOfSostav : ISerializable
    {
        public int IDSostav { get; set; }
        public string CompositionIndex { get; set; }
        public DateTime DateOperation { get; set; }
        public int count_cars { get; set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("IDSostav", this.IDSostav);
            info.AddValue("CompositionIndex", this.CompositionIndex);
            info.AddValue("DateOperation", this.DateOperation);
            info.AddValue("count_cars", this.count_cars);
        }

        
        public CountCarsOfSostav() { 
        
        }

        public CountCarsOfSostav(SerializationInfo info, StreamingContext context)
        {
            this.IDSostav = (int)info.GetValue("IDSostav", typeof(int));//
            this.CompositionIndex = (string)info.GetValue("CompositionIndex", typeof(string));
            this.DateOperation = ((string)info.GetValue("DateOperation", typeof(string))).DateConversion();//
            this.count_cars = (int)info.GetValue("count_cars", typeof(int));
        }
    }
}

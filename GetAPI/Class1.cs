using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GetAPI
{
    public class Class1
    {
        public Class1() {



            var xmlDoc = XDocument.Load("https://api.privatbank.ua/p24api/pubinfo?exchange&coursid=5");
 

            HttpWebRequest htmlr = (System.Net.HttpWebRequest)System.Net.WebRequest.Create("https://api.privatbank.ua/p24api/pubinfo?exchange&coursid=5");

            using (System.Net.WebResponse response = htmlr.GetResponse())
            {
                using (System.IO.StreamReader rd = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    var answer = rd.ReadToEnd();

                }
            }
        }

    }
}

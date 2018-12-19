using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Web.Http;

namespace homework5.Service
{
    public class GetsojsonXml
    {
        public static async Task<XmlDocument> GetWeather(string cityName)
        {
            XmlDocument document = null;
            try
            {
                HttpClient http = new HttpClient();
                string result = await http.GetStringAsync(
                    new Uri("https://www.sojson.com/open/api/weather/xml.shtml?city=" + cityName));
                document = new XmlDocument();
                document.LoadXml(result);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return document;
        }
    }
}

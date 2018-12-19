using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace homework5.Service
{
    public class GetsojsonJson
    {
        public static async Task<RootObject> getWeatherByCityName(string cityName)
        {
            HttpClient http = new HttpClient();
            HttpResponseMessage response = await http.GetAsync(
                new Uri("https://www.sojson.com/open/api/weather/json.shtml?city=" + cityName));
            string result = await response.Content.ReadAsStringAsync();
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RootObject));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            RootObject data = (RootObject)serializer.ReadObject(ms);
            return data;
        }
    }

    [DataContract]
    public class Yesterday
    {
        [DataMember]
        public string date { get; set; }
        [DataMember]
        public string sunrise { get; set; }
        [DataMember]
        public string high { get; set; }
        [DataMember]
        public string low { get; set; }
        [DataMember]
        public string sunset { get; set; }
        [DataMember]
        public double aqi { get; set; }
        [DataMember]
        public string fx { get; set; }
        [DataMember]
        public string fl { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public string notice { get; set; }
    }

    [DataContract]
    public class Forecast
    {
        [DataMember]
        public string date { get; set; }
        [DataMember]
        public string sunrise { get; set; }
        [DataMember]
        public string high { get; set; }
        [DataMember]
        public string low { get; set; }
        [DataMember]
        public string sunset { get; set; }
        [DataMember]
        public double aqi { get; set; }
        [DataMember]
        public string fx { get; set; }
        [DataMember]
        public string fl { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public string notice { get; set; }
    }

    [DataContract]
    public class Data
    {
        [DataMember]
        public string shidu { get; set; }
        [DataMember]
        public double pm25 { get; set; }
        [DataMember]
        public double pm10 { get; set; }
        [DataMember]
        public string quality { get; set; }
        [DataMember]
        public string wendu { get; set; }
        [DataMember]
        public string ganmao { get; set; }
        [DataMember]
        public Yesterday yesterday { get; set; }
        [DataMember]
        public List<Forecast> forecast { get; set; }
    }

    [DataContract]
    public class RootObject
    {
        [DataMember]
        public string date { get; set; }
        [DataMember]
        public string message { get; set; }
        [DataMember]
        public int status { get; set; }
        [DataMember]
        public string city { get; set; }
        [DataMember]
        public int count { get; set; }
        [DataMember]
        public Data data { get; set; }
    }
}
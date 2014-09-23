using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DiaryInfo
{
    // Classes for JSON parsing.
    // Many classes for future.
    [DataContract]
    public class NewComments
    {
        [DataMember(Name = "count")]
        public int Count { get; set; }
    }
    // Main object for JSON
    [DataContract]
    public class DiaryRuInfo
    {
        [DataMember(Name = "newcomments")]
        public NewComments[] NewComments { get; set; }
        [DataMember(Name = "error")]
        public string Error { get; set; }
    }

    public class Client {

        protected async Task<HttpWebResponse> _RequestAsync(String url, String requestMethod, byte[] content)
        {
            ;
        }
        
        public async Task<DiaryRuInfo> GetInfoAsync()
        {
            DiaryRuInfo info = null;
            using (HttpWebResponse response = await this._RequestAsync(URL_INFO, "GET", null))
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(DiaryRuInfo));
                using (Stream oStream = response.GetResponseStream())
                {
                    try
                    {
                        info = (DiaryRuInfo)json.ReadObject(oStream);
                    }
                    catch (Exception e)
                    {
                        info = null;
                    }
                }
            }
            return info;
        }
    }
}
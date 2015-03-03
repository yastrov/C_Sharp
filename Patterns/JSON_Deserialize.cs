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

        public static IList<T> GetObjectListFromJson<T>(HttpWebResponse response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
                throw new WebException(response.StatusDescription);
            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(List<T>));
            using (Stream oStream = response.GetResponseStream())
            {
                return (List<T>)json.ReadObject(oStream);
            }
        }
    }
}

using Newtonsoft.Json;
namespace JSONNETExample
{
    [JsonObject] // Really not needed
    public class PostGetJSONResponse
    {
        public Dictionary<string, PostUnit> Posts { get; set; }
        public string Result { get; set; }
        public string Error { get; set; }
        [JsonProperty("author")]
        public AuthorUnit Author { get; set; } 

        public List<PostUnit> GetPosts()
        {
            List<PostUnit> result = new List<PostUnit>();
            foreach (KeyValuePair<string, PostUnit> kvp in this.Posts)
            {
                kvp.Value.Postid = kvp.Value.Postid.Trim();
                result.Add(kvp.Value);
            }
            return result;
        }
    }
    [JsonObject]
    public class PostUnit
    {
        [JsonProperty("Avatar_path")]
        public string AvatarPath { get; set; }
        public string Author_title { get; set; }
        public string Author_username { get; set; }
        public string Title { get; set; }
  
        public string Message_src { get; set; }
        public string Message_html { get; set; } // No usable?

    }
    [JsonObject]
    public class AuthorUnit
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum AuthorGenderEnum
        {
            Male,
            Female
        }
        [JsonConverter(typeof(LanguageEnumConverter))]
        public enum LanguageEnum
        {
            Russian,
            English
        }
    }

    public class LanguageEnumConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, 
                                         Type objectType,
                                         object existingValue, 
                                         JsonSerializer serializer)
        {
            var value = reader.Value.ToString();
            if (string.Compare(value, "R", true) == 0)
            {
                return AuthorUnit.LanguageEnum.Russian;
            }
            if (string.Compare(value, "E", true) == 0)
            {
                return AuthorUnit.LanguageEnum.English;
            }
            return AuthorUnit.LanguageEnum.Russian;
        }
        public override void WriteJson(JsonWriter writer,
                                        object value,
                                        JsonSerializer serializer)
        {
            var obj = (AuthorUnit.GenderEnum)value;
            writer.WriteValue(value.ToString().Substring(0,1));
        }
        public override bool CanConvert(Type objectType)
        {
            return false;
        }
    }

    public static T GetObjectFromJson<T>(HttpWebResponse response)
    {
        if (response.StatusCode != HttpStatusCode.OK)
            throw new WebException(response.StatusDescription);
        var settings = new JsonSerializerSettings();
        settings.NullValueHandling = NullValueHandling.Ignore;
        using (Stream oStream = response.GetResponseStream())
        {
            StreamReader reader = new StreamReader(oStream);
            return JsonConvert.DeserializeObject<T>(reader.ReadToEnd(), settings);
        }
    }

    public static List<T> GetListOfObjectFromJson<T>(HttpWebResponse response)
    {
        List<T> result = null;
        if (response.StatusCode != HttpStatusCode.OK)
            throw new WebException(response.StatusDescription);
        var settings = new JsonSerializerSettings();
        settings.NullValueHandling = NullValueHandling.Ignore;
        using (Stream oStream = response.GetResponseStream())
        {
            StreamReader reader = new StreamReader(oStream);
            result = (List<T>)Newtonsoft.Json.JsonConvert.DeserializeObject(reader.ReadToEnd(), typeof(List<T>), settings);
        }
        return result;
    }
}
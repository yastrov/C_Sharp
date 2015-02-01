using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace JsonNetHelpers
{
    /// <summary>
    /// JSON.NET helpers
    /// </summary>
    public static class JsonNetHelpers

        /// <summary>
        /// JSON.NET Deserialize JSON string "[{},{}] to list of objects T"
        /// </summary>
        /// <typeparam name="T">Type of element in List</typeparam>
        /// <param name="jsonString">String which contained JSON</param>
        /// <returns>IList<T></returns>
        public static IList<T> DeserializeToList<T>(string jsonString)
        {
            var array = JArray.Parse(jsonString);
            IList<T> objectsList = new List<T>();
            foreach (var item in array)
            {
                objectsList.Add(item.ToObject<T>());
            }
            return objectsList;
        }

        /// <summary>
        /// Main function for work with JSON.NET
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        public static IList<T> GetObjectListFromJson<T>(HttpWebResponse response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
                throw new WebException(response.StatusDescription);
            using (Stream oStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(oStream);
                return DeserializeToList<T>(reader.ReadToEnd());
            }
        }

        /// <summary>
        /// Deserialize to T object from response with JSON.NET
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
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
    }
}

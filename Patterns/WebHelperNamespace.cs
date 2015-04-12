/**
 * Helper for working with Web. (Name Value pairs, urlencoding and other...)
 * Examples for extends class.
 * */
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WebHelperNamespace
{
    public static class Extender
    {
        public static IDictionary<string, string[]> ToDictionary(
                                    this NameValueCollection source)
        {
            return source.AllKeys.ToDictionary(k => k, k => source.GetValues(k));
        }

        /// <summary>
        /// UrlEncode for NameValueCollection.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="encoding">Encoding</param>
        /// <returns>bytes repesentation.</returns>
        public static byte[] UrlEncode(
                                    this NameValueCollection source, Encoding encoding)
        {
            StringBuilder ResultString = new StringBuilder();
            bool first = true;
            foreach (string key in source.AllKeys)
            {
                if (first)
                    first = false;
                else
                    ResultString.Append("&");
                foreach (var value in source.GetValues(key))
                {
                    ResultString.AppendFormat("{0}={1}", key, value);
                    ResultString.Append("&");
                }
                ResultString.Remove(ResultString.Length - 1, 1);
            }
            return encoding.GetBytes(ResultString.ToString());
        }

        /// <summary>
        /// Encode pair values to byte array.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>bytes repesentation.</returns>
        public static byte[] EncodeValues(this IDictionary<string, string> source, Encoding encoding)
        {
            StringBuilder ResultString = new StringBuilder();
            bool first = true;
            foreach (var pair in source)
            {
                if (first)
                    first = false;
                else
                    ResultString.Append("&");
                ResultString.AppendFormat("{0}={1}", pair.Key, pair.Value);
            }
            // From System.Web
            //HttpUtility.UrlEncode(ResultString.ToString(), encoding);
            return encoding.GetBytes(ResultString.ToString());
        }

        /// <summary>
        /// Encode pair values to byte array.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>bytes repesentation.</returns>
        public static byte[] EncodeValues(this IDictionary<string, string[]> source, Encoding encoding)
        {
            StringBuilder ResultString = new StringBuilder();
            bool first = true;
            foreach (var pair in source)
            {
                if (first)
                    first = false;
                else
                    ResultString.Append("&");
                foreach (var value in pair.Value)
                {
                    ResultString.AppendFormat("{0}={1}", pair.Key, value);
                    ResultString.Append("&");
                }
                ResultString.Remove(ResultString.Length-1, 1);
            }
            //HttpUtility.UrlEncode(ResultString.ToString(), encoding);
            return encoding.GetBytes(ResultString.ToString());
        }

    }
    public class WebHelper {
        private string _referer;
        private CookieContainer _cookies = new CookieContainer();
     
        /// <summary>
        /// Bug fix for Cookie processing in .NET Framework.
        /// </summary>
        /// <param name="cookieContainer"></param>
        private void _BugFix_CookieDomain(CookieContainer cookieContainer)
        {
            System.Collections.Hashtable table = (System.Collections.Hashtable)cookieContainer.GetType().InvokeMember("m_domainTable",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.GetField |
                System.Reflection.BindingFlags.Instance,
                null,
                cookieContainer,
                new object[] { }
            );
            System.Collections.ArrayList keys = new System.Collections.ArrayList(table.Keys);
            foreach (string keyObj in keys)
            {
                string key = (keyObj as string);
                if (key[0] == '.')
                {
                    string newKey = key.Remove(0, 1);
                    table[newKey] = table[keyObj];
                }
            }
        }
        /// <summary>
        /// Do Abstract Request.
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="requestMethod">GET or POST or smth</param>
        /// <param name="content">byte array content</param>
        /// <returns>Response object</returns>
        protected async Task<HttpWebResponse> _RequestAsync(String url, String requestMethod, byte[] content)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.ProtocolVersion = HttpVersion.Version11;
            request.KeepAlive = true;
            request.Method = requestMethod;
            if (!String.IsNullOrEmpty(this._referer))
                request.Referer = this._referer;

            request.AllowAutoRedirect = false;
            request.UserAgent = "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.56 Safari/535.11";
            request.Headers.Add(HttpRequestHeader.AcceptCharset, "windows-1251,utf-8;q=0.7,*;q=0.3");
            request.CookieContainer = this._cookies;
            if (content != null)
            {
                request.ContentType = "application/x-www-form-urlencoded; charset=windows-1251";
                request.ContentLength = content.LongLength;
                using (Stream newStream = await request.GetRequestStreamAsync())
                {
                    newStream.Write(content, 0, content.Length);
                }
            }
            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            request = null;
            this._BugFix_CookieDomain(this._cookies);
            this._referer = url;
            return response;
        }
    }

    class Program
    {   
        static void Main(string[] args)
        {
            NameValueCollection Data = new NameValueCollection();
            Data.Add("Foo", "bar");
            Data.Add("Foo", "Bar");
            var Dict = Data.ToDictionary();
            Encoding encoding = Encoding.GetEncoding("Windows-1251");
            var urlData = Data.UrlEncode(encoding);
            byte[] ByteData = Dict.EncodeValues(encoding);
            //
            var uS = System.Web.HttpUtility.UrlEncode("My string.", Encoding.GetEncoding(1251));
        }
    }
}

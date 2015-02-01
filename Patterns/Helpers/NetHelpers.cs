using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace NetHelpers
{
    /// <summary>
    /// JSON.NET helpers
    /// </summary>
    public static class NetHelpers
    {
        #region NameValueCollection
        public static string NameValueCollectionToParamString(System.Collections.Specialized.NameValueCollection nvc)
        {
            return string.Join("&", nvc.AllKeys.Where(_key =>
                !string.IsNullOrWhiteSpace(nvc[_key]))
                .Select(_key =>
                    string.Format("{0}={1}", _key, nvc[_key])));
        }

        public static string NameValueCollectionToParamString(this System.Collections.Specialized.NameValueCollection nvc)
        {
            return string.Join("&", nvc.AllKeys.Where(_key =>
                !string.IsNullOrWhiteSpace(nvc[_key]))
                .Select(_key =>
                    string.Format("{0}={1}", _key, nvc[_key])));
        }

        public static string NameValueCollectionToUrlString(System.Collections.Specialized.NameValueCollection nvc, string url)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(url).Append("?");
            var s = string.Join("&", nvc.AllKeys.Where(_key =>
                !string.IsNullOrWhiteSpace(nvc[_key]))
                .Select(_key =>
                    string.Format("{0}={1}", _key, nvc[_key])));
            sb.Append(s);
            return sb.ToString();
        }
        #endregion
    }
}

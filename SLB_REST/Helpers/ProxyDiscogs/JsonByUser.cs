using SLB_REST.Helpers.SingletonDiscogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SLB_REST.Helpers.Proxy
{
    public class JsonByUser : IDiscogsClient, IDiscogsClientMethod
    {
        public SingletonDiscogsUser DiscogsUser { get; }
        public string WebResource { get; private set; }
        public string Query { get; set; }
        public string Key { get; set; }
        public string Secret { get; private set; }

        public JsonByUser()
        {
            DiscogsUser = SingletonDiscogsUser.Instance();
            Key = DiscogsUser.Key;
            Secret = DiscogsUser.Secret;
            WebResource = DiscogsUser.WebResource;
        }

        public string GetJson()
        {
            string result = null;
            string[] queryUser = Query.Split(",");
            try
            {
                if (string.IsNullOrEmpty(queryUser[0])) throw new Exception("No Query, use SetQuery()");
                using (WebClient client = DiscogsUser.Client)
                {
                    result = client.DownloadString(WebResource + QueryToLink(queryUser));
                }

                return result;

            }
            catch (WebException)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IDiscogsClientMethod LinkQuery(string query)
        {
            Query = query;
            return this;
        }

        private string QueryToLink(string[] queryUser)
        {
            string get = "";
            if (queryUser.Length == 1)
                get = "/database/search?q={" + queryUser[0].Trim() + "}&per_page=" + DiscogsUser.PerPage;
            else
            {
                get = "/database/search?q={" + queryUser[0].Trim() + "}&{?";

                for (int i = 1; i < Query.Length; i++)
                {
                    get += queryUser[i].Trim();
                    if (i < queryUser.Length - 1) get += ",";
                    else get += "}&per_page=" + DiscogsUser.PerPage;
                }
            }
            return get;
        }

    }
}

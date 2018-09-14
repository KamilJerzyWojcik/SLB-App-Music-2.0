using SLB_REST.Helpers.SingletonDiscogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SLB_REST.Helpers.Proxy
{
    public class JsonByLink : IDiscogsClient, IDiscogsClientMethod
    {
        public SingletonDiscogsUser DiscogsUser { get; }
        public string WebResource { get; private set; }
        public string Query { get; set; }
        public string Key { get; set; }
        public string Secret { get; private set; }

        public JsonByLink()
        {
            DiscogsUser = SingletonDiscogsUser.Instance();
            Key = DiscogsUser.Key;
            Secret = DiscogsUser.Secret;
            WebResource = DiscogsUser.WebResource;
        }

        public string GetJson()
        {
            string result;
            try
            {
                if (string.IsNullOrEmpty(Query)) throw new Exception("No Link, use SetLink()");

                using (WebClient client = DiscogsUser.Client)
                {
                    result = client.DownloadString(Query);
                }
                return result;
            }
            catch (WebException)
            {
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        public IDiscogsClientMethod LinkQuery(string query)
        {
            Query = query;
            return this;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLB_REST.Helpers.Proxy
{
    public class ProxyDiscogs : IDiscogsClientMethod
    {
        private IDiscogsClientMethod _context;

        public ProxyDiscogs Context(IDiscogsClientMethod context)
        {
            _context = context;
            return this;
        }

        public string GetJson()
        {
            return _context.GetJson();
        }

        public IDiscogsClientMethod LinkQuery(string query)
        {
            return _context.LinkQuery(query);
        }
    }
}

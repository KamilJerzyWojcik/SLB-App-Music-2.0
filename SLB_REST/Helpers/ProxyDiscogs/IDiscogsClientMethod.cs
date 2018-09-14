using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLB_REST.Helpers.Proxy
{
    public interface IDiscogsClientMethod
    {
        IDiscogsClientMethod LinkQuery(string query);
        string GetJson();
    }
}

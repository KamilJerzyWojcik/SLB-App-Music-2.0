using SLB_REST.Helpers.SingletonDiscogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLB_REST.Helpers.Proxy
{
    public interface IDiscogsClient
    {
        string WebResource { get; }
        string Query { get; }
        string Key { get; }
        string Secret { get; }
        SingletonDiscogsUser DiscogsUser {get;} 


    }
}

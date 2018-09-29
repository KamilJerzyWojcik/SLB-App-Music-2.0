using Newtonsoft.Json.Linq;
using SLB_REST.Context;
using SLB_REST.Helpers.GetDataBaseStrategy.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLB_REST.Helpers.GetDataBaseStrategy
{
    public class GetDataBaseStrategy : IGetData
    {
        public JObject JsonFile { get; private set; }
        private IGetData _context;

        public IGetData Context(IGetData context)
        {
            _context = context;
            return this;
        }

        public IGetData Data(JObject json)
        {
            JsonFile = json;
            return this;
        }

        public dynamic Get(EFContext EFcontext)
        {
            dynamic results = _context.Data(JsonFile).Get(EFcontext);
            return results;
        }
    }
}

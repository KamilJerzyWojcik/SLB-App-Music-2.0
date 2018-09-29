using Newtonsoft.Json.Linq;
using SLB_REST.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLB_REST.Helpers.GetDataBaseStrategy.Interfaces
{
    public interface IGetData
    {
        JObject JsonFile { get; }
        IGetData Data(JObject json);
        dynamic Get(EFContext EFcontext);
    }
}

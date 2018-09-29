using Newtonsoft.Json.Linq;
using SLB_REST.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLB_REST.Helpers.GetDataBaseStrategy.Interfaces
{
    public interface IChainGet
    {
        IChainGet Successor { get; }
        void SetSuccessor(IChainGet successor);
        void GetDB(EFContext context, JObject jsonFile, dynamic album);
    }
}

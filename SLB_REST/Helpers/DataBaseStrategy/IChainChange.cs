using Newtonsoft.Json.Linq;
using SLB_REST.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLB_REST.Helpers.DataBaseStrategy.DBChainResp
{
	public interface IChainChange
	{
        IChainChange Successor { get; }
        void SetSuccessor(IChainChange successor);
        void ChangeDB(EFContext context, JObject jsonFile, List<int> ids);
	}
}

using Newtonsoft.Json.Linq;
using SLB_REST.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLB_REST.Helpers.DataBaseStrategy.DBChainResp
{
	public interface IChainAdd
	{
		IChainAdd Successor { get; }
		void SetSuccessor(IChainAdd successor);
		void SaveToDB(EFContext context, JObject jsonFile, List<int> ids);
	}
}

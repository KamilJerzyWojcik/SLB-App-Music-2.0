using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SLB_REST.Context;
using SLB_REST.Models;

namespace SLB_REST.Helpers.DataBaseStrategy.DBChainResp
{
	public class ChainAddStyles : IChainAdd
	{
		public IChainAdd Successor { get; private set; }

		public void SetSuccessor(IChainAdd successor)
		{
			Successor = successor;
		}

		public void SaveToDB(EFContext context, JObject jsonFile, List<int> ids)
		{
			if (ids.Count != 2)
			{
				if (Successor != null)
					Successor.SaveToDB(context, jsonFile, ids);
			}
			else
			{
				try
				{
					foreach (var s in jsonFile["styles"])
					{
						var style = new StyleModel()
						{
							Style = s.ToString(),
							Album = new AlbumModel()
						};
						style.Album.ID = ids[1];

						context.Styles.Add(style);
						context.SaveChanges();
					}
					if (Successor != null)
						Successor.SaveToDB(context, jsonFile, ids);
				}
				catch (Exception)
				{
					var style = new StyleModel()
					{
						Style = "no data",
						Album = new AlbumModel()
					};
					style.Album.ID = ids[1];

					context.Styles.Add(style);
					context.SaveChanges();

					if (Successor != null)
						Successor.SaveToDB(context, jsonFile, ids);
				}
			}
		}
	}
}

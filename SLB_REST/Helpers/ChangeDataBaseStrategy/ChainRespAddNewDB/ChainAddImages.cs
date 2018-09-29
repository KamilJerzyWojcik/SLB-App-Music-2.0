using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SLB_REST.Context;
using SLB_REST.Models;

namespace SLB_REST.Helpers.DataBaseStrategy.DBChainResp
{
	public class ChainAddImages : IChainChange
	{
		public IChainChange Successor { get; private set; }

		public void SetSuccessor(IChainChange successor)
		{
			Successor = successor;
		}

		public void ChangeDB(EFContext context, JObject jsonFile, List<int> ids)
		{
			if (ids.Count != 2)
			{
				if (Successor != null)
					Successor.ChangeDB(context, jsonFile, ids);
			}
			else
			{
				try
				{
					foreach (var i in jsonFile["images"])
					{
						var image = new ImageModel()
						{
							Uri = i["uri"].ToString(),
							Album = new AlbumModel()
						};
						image.Album.ID = ids[1];

						context.Images.Add(image);
						context.SaveChanges();
					}
					if (Successor != null)
						Successor.ChangeDB(context, jsonFile, ids);
				}
				catch (Exception)
				{
					var image = new ImageModel()
					{
						Uri = "no data",
						Album = new AlbumModel()
					};
					image.Album.ID = ids[1];

					context.Images.Add(image);
					context.SaveChanges();
					if (Successor != null)
						Successor.ChangeDB(context, jsonFile, ids);
				}
			}
		}
	}
}

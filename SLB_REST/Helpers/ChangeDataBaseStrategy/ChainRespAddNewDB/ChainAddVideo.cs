using Newtonsoft.Json.Linq;
using SLB_REST.Context;
using SLB_REST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLB_REST.Helpers.DataBaseStrategy.DBChainResp
{
	public class ChainAddVideo : IChainChange
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
					foreach (var v in jsonFile["videos"])
					{
						var video = new VideoModel()
						{
							Description = v["description"].ToString(),
							Uri = v["uri"].ToString(),
							Album = new AlbumModel()
						};
						video.Album.ID = ids[1];

						context.Videos.Add(video);
						context.SaveChanges();
					}
					if (Successor != null)
						Successor.ChangeDB(context, jsonFile, ids);
				}
				catch (Exception)
				{
					var video = new VideoModel()
					{
						Description = "no data",
						Uri = "no data",
						Album = new AlbumModel()
					};
					video.Album.ID = ids[1];

					context.Videos.Add(video);
					context.SaveChanges();

					if (Successor != null)
						Successor.ChangeDB(context, jsonFile, ids);
				}
			}
		}
	}
}

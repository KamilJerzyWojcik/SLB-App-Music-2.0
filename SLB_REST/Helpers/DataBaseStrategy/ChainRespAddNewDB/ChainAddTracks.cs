using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SLB_REST.Context;
using SLB_REST.Models;

namespace SLB_REST.Helpers.DataBaseStrategy.DBChainResp
{
	public class ChainAddTracks : IChainAdd
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
					foreach (var t in jsonFile["tracklist"])
					{
						var track = new TrackModel()
						{
							Title = t["title"].ToString(),
							Duration = t["duration"].ToString(),
							Position = t["position"].ToString(),
							Album = new AlbumModel()
						};

						track.Album.ID = ids[1];
						context.Tracks.Add(track);
						context.SaveChanges();
					}
					if (Successor != null)
						Successor.SaveToDB(context, jsonFile, ids);
				}
				catch (Exception)
				{
					var track = new TrackModel()
					{
						Title = "no data",
						Duration = "no data",
						Position = "no data",
						Album = new AlbumModel()
					};

					track.Album.ID = ids[1];
					context.Tracks.Add(track);
					context.SaveChanges();

					if (Successor != null)
						Successor.SaveToDB(context, jsonFile, ids);
				}
			}
		}
	}
}

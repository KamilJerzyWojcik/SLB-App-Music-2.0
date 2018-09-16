using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using SLB_REST.Context;
using SLB_REST.Models;

namespace SLB_REST.Helpers.DataBaseStrategy.DBChainResp
{
	public class ChainAddExtraartists : IChainChange
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
				List<int> tracksId = context
			.Tracks
			.Include(t => t.Album)
			.Where(t => t.Album.ID == ids[1])
			.Select(t => t.ID)
			.ToList();
				try
				{
					for (int i = 0; i < tracksId.Count; i++)
					{
						if (jsonFile["tracklist"][i]["extraartists"] is null) continue;

						foreach (var xa in jsonFile["tracklist"][i]["extraartists"])
						{
							var xartist = new ExtraArtistModel()
							{
								Track = new TrackModel(),
								Name = xa["name"].ToString()
							};

							xartist.Track.ID = tracksId[i];
							context.ExtraArtists.Add(xartist);
							context.SaveChanges();
						}
					}
					if (Successor != null)
						Successor.ChangeDB(context, jsonFile, ids);
				}
				catch (Exception)
				{
					if (Successor != null)
						Successor.ChangeDB(context, jsonFile, ids);
				}
			}
		}


	}
}

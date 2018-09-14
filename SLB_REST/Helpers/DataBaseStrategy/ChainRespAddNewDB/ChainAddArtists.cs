using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SLB_REST.Context;
using SLB_REST.Models;

namespace SLB_REST.Helpers.DataBaseStrategy.DBChainResp
{
	public class ChainAddArtists : IChainAdd
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
					foreach (var a in jsonFile["artists"])
					{
						var artist = new ArtistModel()
						{
							Name = a["name"].ToString(),
							Album = new AlbumModel()
						};
						artist.Album.ID = ids[1];

						context.Artists.Add(artist);
						context.SaveChanges();
						if (Successor != null)
							Successor.SaveToDB(context, jsonFile, ids);
					}
				}
				catch (Exception)
				{
					var artist = new ArtistModel()
					{
						Name = "no data",
						Album = new AlbumModel()
					};
					artist.Album.ID = ids[1];

					context.Artists.Add(artist);
					context.SaveChanges();
					if (Successor != null)
						Successor.SaveToDB(context, jsonFile, ids);
				}
			}
		}

	}
}

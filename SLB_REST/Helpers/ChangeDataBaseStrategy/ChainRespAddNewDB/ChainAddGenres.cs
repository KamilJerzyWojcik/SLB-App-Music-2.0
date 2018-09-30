using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SLB_REST.Context;
using SLB_REST.Models;

namespace SLB_REST.Helpers.DataBaseStrategy.DBChainResp
{
	public class ChainAddGenres : IChainChange
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
					foreach (var g in jsonFile["genres"])
					{
						var genre = new GenreModel()
						{
							Genre = g.ToString(),
							Album = new AlbumModel()
						};
						genre.Album.ID = ids[1];

						context.Genres.Add(genre);
						context.SaveChanges();
						
					}

                    if (Successor != null)
                        Successor.ChangeDB(context, jsonFile, ids);
                }
				catch (Exception)
				{
					var genre = new GenreModel()
					{
						Genre = "no data",
						Album = new AlbumModel()
					};
					genre.Album.ID = ids[1];

					context.Genres.Add(genre);
					context.SaveChanges();

					if (Successor != null)
						Successor.ChangeDB(context, jsonFile, ids);

                    return;
				}
			}
		}


	}
}

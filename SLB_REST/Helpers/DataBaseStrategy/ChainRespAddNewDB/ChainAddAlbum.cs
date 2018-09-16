using Newtonsoft.Json.Linq;
using SLB_REST.Context;
using SLB_REST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLB_REST.Helpers.DataBaseStrategy.DBChainResp
{
	public class ChainAddAlbum : IChainChange
	{
		public IChainChange Successor { get; private set; }

		public void SetSuccessor(IChainChange successor)
		{
			Successor = successor;
		}

		public void ChangeDB(EFContext context, JObject jsonFile, List<int> ids)
		{
			if (ids.Count == 0)
			{
				if (Successor != null)
					Successor.ChangeDB(context, jsonFile, ids);
			}
			else
			{
				AlbumModel album = new AlbumModel()
				{
					ID = 0,
					User = new UserModel()
				};
				
				album.User.Id = ids[0];

				if (jsonFile["title"] is null) album.Title = "no data";
				else album.Title = jsonFile["title"].ToString();

				context.Albums.Add(album);
				context.SaveChanges();
				ids.Add(album.ID);

				if (Successor != null)
					Successor.ChangeDB(context, jsonFile, ids);
			}
		}


	}
}

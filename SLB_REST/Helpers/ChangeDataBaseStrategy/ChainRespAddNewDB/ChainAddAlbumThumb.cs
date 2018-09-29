using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SLB_REST.Context;
using SLB_REST.Models;

namespace SLB_REST.Helpers.DataBaseStrategy.DBChainResp
{
	public class ChainAddAlbumThumb : IChainChange
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
				var thumbAlbum = new AlbumThumbModel()
				{
					Album = new AlbumModel(),
					User = new UserModel(),
				};

				thumbAlbum.User.Id = ids[0];
				thumbAlbum.Album.ID = ids[1];

				if (jsonFile["images"][0] is null) thumbAlbum.ImageThumbSrc = "/img/cd.jpg";
				else thumbAlbum.ImageThumbSrc = jsonFile["images"][0]["uri"].ToString();

				if (jsonFile["title"] is null) thumbAlbum.Title = "no data";
				else thumbAlbum.Title = jsonFile["title"].ToString();

				if (jsonFile["artists"] is null) thumbAlbum.ArtistName = "no data";
				else thumbAlbum.ArtistName = jsonFile["artists"][0]["name"].ToString();

				if (jsonFile["styles"] is null) thumbAlbum.Style = "no data";
				else thumbAlbum.Style = jsonFile["styles"][0].ToString();

				if (jsonFile["genres"] is null) thumbAlbum.Genres = "no data";
				else thumbAlbum.Genres = jsonFile["genres"][0].ToString();

				context.AlbumsThumb.Add(thumbAlbum);
				context.SaveChanges();

				if (Successor != null)
					Successor.ChangeDB(context, jsonFile, ids);
			}
		}


	}
}

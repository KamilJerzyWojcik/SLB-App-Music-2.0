using Newtonsoft.Json.Linq;
using SLB_REST.Context;
using SLB_REST.Helpers.DataBaseStrategy.DBChainResp;
using SLB_REST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLB_REST.Helpers.DataBaseStrategy
{
	public class AddNewDB : ISaveChanges
	{
		public JObject JsonFile { get; private set; }
		public int UserId { get; private set; }
		private int _albumId;

		public ISaveChanges Load(JObject json, int userId)
		{
			JsonFile = json;
			UserId = userId;
			return this;
		}

		public void SaveChanges(EFContext _context )
		{
			List<int> ids = new List<int>(){ UserId };

			IChainAdd addAlbumThumb = new ChainAddAlbumThumb();

			IChainAdd addExtraartists = new ChainAddExtraartists();
			addExtraartists.SetSuccessor(addAlbumThumb);

			IChainAdd addTracks = new ChainAddTracks();
			addTracks.SetSuccessor(addExtraartists);

			IChainAdd addStyles = new ChainAddStyles();
			addStyles.SetSuccessor(addTracks);

			IChainAdd addGenres = new ChainAddGenres();
			addGenres.SetSuccessor(addStyles);

			IChainAdd addArtists = new ChainAddArtists();
			addArtists.SetSuccessor(addGenres);

			IChainAdd addImages = new ChainAddImages();
			addImages.SetSuccessor(addArtists);

			IChainAdd addVideos = new ChainAddVideo();
			addVideos.SetSuccessor(addImages);

			IChainAdd addAlbum = new ChainAddAlbum();
			addAlbum.SetSuccessor(addVideos);

			addAlbum.SaveToDB(_context, JsonFile, ids);
		}

	}
}


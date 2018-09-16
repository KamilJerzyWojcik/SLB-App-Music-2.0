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

            IChainChange addAlbumThumb = new ChainAddAlbumThumb();

            IChainChange addExtraartists = new ChainAddExtraartists();
			addExtraartists.SetSuccessor(addAlbumThumb);

            IChainChange addTracks = new ChainAddTracks();
			addTracks.SetSuccessor(addExtraartists);

            IChainChange addStyles = new ChainAddStyles();
			addStyles.SetSuccessor(addTracks);

            IChainChange addGenres = new ChainAddGenres();
			addGenres.SetSuccessor(addStyles);

            IChainChange addArtists = new ChainAddArtists();
			addArtists.SetSuccessor(addGenres);

            IChainChange addImages = new ChainAddImages();
			addImages.SetSuccessor(addArtists);

            IChainChange addVideos = new ChainAddVideo();
			addVideos.SetSuccessor(addImages);

            IChainChange addAlbum = new ChainAddAlbum();
			addAlbum.SetSuccessor(addVideos);

			addAlbum.ChangeDB(_context, JsonFile, ids);
		}

	}
}


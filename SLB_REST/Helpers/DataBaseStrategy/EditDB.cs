using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SLB_REST.Context;
using SLB_REST.Helpers.DataBaseStrategy.ChainRespEditDB;
using SLB_REST.Helpers.DataBaseStrategy.DBChainResp;

namespace SLB_REST.Helpers.DataBaseStrategy
{
    public class EditDB : ISaveChanges
    {
        public JObject JsonFile { get; private set; }
        public int UserId { get; private set; }

        public ISaveChanges Load(JObject json, int userId)
        {
            JsonFile = json;
            UserId = userId;
            return this;
        }

        public void SaveChanges(EFContext _context)
        {
            List<int> ids = new List<int>() { int.Parse(JsonFile["id"].ToString()) };

            if (!(JsonFile["idOnList"] is null))
                ids.Add(int.Parse(JsonFile["idOnList"].ToString()));

            IChainChange editAlbumThumb = new ChainEditAlbumThumb();

            IChainChange editStyles = new ChainEditStyles();
            editStyles.SetSuccessor(editAlbumThumb);

            IChainChange editGenres = new ChainEditGenres();
            editGenres.SetSuccessor(editStyles);

            IChainChange editArtists = new ChainEditArtists();
            editArtists.SetSuccessor(editGenres);

            IChainChange editImages = new ChainEditImages();
            editImages.SetSuccessor(editArtists);

            IChainChange editVideos = new ChainEditVideos();
            editVideos.SetSuccessor(editImages);


            editVideos.ChangeDB(_context, JsonFile, ids);
        }
    }
}

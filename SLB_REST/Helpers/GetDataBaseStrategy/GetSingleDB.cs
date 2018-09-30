using Newtonsoft.Json.Linq;
using SLB_REST.Context;
using SLB_REST.Helpers.GetDataBaseStrategy.ChainRespGetDB;
using SLB_REST.Helpers.GetDataBaseStrategy.Interfaces;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace SLB_REST.Helpers.DataBaseStrategy
{
    public class GetSingleDB : IGetData
    {
        public JObject JsonFile { get; private set; }

        public IGetData Data(JObject json)
        {
            JsonFile = json;
            return this;
        }

        public dynamic Get(EFContext EFcontext)
        {
            IChainGet getThumbAlbum = new ChainGetThumbAlbum();

            IChainGet getExtraartists = new ChainGetExtraartists();
            getExtraartists.SetSuccessor(getThumbAlbum);
            IChainGet getTracks = new ChainGetTracks();
            getTracks.SetSuccessor(getExtraartists);
            IChainGet getVideos = new ChainGetVideos();
            getVideos.SetSuccessor(getTracks);
            IChainGet getImages = new ChainGetImages();
            getImages.SetSuccessor(getVideos);
            IChainGet getArtists = new ChainGetArtists();
            getArtists.SetSuccessor(getImages);
            IChainGet getStyles = new ChainGetStyles();
            getStyles.SetSuccessor(getArtists);
            IChainGet getGenres = new ChainGetGenres();
            getGenres.SetSuccessor(getStyles);
            IChainGet getTitle = new ChainGetTitle();
            getTitle.SetSuccessor(getGenres);

            dynamic album = new ExpandoObject();
            getTitle.GetDB(EFcontext, JsonFile, album);

            return album;
        }
    }
}

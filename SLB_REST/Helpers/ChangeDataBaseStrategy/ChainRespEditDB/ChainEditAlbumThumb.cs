using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using SLB_REST.Context;
using SLB_REST.Helpers.DataBaseStrategy.DBChainResp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLB_REST.Helpers.DataBaseStrategy.ChainRespEditDB
{
    public class ChainEditAlbumThumb : IChainChange
    {
        public IChainChange Successor { get; private set; }

        public void SetSuccessor(IChainChange successor)
        {
            Successor = successor;
        }

        public void ChangeDB(EFContext context, JObject jsonFile, List<int> ids)
        {
            if (jsonFile["thumbalbums"] is null)
            {
                if (Successor != null)
                    Successor.ChangeDB(context, jsonFile, ids);
            }
            else
            {
                try
                {
                    var album = context.Albums
                        .Include(a => a.AlbumThumb)
                        .Where(a => a.ID == ids[0])
                        .SingleOrDefault();

                    album.AlbumThumb.ImageThumbSrc = jsonFile["thumbalbums"][0]["srcImage"].ToString();
                    album.AlbumThumb.ArtistName = jsonFile["thumbalbums"][0]["artistName"].ToString();
                    album.AlbumThumb.Genres = jsonFile["thumbalbums"][0]["genre"].ToString();
                    album.AlbumThumb.Style = jsonFile["thumbalbums"][0]["style"].ToString();

                    context.Albums.Update(album);
                    context.SaveChanges();
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

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using SLB_REST.Context;
using SLB_REST.Helpers.GetDataBaseStrategy.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLB_REST.Helpers.GetDataBaseStrategy.ChainRespGetDB
{
    public class ChainGetThumbAlbum : IChainGet
    {
        public IChainGet Successor { get; private set; }

        public void SetSuccessor(IChainGet successor)
        {
            Successor = successor;
        }

        public void GetDB(EFContext context, JObject jsonFile, dynamic album)
        {
            if (jsonFile["thumbAlbum"] is null)
            {
                if (Successor != null)
                    Successor.GetDB(context, jsonFile, album);
            }
            else
            {
                try
                {
                    int id = int.Parse(jsonFile["id"].ToString());

                    if (id == -1)
                        GetAlbums(context, album, jsonFile);
                    else
                        GetAlbumByID(context, album, jsonFile);

                    if (Successor != null)
                        Successor.GetDB(context, jsonFile, album);
                }
                catch (Exception)
                {
                    if (Successor != null)
                        Successor.GetDB(context, jsonFile, album);
                }
            }
        }

        private dynamic GetAlbumByID(EFContext context, dynamic album, JObject jsonFile)
        {
            int id = int.Parse(jsonFile["id"].ToString());

            album.albumThumb = context
                   .AlbumsThumb
                   .Include(a => a.Album)
                   .Where(a => a.Album.ID == id)
                   .Select(a => new
                   {
                       a.ImageThumbSrc,
                       a.Title,
                       a.Style,
                       a.Genres,
                       a.ArtistName
                   })
                   .SingleOrDefault();

            return album;
        }

        private dynamic GetAlbums(EFContext context, dynamic albums, JObject jsonFile)
        {
            int page = int.Parse(jsonFile["page"].ToString());
            string user = jsonFile["user"].ToString();

            albums.thumbAlbums = context
                    .AlbumsThumb
                    .Include(t => t.User)
                    .Where(t => t.User.UserName == user)
                    .Include(t => t.Album)
                    .Skip(page * 6)
                    .Take(6)
                    .Select(a => new
                    {
                        a.ImageThumbSrc,
                        a.Title,
                        a.Style,
                        a.Genres,
                        a.ArtistName,
                        a.Album
                    })
                    .ToList();
            
            return albums;
        }
    }
}
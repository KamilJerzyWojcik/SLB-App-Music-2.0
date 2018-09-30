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
    public class ChainGetArtists : IChainGet
    {
        public IChainGet Successor { get; private set; }

        public void SetSuccessor(IChainGet successor)
        {
            Successor = successor;
        }

        public void GetDB(EFContext context, JObject jsonFile, dynamic album)
        {
            if (jsonFile["artists"] is null)
            {
                if (Successor != null)
                    Successor.GetDB(context, jsonFile, album);
            }
            else
            {
                try
                {
                    int id = (int)jsonFile["id"];
                    album.artists = context
                        .Artists
                        .Include(g => g.Album)
                        .Where(g => g.Album.ID == id)
                        .Select(g => new
                        {
                            g.Name
                        })
                        .ToList();

                    if (Successor != null)
                        Successor.GetDB(context, jsonFile, album);
                }
                catch(Exception)
                {
                    if (Successor != null)
                        Successor.GetDB(context, jsonFile, album);
                }
            }
        }

        
    }
}

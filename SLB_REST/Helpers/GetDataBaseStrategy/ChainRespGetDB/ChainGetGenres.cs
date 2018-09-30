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
    public class ChainGetGenres : IChainGet
    {
        public IChainGet Successor { get; private set; }

        public void SetSuccessor(IChainGet successor)
        {
            Successor = successor;
        }

        public void GetDB(EFContext context, JObject jsonFile, dynamic album)
        {

            if (jsonFile["genres"] is null)
            {
                if (Successor != null)
                    Successor.GetDB(context, jsonFile, album);
            }
            else
            {
                try
                {
                    int id = (int)jsonFile["id"];
                    album.genres = context
                        .Genres
                        .Include(g => g.Album)
                        .Where(g => g.Album.ID == id)
                        .Select(g => new { g.Genre })
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

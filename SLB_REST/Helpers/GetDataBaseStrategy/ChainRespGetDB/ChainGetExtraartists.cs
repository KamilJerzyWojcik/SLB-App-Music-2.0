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
    public class ChainGetExtraartists : IChainGet
    {
        public IChainGet Successor { get; private set; }

        public void SetSuccessor(IChainGet successor)
        {
            Successor = successor;
        }

        public void GetDB(EFContext context, JObject jsonFile, dynamic album)
        {
            if (jsonFile["extraartists"] is null)
            {
                if (Successor != null)
                    Successor.GetDB(context, jsonFile, album);
            }
            else
            {
                try
                {
                    int id = (int)jsonFile["id"];
                    album.extraArtists = new List<dynamic>();
                    foreach (var track in album.tracks)
                    {
                        int i = track.ID;

                        album.extraArtists.Add(context
                        .ExtraArtists
                        .Include(v => v.Track)
                        .Where(v => v.Track.ID == i)
                        .Select(t => new { t.Name })
                        .ToList());
                    }

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
        
    }
}

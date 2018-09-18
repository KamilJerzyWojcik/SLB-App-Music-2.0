using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using SLB_REST.Context;
using SLB_REST.Helpers.DataBaseStrategy.DBChainResp;
using SLB_REST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLB_REST.Helpers.DataBaseStrategy.ChainRespEditDB
{
    public class ChainEditVideos : IChainChange
    {
        public IChainChange Successor { get; private set; }

        public void SetSuccessor(IChainChange successor)
        {
            Successor = successor;
        }

        public void ChangeDB(EFContext context, JObject jsonFile, List<int> ids)
        {
            if (jsonFile["videos"] is null)
            {
                if (Successor != null)
                    Successor.ChangeDB(context, jsonFile, ids);
            }
            else
            {
                try
                {
                    var album = context.Albums
                        .Where(a => a.ID == ids[0])
                        .Include(a => a.Videos)
                        .SingleOrDefault();

                    if (ids.Count == 1)
                    {
                        album.Videos.Add(new VideoModel()
                        {
                            Description = jsonFile["videos"][0]["description"].ToString(),
                            Uri = jsonFile["videos"][0]["source"].ToString()
                        });
                    }

                    if (ids.Count == 2)
                    {
                        int i = 0;
                        foreach (var vid in album.Videos)
                        {
                            if (i == ids[1])
                            {
                                vid.Description = jsonFile["videos"][0]["description"].ToString();
                                vid.Uri = jsonFile["videos"][0]["source"].ToString();
                            }
                            i++;
                        }
                    }

                    context.Albums.Update(album);
                    context.SaveChanges();

                    if (Successor != null)
                        Successor.ChangeDB(context, jsonFile, ids);
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

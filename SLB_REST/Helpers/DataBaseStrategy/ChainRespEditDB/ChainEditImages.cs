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
    public class ChainEditImages : IChainChange
    {
        public IChainChange Successor { get; private set; }

        public void SetSuccessor(IChainChange successor)
        {
            Successor = successor;
        }

        public void ChangeDB(EFContext context, JObject jsonFile, List<int> ids)
        {
            if (jsonFile["images"] is null)
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
                        .Include(a => a.Images)
                        .SingleOrDefault();

                    if (ids.Count == 1)
                        album.Images.Add(new ImageModel() { Uri = jsonFile["images"][0].ToString() });

                    if (ids.Count == 2)
                    {
                        int i = 0;
                        foreach (var img in album.Images)
                        {
                            if (i == ids[1]) img.Uri = jsonFile["images"][0].ToString();
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

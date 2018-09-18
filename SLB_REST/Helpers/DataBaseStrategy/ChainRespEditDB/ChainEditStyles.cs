using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using SLB_REST.Context;
using SLB_REST.Helpers.DataBaseStrategy.DBChainResp;
using SLB_REST.Models;
using SLB_REST.Models.Albums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLB_REST.Helpers.DataBaseStrategy.ChainRespEditDB
{
    public class ChainEditStyles : IChainChange
    {
        public IChainChange Successor { get; private set; }

        public void SetSuccessor(IChainChange successor)
        {
            Successor = successor;
        }

        public void ChangeDB(EFContext context, JObject jsonFile, List<int> ids)
        {
            if (jsonFile["styles"] is null)
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
                        .Include(a => a.Styles)
                        .SingleOrDefault();

                    List<StyleModel> styles = new List<StyleModel>();
                    foreach (var style in jsonFile["styles"])
                        styles.Add(new StyleModel() { Style = style.ToString().Trim() });

                    album.Styles = styles;
                    context.Albums.Update(album);
                    context.SaveChanges();

                    var old = context
                        .Styles
                        .Include(g => g.Album)
                        .Where(g => g.Album.ID == null)
                        .ToList();

                    foreach (var o in old)
                    {
                        context.Styles.Remove(o);
                        context.SaveChanges();
                    }

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

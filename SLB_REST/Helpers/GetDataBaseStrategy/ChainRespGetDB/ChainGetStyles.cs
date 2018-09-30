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
    public class ChainGetStyles : IChainGet
    {
        public IChainGet Successor { get; private set; }

        public void SetSuccessor(IChainGet successor)
        {
            Successor = successor;
        }

        public void GetDB(EFContext context, JObject jsonFile, dynamic album)
        {
            if (jsonFile["styles"] is null)
            {
                if (Successor != null)
                    Successor.GetDB(context, jsonFile, album);
            }
            else
            {
                try
                {
                    int id = (int)jsonFile["id"];
                    album.styles = context
                        .Styles
                        .Include(g => g.Album)
                        .Where(g => g.Album.ID == id)
                        .Select(g => new { g.Style })
                        .ToList();

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

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
            IChainGet getTitle = new ChainGetTitle();
            getTitle.SetSuccessor(null);

            dynamic album = new ExpandoObject();
            getTitle.GetDB(EFcontext, JsonFile, album);
            //łancuch zaleznosci
            //wejscie na dynamic
            return album;
        }
    }
}

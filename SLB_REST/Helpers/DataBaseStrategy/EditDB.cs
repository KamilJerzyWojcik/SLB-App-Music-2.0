using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SLB_REST.Context;

namespace SLB_REST.Helpers.DataBaseStrategy
{
    public class EditDB : ISaveChanges
    {
        public JObject JsonFile { get; private set; }
        public int UserId { get; private set; }

        public ISaveChanges Load(JObject json, int userId)
        {
            JsonFile = json;
            UserId = userId;
            return this;
        }

        public void SaveChanges(EFContext EFcontext)
        {
            throw new NotImplementedException();
        }
    }
}

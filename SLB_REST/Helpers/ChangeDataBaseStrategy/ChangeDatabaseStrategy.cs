using Newtonsoft.Json.Linq;
using SLB_REST.Context;
using SLB_REST.Helpers.DataBaseStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLB_REST.Helpers.ChangeDatabaseStrategy
{
    public class ChangeDatabaseStrategy : ISaveChanges
    {
        public JObject JsonFile { get; private set; }
        public int UserId { get; private set; }
        private ISaveChanges _context;

        public ISaveChanges Context(ISaveChanges context)
        {
            _context = context;
            return this;
        }

        public ISaveChanges Data(JObject json, int userId)
        {
            JsonFile = json;
            UserId = userId;
            return this;
        }

        public void Save(EFContext EFcontext)
        {
            _context.Data(JsonFile, UserId).Save(EFcontext);
        }
    }
}

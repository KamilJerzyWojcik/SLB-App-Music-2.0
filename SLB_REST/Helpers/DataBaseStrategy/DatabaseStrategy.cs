using Newtonsoft.Json.Linq;
using SLB_REST.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLB_REST.Helpers.DataBaseStrategy
{
	public class DatabaseStrategy : ISaveChanges
	{
		public JObject JsonFile { get; private set; }
		public int UserId { get; private set; }
		private ISaveChanges _context;

		public ISaveChanges Context(ISaveChanges context)
		{
			_context = context;
			return this;
		}

		public ISaveChanges Load(JObject json, int userId)
		{
			JsonFile = json;
			UserId = userId;
			return this;
		}

		public void SaveChanges(EFContext EFcontext)
		{
			_context.Load(JsonFile, UserId).SaveChanges(EFcontext);
		}
	}
}

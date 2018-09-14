﻿using Newtonsoft.Json.Linq;
using SLB_REST.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLB_REST.Helpers.DataBaseStrategy
{
	public interface ISaveChanges
	{
		JObject JsonFile { get; }
		int UserId{ get; }
		ISaveChanges Load(JObject json, int userId);
		void SaveChanges(EFContext EFcontext);
	}
}

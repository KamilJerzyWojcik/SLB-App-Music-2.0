using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SLB_REST.Helpers.SingletonDiscogs
{
	public class SingletonDiscogsUser
	{
		private static SingletonDiscogsUser _singletonDiscogs;
		private static readonly object _lock = new object();

		public WebClient Client
		{
			get
			{
				var client = new WebClient();
				client.Headers.Add("User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
				client.Headers.Add("Content-Type", "application/json");
				client.Headers.Add($"Authorization: Discogs key={Key}, secret={Secret}");
				client.UseDefaultCredentials = true;

				return client;
			}
		}

		public string Key { get; } = "HfsyysppxnrCYpIttoYY";
		public string Secret { get; } = "hIuWlIIOvzhnINezvqJCeRJgeWYYNsNw";
		public string WebResource { get; } = "https://api.discogs.com";
		public int PerPage = 6;

		private SingletonDiscogsUser(){}

		public static SingletonDiscogsUser Instance()
		{
			lock (_lock)
			{
				if (_singletonDiscogs == null)
				{
					_singletonDiscogs = new SingletonDiscogsUser();
				}
				return _singletonDiscogs;
			}
		}
	}
}

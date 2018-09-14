using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SLB_REST.Context;
using SLB_REST.Helpers;
using SLB_REST.Helpers.DataBaseStrategy;
using SLB_REST.Helpers.Proxy;
using SLB_REST.Models;
using Newtonsoft.Json.Linq;

namespace SLB_REST.Controllers
{
    [Authorize]
    public class DiscogsController : Controller
	{
        private readonly EFContext _context;
        private readonly ProxyDiscogs _proxyDiscogs;
		private readonly DatabaseStrategy _databaseStrategy;

		public DiscogsController(
            EFContext context, 
            ProxyDiscogs proxyDiscogs,
			DatabaseStrategy databaseStrategy)
        {
            _context = context;
            _proxyDiscogs = proxyDiscogs;
			_databaseStrategy = databaseStrategy;

		}

        public IActionResult Albums(string queryUser = "")
		{

			if (queryUser == "" || queryUser == null)
			{
				TempData["SearchError"] = "Wrong query, try search again";
				return View();
			}

			ViewBag.query = queryUser;

			return View();
		}

		public IActionResult GetJsonByQuery(string queryUser)
		{
            string json = _proxyDiscogs
			.Context(new JsonByUser())
			.LinkQuery(queryUser)
			.GetJson();

			return Content(json);
		}

		public IActionResult GetJsonByLink(string link)
		{
			string json = _proxyDiscogs
			.Context(new JsonByLink())
			.LinkQuery(link)
			.GetJson();

			return Content(json);
		}

		[HttpPost]
		public IActionResult Add(string link)
		{
			int userId = _context.Users
			.Where(u => u.UserName == User.Identity.Name)
			.Single().Id;

			_databaseStrategy
			.Context(new AddNewDB())
			.Load(JObject.Parse(link), userId)
			.SaveChanges(_context);

			return Ok();
		}
    }
}

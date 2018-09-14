using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SLB_REST.Context;
using SLB_REST.Helpers;
using SLB_REST.Helpers.Proxy;
using SLB_REST.Models;

namespace SLB_REST.Controllers
{
    [Authorize]
    public class DiscogsController : Controller
	{
        private readonly EFContext _context;
        private SourceManagerEF _sourceManagerEF;
        private DiscogsClientModel _discogsClient;
        private SourceManagerSaveJson _sourceManagerSaveJson;
        private readonly ProxyDiscogs _proxyDiscogs;

        public DiscogsController(
            SourceManagerEF sourceManagerEF, 
            EFContext context, 
            DiscogsClientModel discogsClient,
            SourceManagerSaveJson sourceManagerSaveJson,
            ProxyDiscogs proxyDiscogs)
        {
            _context = context;
            _sourceManagerEF = sourceManagerEF;
            _discogsClient = discogsClient;
            _sourceManagerSaveJson = sourceManagerSaveJson;
            _proxyDiscogs = proxyDiscogs;
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
            string json = _proxyDiscogs.Context(new JsonByUser()).LinkQuery(queryUser).GetJson();

   //         string[] query = queryUser.Split(",");
			//string json = _discogsClient.SetQuery(query).SearchJsonByQuery();
			return Content(json);
		}

		public IActionResult GetJsonByLink(string link)
		{
			string json = _discogsClient.SetLink(link).GetJsonByLink();
			return Content(json);
		}

		public IActionResult Album(string resource)
		{
			string json = _discogsClient.SetLink(resource).GetJsonByLink();
			return Content(json);
		}

		[HttpPost]
		public IActionResult Add(string link)
		{

            AlbumModel album = _sourceManagerEF.Load(link).GetAlbum();

            album = addAlbum(album);
            _sourceManagerSaveJson.Load(_context, _sourceManagerEF).addTracks(album);
             _sourceManagerSaveJson.addVideos(album);
            _sourceManagerSaveJson.addStyles(album);
            _sourceManagerSaveJson.addGenres(album);
            _sourceManagerSaveJson.addImages(album);
            _sourceManagerSaveJson.addArtists(album);
            _sourceManagerSaveJson.addAlbumThumb(album);
            _context.SaveChanges();

            return Ok();
        }

        private AlbumModel addAlbum(AlbumModel album)
        {
            UserModel user = _context.Users
            .Where(u => u.UserName == User.Identity.Name).SingleOrDefault();

            album.User = new UserModel();
            album.ID = 0;
            album.User.Id = user.Id;
            _context.Albums.Add(album);
            _context.SaveChanges();

            return album;
        }
    }
}

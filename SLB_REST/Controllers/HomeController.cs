using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SLB_REST.Context;
using SLB_REST.Helpers;
using SLB_REST.Helpers.DataBaseStrategy;
using SLB_REST.Helpers.GetDataBaseStrategy;
using SLB_REST.Models;
using SLB_REST.Models.Albums;
using SLB_REST.Models.ViewAlbums;

namespace SLB_REST.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly EFContext _context;
        private readonly GetDataBaseStrategy _getDataBaseStrategy;


        public HomeController(EFContext context, GetDataBaseStrategy getDataBaseStrategy)
        {
            _context = context;
            _getDataBaseStrategy = getDataBaseStrategy;
        }

        public IActionResult Albums(string titleDelete = "", int idDelete = 0)
        {
            if (titleDelete != "" && titleDelete != null)
            {
                TempData["delete"] = $"Album {titleDelete} was deleted";
            }
            int nr = _context.AlbumsThumb
            .Include(a => a.Album)
            .Where(a => a.Album.ID != idDelete)
            .Where(t => t.User.UserName == User.Identity.Name).ToList().Count;
            ViewBag.max = nr;
            if (nr == -1)
            {
                ViewBag.Start = "Your collection is empty, search albums";
            }

            return View();
        }

        public IActionResult GetAlbum(string data)
        {

            JObject getData = JObject.Parse(data);
            int id = (int)getData["id"];
            dynamic album = new ExpandoObject();

            album = _getDataBaseStrategy
                .Context(new GetSingleDB())
                .Data(getData)
                .Get(_context);
            

            if (!(getData["thumbAlbum"] is null))
            {
                album.albumThumb = _context
                    .AlbumsThumb
                    .Include(a => a.Album)
                    .Where(a => a.Album.ID == id)
                    .Select(a => new
                    {
                        a.ImageThumbSrc,
                        a.Title,
                        a.Style,
                        a.Genres,
                        a.ArtistName
                    })
                    .SingleOrDefault();
            }

            return Json(album);
        }

    }
}

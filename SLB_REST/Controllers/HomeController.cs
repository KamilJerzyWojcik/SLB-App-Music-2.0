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
using SLB_REST.Models;
using SLB_REST.Models.Albums;
using SLB_REST.Models.ViewAlbums;

namespace SLB_REST.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly EFContext _context;

        public HomeController(EFContext context)
        {
            _context = context;
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
            .Where(t => t.User.UserName == User.Identity.Name).ToList().Count - 1;
            ViewBag.max = nr;
            if (nr == -1)
            {
                ViewBag.Start = "Your collection is empty, search albums";
            }

            return View();
        }

        public IActionResult GetThumbAlbums(int page, int id = -1)
        {
            if (id == -1)
            {
                dynamic albums = new ExpandoObject();

                albums.thumbAlbums = _context
                    .AlbumsThumb
                    .Include(t => t.User)
                    .Where(t => t.User.UserName == User.Identity.Name)
                    .Include(t => t.Album)
                    .Skip(page * 6).Take(6)
                    .Select(a => new {
                        a.ImageThumbSrc,
                        a.Title,
                        a.Style,
                        a.Genres,
                        a.ArtistName
                    })
                    .ToList();

                albums.id = _context
                    .AlbumsThumb
                    .Include(t => t.User)
                    .Where(t => t.User.UserName == User.Identity.Name)
                    .Include(t => t.Album)
                    .Skip(page * 6).Take(6)
                    .Select(a => a.Album.ID)
                    .ToList();

                return Json(albums);
            }
            else
            {
                dynamic album = new ExpandoObject();
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
                return Json(album);
            }
        }

        public IActionResult GetAlbum(string data)//łancuch zaleznosci
        {
            
            JObject getData = JObject.Parse(data);
            int id = (int)getData["id"];
            dynamic album = new ExpandoObject();

            if (!(getData["title"] is null))
            {
                album.title = _context
                    .Albums
                    .Where(g => g.ID == id)
                    .Select(g => g.Title)
                    .SingleOrDefault();
            }

            if (!(getData["genres"] is null))
            {
                album.genres = _context
                    .Genres
                    .Include(g => g.Album)
                    .Where(g => g.Album.ID == id)
                    .Select(g => new { g.Genre })
                    .ToList();
            }

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

            if (!(getData["styles"] is null))
            {
                album.styles = _context
                    .Styles
                    .Include(g => g.Album)
                    .Where(g => g.Album.ID == id)
                    .Select(g => new { g.Style })
                    .ToList();
            }

            if (!(getData["artists"] is null))
            {
                album.artists = _context
                    .Artists
                    .Include(g => g.Album)
                    .Where(g => g.Album.ID == id)
                    .Select(g => new
                    {
                        g.Name
                    })
                    .ToList();
            }

            if (!(getData["images"] is null))
            {
                album.images = _context
                .Images
                .Include(i => i.Album)
                .Where(i => i.Album.ID == id)
                .Select(i => i.Uri)
                .ToList();
            }

            if (!(getData["videos"] is null))
            {
                album.videos = _context
                .Videos
                .Include(v => v.Album)
                .Where(v => v.Album.ID == id)
                .Select(v => new
                {
                    v.Uri,
                    v.Description
                })
                .ToList();
            }

            if (!(getData["tracks"] is null))
            {
                album.tracks = _context
                .Tracks
                .Include(v => v.Album)
                .Where(v => v.Album.ID == id)
                .Select(t => new
                {
                    t.Duration,
                    t.Position,
                    t.Title,
                    t.ID
                })
                .ToList();

            }

            if (!(getData["extraartists"] is null))
            {
                album.extraArtists = new List<dynamic>();

                foreach (var track in album.tracks)
                {
                    int i = track.ID;

                    album.extraArtists.Add(_context
                    .ExtraArtists
                    .Include(v => v.Track)
                    .Where(v => v.Track.ID == i)
                    .Select(t => new { t.Name })
                    .ToList());
                }
            }
            return Json(album);
        }

    }
}

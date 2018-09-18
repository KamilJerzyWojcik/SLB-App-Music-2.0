using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using SLB_REST.Context;
using SLB_REST.Helpers;
using SLB_REST.Helpers.DataBaseStrategy;
using SLB_REST.Models;
using SLB_REST.Models.AlbumDiscogs.Interfaces;

namespace SLB_REST.Controllers
{
    public class EditAlbumController : Controller
    {
        private readonly EFContext _context;
        private SourceManagerDeleteAlbum _sourceManagerDeleteAlbum;
        private DatabaseStrategy _databaseStrategy;


        public EditAlbumController(
            EFContext context,
            SourceManagerDeleteAlbum sourceManagerDeleteAlbum,
            DatabaseStrategy databaseStrategy)
        {
            _context = context;
            _sourceManagerDeleteAlbum = sourceManagerDeleteAlbum;
            _databaseStrategy = databaseStrategy;
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.id = id;
            return View();
        }

        [HttpPost]
        public IActionResult Edit(string data)
        {
            int userId = _context.Users
            .Where(u => u.UserName == User.Identity.Name)
            .Single().Id;

            _databaseStrategy
            .Context(new EditDB())
            .Load(JObject.Parse(data), userId)
            .SaveChanges(_context);

            return Ok();
        }

        [HttpGet]
        public IActionResult Get(int id, string type)
        {

            if (type == "genre")
            {
                var genres = _context
                    .Genres
                    .Include(g => g.Album)
                    .Where(g => g.Album.ID == id)
                    .Select(g => g.Genre)
                    .ToList();
                return Json(genres);
            }

            if (type == "albumThumb")
            {
                var albumThumb = _context
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

                return Json(albumThumb);
            }

            if (type == "style")
            {
                var styles = _context
                    .Styles
                    .Include(g => g.Album)
                    .Where(g => g.Album.ID == id)
                    .Select(g => g.Style)
                    .ToList();
                return Json(styles);
            }

            if (type == "artist")
            {
                var artists = _context
                    .Artists
                    .Include(g => g.Album)
                    .Where(g => g.Album.ID == id)
                    .Select(g => g.Name)
                    .ToList();
                return Json(artists);
            }

            if (type == "image")
            {
                var images = _context
                .Images
                .Include(i => i.Album)
                .Where(i => i.Album.ID == id)
                .Select(i => i.Uri)
                .ToList();
                return Json(images);
            }

            if (type == "video")
            {
                var videos = _context
                .Videos
                .Include(v => v.Album)
                .Where(v => v.Album.ID == id)
                .Select(v => new {
                    v.Uri,
                    v.Description
                })
                .ToList();
                return Json(videos);
            }

            return StatusCode(404);
        }

        [HttpPost]
        public IActionResult DeleteImage(int idList, int id)
        {
            AlbumModel albumWithImages = _context.Albums
            .Where(a => a.ID == id)
            .Include(a => a.Images)
            .SingleOrDefault();

            var uri = _sourceManagerDeleteAlbum.DeleteImageUri(albumWithImages, idList);

            var imgCont = _context.Images
            .Include(im => im.Album)
            .Where(im => im.Album.ID == id && im.Uri == uri)
            .SingleOrDefault();

            _context.Images.Remove(imgCont);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost]
        public IActionResult DeleteVideo(int id, int idList)
        {
            var albumWithVideos = _context.Albums
            .Where(a => a.ID == id)
            .Include(a => a.Videos)
            .SingleOrDefault();

            var videoUri = _sourceManagerDeleteAlbum.DeleteVideoUri(albumWithVideos, idList);

            var videoCont = _context.Videos
            .Include(im => im.Album)
            .Where(im => im.Album.ID == id && im.Uri == videoUri)
            .SingleOrDefault();

            _context.Videos.Remove(videoCont);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost]
        public IActionResult DeleteAlbum(int id)
        {
            var album = _context.Albums.Where(a => a.ID == id)
            .Include(a => a.AlbumThumb).SingleOrDefault();
            _context.AlbumsThumb.Remove(album.AlbumThumb);
            _context.SaveChanges();

            var artists = _context.Artists
            .Include(a => a.Album)
            .Where(a => a.Album.ID == id).ToList();
            _sourceManagerDeleteAlbum.DeleteAlbumArtists(artists);

            var tracks = _context.Tracks
            .Include(t => t.Album)
            .Where(t => t.Album.ID == id).ToList();
            _sourceManagerDeleteAlbum.DeleteAlbumTracksAndExtraartists(tracks);

            var genres = _context.Genres
            .Include(g => g.Album)
            .Where(g => g.Album.ID == id).ToList();
            _sourceManagerDeleteAlbum.DeleteAlbumGenres(genres);


            var styles = _context.Styles
            .Include(s => s.Album)
            .Where(s => s.Album.ID == id).ToList();
            _sourceManagerDeleteAlbum.DeleteAlbumStyles(styles);

            var videos = _context.Videos
            .Include(v => v.Album)
            .Where(v => v.Album.ID == id).ToList();
            _sourceManagerDeleteAlbum.DeleteAlbumVideos(videos);

            var images = _context.Images
            .Include(i => i.Album)
            .Where(i => i.Album.ID == id).ToList();
            _sourceManagerDeleteAlbum.DeleteAlbumImages(images);

            _context.Albums.Remove(album);
            _context.SaveChanges();

            return Ok();
        }
    }
}

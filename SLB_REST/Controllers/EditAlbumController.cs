using System;
using System.Collections.Generic;
using System.Dynamic;
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
            .Data(JObject.Parse(data), userId)
            .Save(_context);

            return Ok();
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

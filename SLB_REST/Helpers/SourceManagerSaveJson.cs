using SLB_REST.Context;
using SLB_REST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLB_REST.Helpers
{
    public class SourceManagerSaveJson
    {
        private EFContext _context;
        private SourceManagerEF _sourceManagerEF;

        public SourceManagerSaveJson Load(EFContext context, SourceManagerEF sourceManagerEF)
        {
            _context = context;
            _sourceManagerEF = sourceManagerEF;
            return this;
        }

        public void addTracks(AlbumModel album)
        {
            List<TrackModel> tracks = _sourceManagerEF.GetTracks();
            int id = 0;
            if (tracks != null && tracks.Count != 0)
            {
                foreach (var track in tracks)
                {
                    track.Album = new AlbumModel();
                    track.Album.ID = album.ID;
                     _context.Tracks.Add(track);
                    _context.SaveChanges();

                    int trackId = _context.Tracks.Where(t => t.Album.ID == album.ID && t.Title == track.Title).SingleOrDefault().ID;

                    List<ExtraArtistModel> xartists = _sourceManagerEF.GetExtraArtist(id);
                    if (xartists != null && xartists.Count != 0)
                    {
                        foreach (var xartist in xartists)
                        {
                            xartist.Track = new TrackModel();
                            xartist.Track.ID = trackId;
                            _context.ExtraArtists.Add(xartist);
                        }
                    }
                    id++;
                }
            }
        }

        public void addVideos(AlbumModel album)
        {
            List<VideoModel> videos = _sourceManagerEF.GetVideos();
            if (videos != null && videos.Count != 0)
            {
                foreach (var video in videos)
                {
                    video.Album = new AlbumModel();
                    video.Album.ID = album.ID;
                    _context.Videos.Add(video);
                    _context.SaveChanges();
                }
            }
        }

        public void addStyles(AlbumModel album)
        {
            List<StyleModel> styles = _sourceManagerEF.GetStyles();
            if (styles != null && styles.Count != 0)
            {
                foreach (var style in styles)
                {
                    style.Album = new AlbumModel();
                    style.Album.ID = album.ID;
                    _context.Styles.Add(style);
                }
            }
        }

        public void addGenres(AlbumModel album)
        {
            List<GenreModel> genres = _sourceManagerEF.GetGenres();
            if (genres != null && genres.Count != 0)
            {
                foreach (var genre in genres)
                {
                    genre.Album = new AlbumModel();
                    genre.Album.ID = album.ID;
                    _context.Genres.Add(genre);
                    //_context.SaveChanges();
                }
            }
        }

        public void addImages(AlbumModel album)
        {
            List<ImageModel> images = _sourceManagerEF.GetImages();
            if (images != null && images.Count != 0)
            {
                foreach (var image in images)
                {
                    image.Album = new AlbumModel();
                    image.Album.ID = album.ID;
                    _context.Images.Add(image);
                    _context.SaveChanges();
                }
            }
        }

        public void addArtists(AlbumModel album)
        {
            List<ArtistModel> artists = _sourceManagerEF.GetArtist();
            if (artists != null)
            {
                foreach (var artist in artists)
                {
                    artist.Album = new AlbumModel();
                    artist.Album.ID = album.ID;
                    _context.Artists.Add(artist);
                    //_context.SaveChanges();
                }
            }
        }

        public void addAlbumThumb(AlbumModel album)
        {
            AlbumThumbModel albumThumb = _sourceManagerEF.GetAlbumThumb();
            if (albumThumb != null)
            {

                albumThumb.Album = new AlbumModel();
                albumThumb.Album.ID = album.ID;
                albumThumb.User = new UserModel();
                albumThumb.User.Id = album.User.Id;

                _context.AlbumsThumb.Add(albumThumb);
            }
        }
    }

}

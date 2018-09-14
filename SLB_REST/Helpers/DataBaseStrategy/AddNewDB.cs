using Newtonsoft.Json.Linq;
using SLB_REST.Context;
using SLB_REST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLB_REST.Helpers.DataBaseStrategy
{
	public class AddNewDB : ISaveChanges
	{
		public JObject JsonFile { get; private set; }
		public int UserId { get; private set; }
		private int _albumId;

		public ISaveChanges Load(JObject json, int userId)
		{
			JsonFile = json;
			UserId = userId;
			return this;
		}


		public void SaveChanges(EFContext _context )
		{
			//zmienić na łańcuch zależności
			addAlbum(_context);
			addVideos(_context);
			addImages(_context);
			addArtists(_context);
			addGenres(_context);
			addStyles(_context);
			addTracks(_context);
			addAlbumThumb(_context);
			_context.SaveChanges();

		}

		private void addAlbum(EFContext _context)
		{
			AlbumModel album = new AlbumModel()
			{
				ID = 0,
				User = new UserModel()
			};
			album.User.Id = UserId;

			if (JsonFile["title"] is null) album.Title = "no data";
			else album.Title = JsonFile["title"].ToString();

			_context.Albums.Add(album);
			_context.SaveChanges();
			_albumId = album.ID;
		}

		private void addVideos(EFContext _context)
		{
			try
			{
				foreach (var v in JsonFile["videos"])
				{
					var video = new VideoModel()
					{
						Description = v["description"].ToString(),
						Uri = v["uri"].ToString(),
						Album = new AlbumModel()
					};
					video.Album.ID = _albumId;

					_context.Videos.Add(video);
					_context.SaveChanges();
				}
			}
			catch (Exception)
			{
				var video = new VideoModel()
				{
					Description = "no data",
					Uri = "no data",
					Album = new AlbumModel()
				};
				video.Album.ID = _albumId;

				_context.Videos.Add(video);
				_context.SaveChanges();
			}

		}

		private void addImages(EFContext _context)
		{
			try
			{
				foreach (var i in JsonFile["images"])
				{
					var image = new ImageModel()
					{
						Uri = i["uri"].ToString(),
						Album = new AlbumModel()
					};
					image.Album.ID = _albumId;

					_context.Images.Add(image);
					_context.SaveChanges();
				}
			}
			catch (Exception)
			{
				var image = new ImageModel()
				{
					Uri = "no data",
					Album = new AlbumModel()
				};
				image.Album.ID = _albumId;

				_context.Images.Add(image);
				_context.SaveChanges();
			}

		}

		private void addArtists(EFContext _context)
		{
			try
			{
				foreach (var a in JsonFile["artists"])
				{
					var artist = new ArtistModel()
					{
						Name = a["name"].ToString(),
						Album = new AlbumModel()
					};
					artist.Album.ID = _albumId;

					_context.Artists.Add(artist);
					_context.SaveChanges();
				}
			}
			catch (Exception)
			{
				var artist = new ArtistModel()
				{
					Name = "no data",
					Album = new AlbumModel()
				};
				artist.Album.ID = _albumId;

				_context.Artists.Add(artist);
				_context.SaveChanges();
			}
		}

		private void addGenres(EFContext _context)
		{
			try
			{
				foreach (var g in JsonFile["genres"])
				{
					var genre = new GenreModel()
					{
						Genre = g.ToString(),
						Album = new AlbumModel()
					};
					genre.Album.ID = _albumId;

					_context.Genres.Add(genre);
					_context.SaveChanges();
				}
			}
			catch (Exception)
			{
				var genre = new GenreModel()
				{
					Genre = "no data",
					Album = new AlbumModel()
				};
				genre.Album.ID = _albumId;

				_context.Genres.Add(genre);
				_context.SaveChanges();
			}
		}

		private void addStyles(EFContext _context)
		{
			try
			{
				foreach (var s in JsonFile["styles"])
				{
					var style = new StyleModel()
					{
						Style = s.ToString(),
						Album = new AlbumModel()
					};
					style.Album.ID = _albumId;

					_context.Styles.Add(style);
					_context.SaveChanges();
				}
			}
			catch (Exception)
			{
				var style = new StyleModel()
				{
					Style = "no data",
					Album = new AlbumModel()
				};
				style.Album.ID = _albumId;

				_context.Styles.Add(style);
				_context.SaveChanges();
			}
		}

		private void addTracks(EFContext _context)
		{
			try
			{
				int i = 0;
				foreach (var t in JsonFile["tracklist"])
				{
					var track = new TrackModel()
					{
						Title = t["title"].ToString(),
						Duration = t["duration"].ToString(),
						Position = t["position"].ToString(),
						Album = new AlbumModel()
					};

					track.Album.ID = _albumId;
					_context.Tracks.Add(track);
					_context.SaveChanges();

					addExtraartists(track.ID, i, _context);
					i++;
				}
			}
			catch (Exception)
			{
				var track = new TrackModel()
				{
					Title = "no data",
					Duration = "no data",
					Position = "no data",
					Album = new AlbumModel()
				};

				track.Album.ID = _albumId;
				_context.Tracks.Add(track);
				_context.SaveChanges();
			}
		}

		private void addExtraartists(int trackId, int i, EFContext _context)
		{
			try
			{
				foreach (var x in JsonFile["tracklist"][i]["extraartists"])
				{
					var xartist = new ExtraArtistModel()
					{
						Track = new TrackModel(),
						Name = x["name"].ToString()
					};

					xartist.Track.ID = trackId;
					_context.ExtraArtists.Add(xartist);
				}
			}
			catch (Exception) { }
		}

		public void addAlbumThumb(EFContext _context)
		{
			var thumbAlbum = new AlbumThumbModel()
			{
				Album = new AlbumModel(),
				User = new UserModel(),
			};
			thumbAlbum.User.Id = UserId;
			thumbAlbum.Album.ID = _albumId;

			if (JsonFile["images"][0] is null) thumbAlbum.ImageThumbSrc = "/img/cd.jpg";
			else thumbAlbum.ImageThumbSrc = JsonFile["images"][0]["uri"].ToString();

			if (JsonFile["title"] is null) thumbAlbum.Title = "no data";
			else thumbAlbum.Title = JsonFile["title"].ToString();

			if (JsonFile["artists"][0]["name"] is null) thumbAlbum.ArtistName = "no data";
			else thumbAlbum.ArtistName = JsonFile["artists"][0]["name"].ToString();

			if (JsonFile["styles"][0] is null) thumbAlbum.Style = "no data";
			else thumbAlbum.Style = JsonFile["styles"][0].ToString();

			if (JsonFile["genres"][0] is null) thumbAlbum.Genres = "no data";
			else thumbAlbum.Genres = JsonFile["genres"][0].ToString();

			_context.AlbumsThumb.Add(thumbAlbum);

		}

		
	}
}


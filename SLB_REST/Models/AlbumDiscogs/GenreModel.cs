using SLB_REST.Models.AlbumDiscogs.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLB_REST.Models
{
	public class GenreModel : IGenre, IAlbumID
	{
		public int ID { get; set; }

        [ForeignKey("AlbumID")]
        public AlbumModel Album { get; set; }

        public string Genre { get; set; }
	}
}
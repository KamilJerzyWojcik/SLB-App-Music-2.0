using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SLB_REST.Models.AlbumDiscogs.Interfaces
{
    public interface IAlbumID
    {
        int ID { get; set; }

        [ForeignKey("AlbumID")]
        AlbumModel Album { get; set; }
    }
}

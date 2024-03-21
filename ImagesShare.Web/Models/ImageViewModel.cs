using ImagesShare.Data;

namespace ImagesShare.Web.Models
{
    public class ImageViewModel
    {
        public Image Image { get; set; }
        public bool UserHasLiked { get; set; }
    }
}

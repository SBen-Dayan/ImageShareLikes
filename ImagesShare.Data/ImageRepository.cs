using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ImagesShare.Data
{
    public class ImageRepository
    {
        private string _conStr;

        public ImageRepository(string connectionString)
        {
            _conStr = connectionString;
        }

        public List<Image> GetImages()
        {
            using var context = new ImagesDataContext(_conStr);
            return context.Images.OrderBy(i => i.DateUploaded).ToList();
        }

        public Image GetImage(int id)
        {
            using var context = new ImagesDataContext(_conStr);
            return context.Images.FirstOrDefault(i => i.Id == id);
        }

        public int? GetImageLikes(int id)
        {
            using var context = new ImagesDataContext(_conStr);
            return context.Images.FirstOrDefault(i => i.Id == id)?.Likes;
        }

        public void IncrementImageLikes(int id)
        {
            using var context = new ImagesDataContext(_conStr);
            var image = context.Images.FirstOrDefault(i => i.Id == id);
            if (image == null)
            {
                return;
            }
            image.Likes++;
            context.SaveChanges();
        }

        public void InsertImage(Image image)
        {
            using var context = new ImagesDataContext(_conStr);
            context.Images.Add(image);
            context.SaveChanges();
        }
    }
}

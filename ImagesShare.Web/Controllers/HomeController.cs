using ImagesShare.Data;
using ImagesShare.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace ImagesShare.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _connectionString;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _connectionString = configuration.GetConnectionString("conStr");
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var imageRepo = new ImageRepository(_connectionString);
            return View(new ImagesViewModel { Images = imageRepo.GetImages()});
        }

        public IActionResult ViewImage(int id)
        {
            var imageRepo = new ImageRepository(_connectionString);
            return View(new ImageViewModel
            {
                Image = imageRepo.GetImage(id),
                UserHasLiked = LikesFromSession().Contains(id)
            });
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile imageFile, string title)
        {
            var imageRepo = new ImageRepository(_connectionString);

            var imagePath = imageFile.UploadImage("uploads", _webHostEnvironment.WebRootPath);
            imageRepo.InsertImage(new()
            {
                Title = title,
                DateUploaded = DateTime.Now,
                FileName = imagePath
            });
            return Redirect("/");
        }

        public int? GetImageLikes(int id)
        {
            return new ImageRepository(_connectionString).GetImageLikes(id);
        }

        [HttpPost]
        public void IncrementImagesLikes(int id)
        {
            var likes = LikesFromSession();
            if(likes.Contains(id))
            {
                return;
            }
            likes.Add(id);
            HttpContext.Session.SetString("likes", JsonSerializer.Serialize(likes));

            var imageRepo = new ImageRepository(_connectionString);
            imageRepo.IncrementImageLikes(id);

        }

        private List<int> LikesFromSession()
        {
            var session = HttpContext.Session.GetString("likes");
            return session != null ? JsonSerializer.Deserialize<List<int>>(session) : new();
        }
    }

    public static class Extensions
    {
        public static string UploadImage(this IFormFile image, string folderName, string path)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
            var fileStream = new FileStream(Path.Combine(path, folderName, fileName), FileMode.CreateNew);
            image.CopyTo(fileStream);
            return fileName;
        }
    }
}
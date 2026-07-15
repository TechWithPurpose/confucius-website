using ConfuciusWebsite.Data;
using ConfuciusWebsite.Models;
using ConfuciusWebsite.Services;
using ConfuciusWebsite.ViewModels.NewsViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConfuciusWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;

        public NewsController(ApplicationDbContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        [HttpGet]
        public IActionResult CreateNews()
        {
            var vm = new NewsEdit
            {
                Status = "Draft"
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult CreateNews([FromForm] NewsEdit vm, [FromForm] List<IFormFile> Images)
        {
            if (vm == null)
            {
                ModelState.AddModelError("", "Invalid form submission.");
                return View("CreateNews", new NewsEdit());
            }

            if (!ModelState.IsValid)
            {
                return View("CreateNews", vm);
            }


            if (Images == null)
            {
                Images = new List<IFormFile>();
            }

            var newsToSave = new News
            {
                Title_BG = vm.Title_BG,
                Title_EN = vm.Title_EN,
                Description_BG = vm.Description_BG,
                Description_EN = vm.Description_EN,
                DateOfEvent = vm.DateOfEvent,
                Status = vm.Status,
                Tickets = vm.Tickets,
                ValidUntil = vm.ValidUntil,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.News.Add(newsToSave);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "The information was saved successfully.";

            // Handle image uploads
            int maxImages = 10;
            if (Images.Count > maxImages)
            {
                ModelState.AddModelError("Images", $"You can upload a maximum of {maxImages} images.");
                return View("CreateNews", vm);
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            long maxFileSize = 10 * 1024 * 1024; // bytes
            var validFiles = new List<IFormFile>();

            foreach (var file in Images)
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("Images", $"File type {extension} is not allowed.");
                    return View("CreateNews", vm);
                }

                if (file.Length > maxFileSize)
                {
                    ModelState.AddModelError("Images", $"File {file.FileName} exceeds the 10 MB limit.");
                    return View("CreateNews", vm);
                }

                if (!file.ContentType.StartsWith("image/"))
                {
                    ModelState.AddModelError("Images", $"File {file.FileName} is not a valid image.");
                    continue;
                }

                validFiles.Add(file);
            }

            if (!ModelState.IsValid)
            {
                return View("CreateNews", vm);
            }

            _imageService.SaveImages("News", newsToSave.Id, validFiles);

            return RedirectToAction("Overview");
        }

        [HttpGet]
        public IActionResult Overview()
        {
            var model = new NewsOverviewViewModel
            {
                News = _context.News.OrderByDescending(n => n.CreatedAt).ToList()
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var newsToEdit = _context.News.FirstOrDefault(n => n.Id == id);
            if (newsToEdit == null)
            {
                return NotFound();
            }

            if (Request.Query.ContainsKey("fileTooLarge"))
            {
                ViewBag.FileTooLarge = true;
            }

            var vm = new NewsEdit
            {
                Id = newsToEdit.Id,
                Title_BG = newsToEdit.Title_BG,
                Title_EN = newsToEdit.Title_EN,
                Description_BG = newsToEdit.Description_BG,
                Description_EN = newsToEdit.Description_EN,
                DateOfEvent = newsToEdit.DateOfEvent,
                Status = newsToEdit.Status,
                Tickets = newsToEdit.Tickets,
                ValidUntil = newsToEdit.ValidUntil
            };

            vm.AvailableImages = _imageService.GetImages("News", id);

            ModelState.Clear();

            return View("Edit", vm);
        }

        [HttpPost]
        public IActionResult Edit(NewsEdit vm, [FromForm] List<IFormFile> Images)
        {
            if (Images == null)
            {
                Images = new List<IFormFile>();
            }

            var newsToSave = _context.News.FirstOrDefault(n => n.Id == vm.Id);
            if (newsToSave == null)
            {
                return NotFound();
            }

            bool changed =
                newsToSave.Title_BG != vm.Title_BG ||
                newsToSave.Title_EN != vm.Title_EN ||
                newsToSave.Description_BG != vm.Description_BG ||
                newsToSave.Description_EN != vm.Description_EN ||
                newsToSave.DateOfEvent != vm.DateOfEvent ||
                newsToSave.Status != vm.Status ||
                newsToSave.Tickets != vm.Tickets ||
                newsToSave.ValidUntil != vm.ValidUntil;

            if (changed)
            {
                newsToSave.Title_BG = vm.Title_BG;
                newsToSave.Title_EN = vm.Title_EN;
                newsToSave.Description_BG = vm.Description_BG;
                newsToSave.Description_EN = vm.Description_EN;
                newsToSave.DateOfEvent = vm.DateOfEvent;
                newsToSave.Status = vm.Status;
                newsToSave.Tickets = vm.Tickets;
                newsToSave.ValidUntil = vm.ValidUntil;
                newsToSave.UpdatedAt = DateTime.Now;

                _context.SaveChanges();

                TempData["SuccessMessage"] = "The information was saved successfully.";
            }

            // Handle image uploads
            int maxImages = 10;
            if (Images.Count > maxImages)
            {
                ModelState.AddModelError("Images", $"You can upload a maximum of {maxImages} images.");
                vm.AvailableImages = _imageService.GetImages("News", vm.Id);
                ModelState.Clear();
                return View("Edit", vm);
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            long maxFileSize = 10 * 1024 * 1024; // bytes
            var validFiles = new List<IFormFile>();

            foreach (var file in Images)
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("Images", $"File type {extension} is not allowed.");
                    vm.AvailableImages = _imageService.GetImages("News", vm.Id);
                    ModelState.Clear();
                    return View("Edit", vm);
                }

                if (file.Length > maxFileSize)
                {
                    ModelState.AddModelError("Images", $"File {file.FileName} exceeds the 10 MB limit.");
                    vm.AvailableImages = _imageService.GetImages("News", vm.Id);
                    ModelState.Clear();
                    return View("Edit", vm);
                }

                if (!file.ContentType.StartsWith("image/"))
                {
                    ModelState.AddModelError("Images", $"File {file.FileName} is not a valid image.");
                    continue;
                }

                validFiles.Add(file);
            }

            if (!ModelState.IsValid)
            {
                vm.AvailableImages = _imageService.GetImages("News", vm.Id);
                ModelState.Clear();
                return View("Edit", vm);
            }

            _imageService.SaveImages("News", newsToSave.Id, validFiles);

            return RedirectToAction("Overview");
        }

        [HttpPost]
        public IActionResult DeleteImage(int id)
        {
            _imageService.DeleteImageById(id);
            return Ok();
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var newsToDelete = _context.News.FirstOrDefault(n => n.Id == id);
            if (newsToDelete == null)
            {
                return NotFound();
            }

            _imageService.DeleteImages("News", id);
            _context.News.Remove(newsToDelete);

            _context.SaveChanges();

            TempData["SuccessMessage"] = "News deleted successfully.";
            return RedirectToAction("Overview", "News", new { area = "Admin" });
        }
    }
}

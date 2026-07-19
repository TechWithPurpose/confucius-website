using ConfuciusWebsite.Data;
using ConfuciusWebsite.Models;
using ConfuciusWebsite.Services;
using ConfuciusWebsite.ViewModels.ClassViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConfuciusWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ClassesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;

        public ClassesController(ApplicationDbContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        [HttpGet]
        public IActionResult CreateClass()
        {
            var vm = new ClassEdit();
            // Start with one blank schedule row so the form isn't empty on first load
            vm.Schedules.Add(new ScheduleItem());

            return View(vm);
        }

        [HttpPost]
        public IActionResult CreateClass([FromForm] ClassEdit vm, [FromForm] List<IFormFile> Images)
        {
            if (vm == null)
            {
                ModelState.AddModelError("", "Invalid form submission.");
                return View("CreateClass", new ClassEdit());
            }

            if (!ModelState.IsValid)
            {
                return View("CreateClass", vm);
            }

            if (Images == null)
            {
                Images = new List<IFormFile>();
            }

            var classToSave = new Classes
            {
                Title_BG = vm.Title_BG,
                Title_EN = vm.Title_EN,
                Description_BG = vm.Description_BG,
                Description_EN = vm.Description_EN,
                StartDate = vm.StartDate,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Classes.Add(classToSave);
            _context.SaveChanges();

            SaveSchedules(classToSave.Id, vm.Schedules);

            TempData["SuccessMessage"] = "The information was saved successfully.";

            // Handle image uploads
            int maxImages = 10;
            if (Images.Count > maxImages)
            {
                ModelState.AddModelError("Images", $"You can upload a maximum of {maxImages} images.");
                return View("CreateClass", vm);
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
                    return View("CreateClass", vm);
                }

                if (file.Length > maxFileSize)
                {
                    ModelState.AddModelError("Images", $"File {file.FileName} exceeds the 10 MB limit.");
                    return View("CreateClass", vm);
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
                return View("CreateClass", vm);
            }

            _imageService.SaveImages("Class", classToSave.Id, validFiles);

            return RedirectToAction("Overview");
        }

        [HttpGet]
        public IActionResult Overview()
        {
            var model = new ClassesOverviewViewModel
            {
                Classes = _context.Classes
                    .OrderBy(c => c.Title_EN)
                    .ToList(),

                NewRequests = _context.ClassSignups
                    .Include(s => s.Class)
                    .Where(s => !s.Contacted)
                    .OrderByDescending(s => s.CreatedAt)
                    .ToList(),

                OldRequests = _context.ClassSignups
                    .Include(s => s.Class)
                    .Where(s => s.Contacted)
                    .OrderByDescending(s => s.CreatedAt)
                    .ToList()
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var classToEdit = _context.Classes
                .Include(c => c.Schedules)
                .FirstOrDefault(c => c.Id == id);

            if (classToEdit == null)
            {
                return NotFound();
            }

            if (Request.Query.ContainsKey("fileTooLarge"))
            {
                ViewBag.FileTooLarge = true;
            }

            var vm = new ClassEdit
            {
                Id = classToEdit.Id,
                Title_BG = classToEdit.Title_BG,
                Title_EN = classToEdit.Title_EN,
                Description_BG = classToEdit.Description_BG,
                Description_EN = classToEdit.Description_EN,
                StartDate = classToEdit.StartDate,
                Schedules = classToEdit.Schedules
                    .Select(s => new ScheduleItem
                    {
                        Id = s.Id,
                        DayOfWeek = s.DayOfWeek,
                        StartTime = s.StartTime,
                        EndTime = s.EndTime
                    })
                    .ToList()
            };

            if (!vm.Schedules.Any())
            {
                vm.Schedules.Add(new ScheduleItem());
            }

            vm.AvailableImages = _imageService.GetImages("Class", id);

            ModelState.Clear();

            return View("Edit", vm);
        }

        [HttpPost]
        public IActionResult Edit(ClassEdit vm, [FromForm] List<IFormFile> Images)
        {
            var classToSave = _context.Classes.FirstOrDefault(c => c.Id == vm.Id);
            if (classToSave == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                vm.AvailableImages = _imageService.GetImages("Class", vm.Id);
                ModelState.Clear();
                return View("Edit", vm);
            }

            if (Images == null)
            {
                Images = new List<IFormFile>();
            }

            classToSave.Title_BG = vm.Title_BG;
            classToSave.Title_EN = vm.Title_EN;
            classToSave.Description_BG = vm.Description_BG;
            classToSave.Description_EN = vm.Description_EN;
            classToSave.StartDate = vm.StartDate;
            classToSave.UpdatedAt = DateTime.Now;

            _context.SaveChanges();

            SaveSchedules(classToSave.Id, vm.Schedules);

            TempData["SuccessMessage"] = "The information was saved successfully.";

            // Handle image uploads
            int maxImages = 10;
            if (Images.Count > maxImages)
            {
                ModelState.AddModelError("Images", $"You can upload a maximum of {maxImages} images.");
                vm.AvailableImages = _imageService.GetImages("Class", vm.Id);
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
                    vm.AvailableImages = _imageService.GetImages("Class", vm.Id);
                    ModelState.Clear();
                    return View("Edit", vm);
                }

                if (file.Length > maxFileSize)
                {
                    ModelState.AddModelError("Images", $"File {file.FileName} exceeds the 10 MB limit.");
                    vm.AvailableImages = _imageService.GetImages("Class", vm.Id);
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
                vm.AvailableImages = _imageService.GetImages("Class", vm.Id);
                ModelState.Clear();
                return View("Edit", vm);
            }

            _imageService.SaveImages("Class", classToSave.Id, validFiles);

            return RedirectToAction("Overview");
        }

        [HttpPost]
        public IActionResult DeleteImage(int id)
        {
            _imageService.DeleteImageById(id);
            return Ok();
        }

        // Replaces all schedule rows for a class with the ones submitted from the form.
        // Simpler and safer than trying to diff/match existing rows against edited ones.
        private void SaveSchedules(int classId, List<ScheduleItem> schedules)
        {
            var existing = _context.ClassSchedule.Where(s => s.ClassId == classId);
            _context.ClassSchedule.RemoveRange(existing);

            if (schedules != null)
            {
                foreach (var item in schedules)
                {
                    // Skip rows the user added but never filled in (e.g. left at defaults with 00:00-00:00)
                    if (item.StartTime == default && item.EndTime == default)
                    {
                        continue;
                    }

                    _context.ClassSchedule.Add(new ClassSchedule
                    {
                        ClassId = classId,
                        DayOfWeek = item.DayOfWeek,
                        StartTime = item.StartTime,
                        EndTime = item.EndTime
                    });
                }
            }

            _context.SaveChanges();
        }

        // Toggles the Contacted flag for a signup request.
        // Called via fetch() from Overview.cshtml so the row can move tables without a page reload.
        [HttpPost]
        public IActionResult ToggleContacted(int id, [FromBody] bool contacted)
        {
            var signup = _context.ClassSignups.FirstOrDefault(s => s.Id == id);
            if (signup == null)
            {
                return NotFound();
            }

            signup.Contacted = contacted;
            _context.SaveChanges();

            return Ok();
        }

        // Deletes a single signup request. Called via fetch() so the row can be removed without a page reload.
        [HttpPost]
        public IActionResult DeleteSignup(int id)
        {
            var signup = _context.ClassSignups.FirstOrDefault(s => s.Id == id);
            if (signup == null)
            {
                return NotFound();
            }

            _context.ClassSignups.Remove(signup);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var classToDelete = _context.Classes.FirstOrDefault(c => c.Id == id);
            if (classToDelete == null)
            {
                return NotFound();
            }

            // Signups and schedules for this class cascade-delete at the DB level (see FK config).
            _imageService.DeleteImages("Class", id);
            _context.Classes.Remove(classToDelete);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Class deleted successfully.";
            return RedirectToAction("Overview", "Classes", new { area = "Admin" });
        }
    }
}

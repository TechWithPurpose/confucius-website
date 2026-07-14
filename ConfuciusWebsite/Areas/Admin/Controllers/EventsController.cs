using ConfuciusWebsite.Data;
using ConfuciusWebsite.Models;
using ConfuciusWebsite.Services;
using ConfuciusWebsite.ViewModels.EventViewModels;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;


namespace ConfuciusWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IImageService _imageService;

        public EventsController(ApplicationDbContext context, IWebHostEnvironment environment, IImageService imageService)
        {
            _context = context;
            _environment = environment;
            _imageService = imageService;
        }


        [HttpGet]
        public IActionResult Create()
        {
            // Initialize a new ViewModel with default values
            var vm = new EventsEdit
            {
                Status = "Draft",
                Tickets = "NoTickets",
                DateOfEvent = DateTime.Today,
                Deadline = DateTime.Today
            };


            return View(vm);
        }

        [HttpPost]
        public IActionResult Create([FromForm]  EventsEdit vm, [FromForm] List<IFormFile> Images)
        {
            if (vm == null)
            {
                ModelState.AddModelError("", "Invalid form submission.");
                return View("Create", new EventsEdit());
            }

            if (Images == null)
            {
                Images = new List<IFormFile>(); // avoid null later
            }

            // Create new
            var eventToSave = new Event
                {
                    Title_BG = vm.Title_BG,
                    Title_EN = vm.Title_EN,
                    Description_BG = vm.Description_BG,
                    Description_EN = vm.Description_EN,
                    Photographer = vm.Photographer,
                    Author = vm.Author,
                    Translator = vm.Translator,
                    Status = vm.Status,
                    Tickets = vm.Tickets,
                    DateOfEvent = vm.DateOfEvent,
                    Deadline = vm.Deadline,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

            // Add the new entity to the database
            _context.Events.Add(eventToSave);

            _context.SaveChanges();

            TempData["SuccessMessage"] = "The information was saved successfully.";

            // Handle image uploads
            // 1. Max number of images
            int maxImages = 10;
            if (Images.Count > maxImages)
            {
                ModelState.AddModelError("Images", $"You can upload a maximum of {maxImages} images.");
                return View(vm);
            }

            // 2. Allowed extensions
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            // 3. File size limit (e.g., 10 MB)
            long maxFileSize = 10 * 1024 * 1024; // bytes
            var validFiles = new List<IFormFile>();

            foreach (var file in Images ?? Enumerable.Empty<IFormFile>())
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("Images", $"File type {extension} is not allowed.");
                    return View("Create", vm);
                }

                if (file.Length > maxFileSize)
                {
                    ModelState.AddModelError("Images", $"File {file.FileName} exceeds the 10 MB limit.");
                    return View("Create", vm);
                }

                if (!file.ContentType.StartsWith("image/"))
                {
                    ModelState.AddModelError("Images", $"File {file.FileName} is not a valid image.");
                    continue; // skip this file
                }

                validFiles.Add(file);
            }

            if (!ModelState.IsValid)
            {
                return View("Create", vm); // show errors
            }

            // Only save valid files
            _imageService.SaveImages("Event", eventToSave.Id, validFiles);


            return RedirectToAction("Overview");
        }

        [HttpGet]
        public IActionResult Overview()
        {
            var model = new EventsOverviewViewModel
            {
                Events = _context.Events.ToList()
            };


            return View(model);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            //Console.WriteLine("DEBUG: Edit GET reached with id = " + id);
            var eventToEdit = _context.Events.FirstOrDefault(e => e.Id == id);
            if (eventToEdit == null)
            {
                return NotFound();
            }

            if (Request.Query.ContainsKey("fileTooLarge"))
            {
                ViewBag.FileTooLarge = true;
            }

            // Map the entity to the ViewModel
            var vm = new EventsEdit
            {
                Id = eventToEdit.Id,
                Title_BG = eventToEdit.Title_BG,
                Title_EN = eventToEdit.Title_EN,
                Description_BG = eventToEdit.Description_BG,
                Description_EN = eventToEdit.Description_EN,
                Photographer = eventToEdit.Photographer,
                Author = eventToEdit.Author,
                Translator = eventToEdit.Translator,
                Status = eventToEdit.Status,
                Tickets = eventToEdit.Tickets,
                DateOfEvent = eventToEdit.DateOfEvent,
                Deadline = eventToEdit.Deadline
            };

            //Console.WriteLine("DEBUG: eventToEdit.Title_EN = " + eventToEdit.Title_EN);
            //Console.WriteLine("DEBUG: vm.Title_EN = " + vm.Title_EN);
            //Console.WriteLine("ID received: " + id);
            //Console.WriteLine("Event title: " + eventToEdit.Title_EN);
            //Console.WriteLine("Event title: " + vm.Title_EN);

            vm.AvailableImages = _imageService.GetImages("Event", id);

            // Clear ModelState so Razor uses vm instead of corrupted values
            ModelState.Clear();

            return View("Edit", vm);
        }

        [HttpPost]
        public IActionResult Edit(EventsEdit vm, [FromForm] List<IFormFile> Images)
        {
            Console.WriteLine(">>> POST HIT <<<");
            Console.WriteLine("Content type: " + Request.ContentType);

            try
            {
                Console.WriteLine("Form keys: " + string.Join(", ", Request.Form.Keys));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Form read error: " + ex.Message);
            }

            Console.WriteLine("vm is null? " + (vm == null));
            Console.WriteLine("Images is null? " + (Images == null));

            if (Images == null)
            {
                Images = new List<IFormFile>(); // avoid null later
            }

            // Retrieve the existing entity from the database
            var eventToSave = _context.Events.FirstOrDefault(e => e.Id == vm.Id);
            if (eventToSave == null)
            {
                return NotFound();
            }

            // Compare values to detect changes
            bool changed =
                eventToSave.Title_BG != vm.Title_BG ||
                eventToSave.Title_EN != vm.Title_EN ||
                eventToSave.Description_BG != vm.Description_BG ||
                eventToSave.Description_EN != vm.Description_EN ||
                eventToSave.Photographer != vm.Photographer ||
                eventToSave.Author != vm.Author ||
                eventToSave.Translator != vm.Translator ||
                eventToSave.Status != vm.Status ||
                eventToSave.Tickets != vm.Tickets ||
                eventToSave.DateOfEvent != vm.DateOfEvent ||
                eventToSave.Deadline != vm.Deadline;


            if (changed)
            {
                // Update the entity with values from the ViewModel
                eventToSave.Title_BG = vm.Title_BG;
                eventToSave.Title_EN = vm.Title_EN;
                eventToSave.Description_BG = vm.Description_BG;
                eventToSave.Description_EN = vm.Description_EN;
                eventToSave.Photographer = vm.Photographer;
                eventToSave.Author = vm.Author;
                eventToSave.Translator = vm.Translator;
                eventToSave.Status = vm.Status;
                eventToSave.Tickets = vm.Tickets;
                eventToSave.DateOfEvent = vm.DateOfEvent;
                eventToSave.Deadline = vm.Deadline;
                eventToSave.UpdatedAt = DateTime.Now;

                _context.SaveChanges();

                TempData["SuccessMessage"] = "The information was saved successfully.";
            }

            // Handle image uploads
            // 1. Max number of images
            int maxImages = 10;
            if (Images.Count > maxImages)
            {
                ModelState.AddModelError("Images", $"You can upload a maximum of {maxImages} images.");
                vm.AvailableImages = _imageService.GetImages("Event", vm.Id);

                // Clear ModelState so Razor uses vm instead of corrupted values
                ModelState.Clear();
                return View(vm);
            }

            // 2. Allowed extensions
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            // 3. File size limit (e.g., 10 MB)
            long maxFileSize = 10 * 1024 * 1024; // bytes
            var validFiles = new List<IFormFile>();

            foreach (var file in Images ?? Enumerable.Empty<IFormFile>())
            {

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("Images", $"File type {extension} is not allowed.");

                    vm.AvailableImages = _imageService.GetImages("Event", vm.Id);

                    // Clear ModelState so Razor uses vm instead of corrupted values
                    ModelState.Clear();
                    return View("Edit", vm);
                }

                if (file.Length > maxFileSize)
                {
                    ModelState.AddModelError("Images", $"File {file.FileName} exceeds the 10 MB limit.");

                    vm.AvailableImages = _imageService.GetImages("Event", vm.Id);

                    // Clear ModelState so Razor uses vm instead of corrupted values
                    ModelState.Clear();
                    return View("Edit", vm);
                }

                if (!file.ContentType.StartsWith("image/"))
                {
                    ModelState.AddModelError("Images", $"File {file.FileName} is not a valid image.");
                    continue; // skip this file
                }

                validFiles.Add(file);
            }

            if (!ModelState.IsValid)
            {

                vm.AvailableImages = _imageService.GetImages("Event", vm.Id);

                // Clear ModelState so Razor uses vm instead of corrupted values
                ModelState.Clear();
                return View("Edit", vm); // show errors
            }

            // Only save valid files
            _imageService.SaveImages("Event", eventToSave.Id, validFiles);

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
            var eventToDelete = _context.Events.FirstOrDefault(e => e.Id == id);
            if (eventToDelete == null)
            {
                return NotFound();
            }

            // Use the image service to delete images
            _imageService.DeleteImages("Event", id);
            // 4. Delete the event
            _context.Events.Remove(eventToDelete);

            _context.SaveChanges();

            TempData["SuccessMessage"] = "Event deleted successfully.";
            return RedirectToAction("Overview", "Events", new { area = "Admin" });
        }


    }
}


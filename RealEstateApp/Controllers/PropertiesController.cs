using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Data;
using RealEstateApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateApp.Controllers
{
    [Authorize] // Default security: User must be logged in to access
    public class PropertiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public PropertiesController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Properties
        [AllowAnonymous]
        public async Task<IActionResult> Index(string searchString, decimal? minPrice, decimal? maxPrice, string propertyType)
        {
            // 1. Start with ALL properties
            var properties = from p in _context.Properties
                             select p;

            // 2. Debug check (Optional: helps verify search is hitting here)
            if (!string.IsNullOrEmpty(searchString))
            {
                // Filter by Title OR Address
                properties = properties.Where(s => s.Title.Contains(searchString)
                                                || s.Address.Contains(searchString));
            }

            // 3. Filter by Price
            if (minPrice.HasValue)
            {
                properties = properties.Where(p => p.Price >= minPrice);
            }
            if (maxPrice.HasValue)
            {
                properties = properties.Where(p => p.Price <= maxPrice);
            }

            // 4. Filter by Type (Buy/Rent)
            if (!string.IsNullOrEmpty(propertyType) && propertyType != "all")
            {
                if (propertyType == "buy")
                {
                    properties = properties.Where(p => p.IsForSale == true);
                }
                else if (propertyType == "rent")
                {
                    properties = properties.Where(p => p.IsForSale == false);
                }
            }

            // 5. Return the FILTERED list to the View
            // (Important: We pass the search terms back to ViewData so the inputs don't clear out)
            ViewData["CurrentFilter"] = searchString;
            ViewData["MinPrice"] = minPrice;
            ViewData["MaxPrice"] = maxPrice;
            ViewData["PropertyType"] = propertyType;

            return View(await properties.ToListAsync());
        }

        // GET: Properties/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            // 1. Get the Main Property
            var property = await _context.Properties
                .Include(p => p.Images)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (property == null) return NotFound();

            // 2. Check Saved Status (Your existing code)
            bool isSaved = false;
            if (User.Identity.IsAuthenticated)
            {
                var currentUserEmail = User.Identity.Name;
                isSaved = await _context.SavedProperties
                    .AnyAsync(s => s.UserEmail == currentUserEmail && s.PropertyId == id);
            }
            ViewBag.IsSaved = isSaved;

            // --- 3. THE AI ALGORITHM (Recommendation Engine) ---
            // Logic: Find 3 other properties that are:
            // A. Not the current house
            // B. Same Type (Rent vs Sale)
            // C. Similar Price (+/- 20%)
            // D. (Optional) Similar location by checking if Address contains same words

            var similarProperties = await _context.Properties
                .Where(p => p.Id != id) // Don't show the same house
                .Where(p => p.IsForSale == property.IsForSale) // Must be same type (Buy/Rent)
                .Where(p => p.Price >= (property.Price * 0.7m) && p.Price <= (property.Price * 1.3m)) // Price within 30% range
                .OrderBy(r => Guid.NewGuid()) // Randomize the order so it looks dynamic
                .Take(3) // Show top 3 matches
                .ToListAsync();

            // Pass the list to the View
            ViewBag.SimilarProperties = similarProperties;

            return View(property);
        }



        // 1. GET: Properties/Create (Shows the empty form)
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // 2. POST: Properties/Create (Receives the data and images)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Property property, List<IFormFile> imageFiles)
        {
            // NOTE: We do NOT create a "var property" here because it is passed 
            // as a parameter in the line above. This fixes the "Already Defined" error.

            if (ModelState.IsValid)
            {
                // Handle Multiple Images
                if (imageFiles != null && imageFiles.Count > 0)
                {
                    foreach (var file in imageFiles)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string uploadPath = Path.Combine(_hostEnvironment.WebRootPath, "images");

                        // Create folder if missing
                        if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

                        // Save file
                        using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        // Add to list
                        property.Images.Add(new PropertyImage { Url = fileName });
                    }

                    // Set main image
                    property.ImageUrl = property.Images[0].Url;
                }

                _context.Add(property);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(property);
        }


        // 1. GET: Properties/Edit/5
        // (Displays the form and existing images)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            // We must INCLUDE images so the gallery manager works
            var property = await _context.Properties
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (property == null) return NotFound();
            return View(property);
        }

        // 2. POST: Properties/Edit/5
        // (Saves changes and handles new uploads)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Property property, List<IFormFile> imageFiles)
        {
            if (id != property.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // A. Handle NEW Images being added
                    if (imageFiles != null && imageFiles.Count > 0)
                    {
                        foreach (var file in imageFiles)
                        {
                            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            string uploadPath = Path.Combine(_hostEnvironment.WebRootPath, "images");

                            // Ensure folder exists
                            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

                            using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }

                            // Create the DB record for this image
                            var newImage = new PropertyImage
                            {
                                Url = fileName,
                                PropertyId = id
                            };

                            _context.PropertyImages.Add(newImage);
                        }
                    }

                    // B. Update the main property details
                    _context.Update(property);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Properties.Any(e => e.Id == property.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(property);
        }

        // POST: Properties/DeleteImage/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("Properties/DeleteImage/{id}")] // <--- Ensure this attribute is here just in case
        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await _context.PropertyImages.FindAsync(id);

            if (image != null)
            {
                // Delete file logic...
                string filePath = Path.Combine(_hostEnvironment.WebRootPath, "images", image.Url);
                if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);

                // Database delete
                _context.PropertyImages.Remove(image);
                await _context.SaveChangesAsync();

                return RedirectToAction("Edit", new { id = image.PropertyId });
            }
            return RedirectToAction(nameof(Index));
        }

        // 1. GET: Properties/Delete/5
        // (Asks "Are you sure?")
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var property = await _context.Properties
                .FirstOrDefaultAsync(m => m.Id == id);

            if (property == null) return NotFound();

            return View(property);
        }

        // 2. POST: Properties/Delete/5
        // (Actually deletes it)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property != null)
            {
                _context.Properties.Remove(property);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // POST: Properties/SendInquiry
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous] // Important: Allow guests to send messages!
        public async Task<IActionResult> SendInquiry(int propertyId, string userEmail, string message)
        {
            // 1. Create the inquiry object
            var inquiry = new Inquiry
            {
                PropertyId = propertyId,
                UserEmail = userEmail,
                Message = message,
                DateCreated = DateTime.Now
            };

            // 2. Save to Database
            _context.Inquiries.Add(inquiry);
            await _context.SaveChangesAsync();

            // 3. Show success message
            TempData["SuccessMessage"] = "Your message has been sent to the agent!";

            // 4. Redirect back to the SAME property page
            return RedirectToAction("Details", new { id = propertyId });
        }



    }

    }
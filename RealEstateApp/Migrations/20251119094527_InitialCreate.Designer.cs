using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RealEstateApp.Data;
using RealEstateApp.Models;

namespace RealEstateApp.Pages.Properties
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CreateModel(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        [BindProperty]
        public Property Property { get; set; } = default!;

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (ImageFile != null)
            {
                // 1. Generate a unique filename to avoid duplicates
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);

                // 2. Define the path: wwwroot/images/fileName
                string uploadPath = Path.Combine(_hostEnvironment.WebRootPath, "images");
                Directory.CreateDirectory(uploadPath); // ensure directory exists
                string filePath = Path.Combine(uploadPath, fileName);

                // 3. Save the file to the server
                await using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }

                // 4. Save the filename to the database object
                Property.ImageUrl = fileName;
            }

            _context.Properties.Add(Property);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

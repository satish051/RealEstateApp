using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Data;
using RealEstateApp.Models;

namespace RealEstateApp.Controllers
{
    [Authorize(Roles = "Admin")] // Default: Only Admins can touch this controller
    public class AgentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AgentsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Agents (Public - Everyone can see the list)
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Agents.ToListAsync());
        }

        // GET: Agents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Agents/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Agent agent, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    string uploadPath = Path.Combine(_hostEnvironment.WebRootPath, "images");
                    using (var stream = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    agent.ImageUrl = fileName;
                }
                else
                {
                    agent.ImageUrl = "default.png"; // You can put a default dummy image in wwwroot/images
                }

                _context.Add(agent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(agent);
        }

        // GET: Agents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var agent = await _context.Agents.FindAsync(id);
            if (agent == null) return NotFound();

            // Delete image file
            if (agent.ImageUrl != "default.png")
            {
                string path = Path.Combine(_hostEnvironment.WebRootPath, "images", agent.ImageUrl);
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            }

            _context.Agents.Remove(agent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
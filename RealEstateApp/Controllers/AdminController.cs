using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using RealEstateApp.Data;

[Authorize(Roles = "Admin")] // <--- Only users with "Admin" role can enter
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Admin/Inquiries
    public async Task<IActionResult> Inquiries()
    {
        var inquiries = await _context.Inquiries
            .Include(i => i.Property)
            .Where(i => i.IsArchived == false) // <--- Filter out archived ones
            .OrderByDescending(i => i.DateCreated)
            .ToListAsync();

        return View(inquiries);
    }


    // POST: Admin/ArchiveInquiry/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ArchiveInquiry(int id)
    {
        var inquiry = await _context.Inquiries.FindAsync(id);
        if (inquiry != null)
        {
            inquiry.IsArchived = true; // Mark as archived
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Inquiries));
    }

}
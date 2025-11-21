using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Data;
using RealEstateApp.Models;

[Authorize] // Locks this entire controller to logged-in users
public class UserPanelController : Controller
{
    private readonly ApplicationDbContext _context;

    public UserPanelController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: UserPanel/MyInquiries
    public async Task<IActionResult> MyInquiries()
    {
        var currentUserEmail = User.Identity?.Name;
        if (currentUserEmail == null) return RedirectToAction("Index", "Home");

        var myInquiries = await _context.Inquiries
            .Include(i => i.Property)
            .Where(i => i.UserEmail == currentUserEmail)
            .Where(i => i.IsArchived == false) // <--- ADD THIS LINE (Hides archived)
            .OrderByDescending(i => i.DateCreated)
            .ToListAsync();

        return View(myInquiries);
    }

    // GET: UserPanel/MyFavorites
    public async Task<IActionResult> MyFavorites()
    {
        var currentUserEmail = User.Identity?.Name;
        if (currentUserEmail == null) return RedirectToAction("Index", "Home");

        // Fetch saved properties and include the Property details (Price, Image, etc.)
        var favorites = await _context.SavedProperties
            .Include(s => s.Property)
            .Where(s => s.UserEmail == currentUserEmail)
            .ToListAsync();

        return View(favorites);
    }


    [HttpPost]
    public async Task<IActionResult> ToggleSave(int propertyId)
    {
        var email = User.Identity.Name;
        // Check if already saved
        var existing = await _context.SavedProperties
            .FirstOrDefaultAsync(s => s.UserEmail == email && s.PropertyId == propertyId);

        if (existing != null)
        {
            _context.SavedProperties.Remove(existing); // Un-save
        }
        else
        {
            _context.SavedProperties.Add(new SavedProperty { UserEmail = email, PropertyId = propertyId }); // Save
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Details", "Properties", new { id = propertyId });

    }

    // POST: UserPanel/DeleteInquiry/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteInquiry(int id)
    {
        // 1. Identify the current user
        var currentUserEmail = User.Identity?.Name;
        if (currentUserEmail == null) return RedirectToAction("Index", "Home");

        // 2. Find the inquiry
        var inquiry = await _context.Inquiries.FindAsync(id);

        if (inquiry != null)
        {
            // 3. SECURITY CHECK: 
            // Only allow delete if the inquiry belongs to this user!
            if (inquiry.UserEmail == currentUserEmail)
            {
                _context.Inquiries.Remove(inquiry);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Inquiry deleted successfully.";
            }
            else
            {
                // If user tries to hack/delete someone else's msg
                return Forbid();
            }
        }

        return RedirectToAction(nameof(MyInquiries));
    }

}
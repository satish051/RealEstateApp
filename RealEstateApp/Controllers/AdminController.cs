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
        // 1. Fetch Stats for the Dashboard Cards
        ViewBag.TotalProperties = await _context.Properties.CountAsync();
        ViewBag.TotalAgents = await _context.Agents.CountAsync();
        ViewBag.ActiveInquiries = await _context.Inquiries.Where(i => !i.IsArchived).CountAsync();
        ViewBag.TotalValue = await _context.Properties.SumAsync(p => p.Price); // Total value of market

        // 2. Fetch the actual list of inquiries
        var inquiries = await _context.Inquiries
            .Include(i => i.Property)
            .Where(i => i.IsArchived == false)
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

    // GET: Admin/ExportInquiries
    public async Task<IActionResult> ExportInquiries()
    {
        // 1. Fetch the data (Same logic as the dashboard list)
        var inquiries = await _context.Inquiries
            .Include(i => i.Property)
            .Where(i => i.IsArchived == false)
            .OrderByDescending(i => i.DateCreated)
            .ToListAsync();

        // 2. Build the CSV String
        var builder = new System.Text.StringBuilder();

        // Header Row
        builder.AppendLine("Date,Client Email,Property Title,Property Price,Agent Name,Message");

        // Data Rows
        foreach (var item in inquiries)
        {
            // We use replace(",", " ") to prevent commas in the message from breaking the CSV format
            var date = item.DateCreated.ToString("yyyy-MM-dd HH:mm");
            var email = item.UserEmail;
            var title = item.Property != null ? item.Property.Title.Replace(",", " ") : "Deleted Property";
            var price = item.Property != null ? item.Property.Price.ToString() : "0";
            var agent = item.Property != null ? item.Property.AgentName : "N/A";
            var msg = item.Message.Replace(",", " ").Replace("\n", " ");

            builder.AppendLine($"{date},{email},{title},{price},{agent},{msg}");
        }

        // 3. Return as a File Download
        return File(System.Text.Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "inquiries_export.csv");
    }


}
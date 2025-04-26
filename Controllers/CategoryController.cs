using Microsoft.AspNetCore.Mvc;
using SimpleNotesApp.Data;
using SimpleNotesApp.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Localization;

namespace SimpleNotesApp.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IStringLocalizer<CategoryController> _localizer;

        public CategoryController(AppDbContext context, IStringLocalizer<CategoryController> localizer)
        {
            _localizer =localizer;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            return View(await _context.Categories.Where(c => c.UserId == userId).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category model)
        {
            if (ModelState.IsValid)
            {
                model.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var existingCategory = await _context.Categories
                        .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
                    
                    if(existingCategory == null)
                    {
                        return NotFound();
                    }

                    existingCategory.Name = category.Name;
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "An error has occurred during the update.");
                }
            }
            return View(category);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
            if (category == null)
            {
                return NotFound();
            }

            // Check the number of related notes
            int relatedNotesCount = _context.Notes.Count(n => n.CategoryId == id);
            ViewBag.RelatedNotesCount = relatedNotesCount;

            return View(category);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                var unassignedCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Name == "Kategorisiz" && c.UserId == userId);

                if(unassignedCategory == null)
                {
                    unassignedCategory = new Category
                    {
                        Name = "Kategorisiz",UserId = userId
                    };
                    _context.Categories.Add(unassignedCategory);
                    await _context.SaveChangesAsync();
                }

                var category = await _context.Categories
                    .Include(c => c.Notes)
                    .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

                if (category == null)
                {
                    return NotFound();
                }
                
                if (category.Notes != null)
                {
                    foreach(var note in category.Notes)
                    {
                        note.CategoryId = unassignedCategory.Id;
                        note.Category = unassignedCategory;
                    }
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                TempData["Success"] = _localizer["DeleteCategoryMessage"].Value;
                return RedirectToAction(nameof(Index));

            }
            catch(Exception ex)
            {
                TempData["Error"] = _localizer["DeleteCategoryErrorMessage"] + ex.Message;
                return RedirectToAction(nameof(Index));
            }

        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
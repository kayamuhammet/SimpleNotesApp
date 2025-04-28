using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SimpleNotesApp.Data;
using SimpleNotesApp.Models;

namespace SimpleNotesApp.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IStringLocalizer<NotesController> _localizer;

        public NotesController(AppDbContext context, IStringLocalizer<NotesController> localizer)
        {
            _localizer = localizer;
            _context = context;
        }

        // Get: Notes
        public IActionResult Index(int? categoryId, int? noteId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var categories = _context.Categories.Where(c => c.UserId == userId). ToList();
            
            // If no category is selected, select the first category
            if (!categoryId.HasValue && categories.Count > 0)
            {
                categoryId = categories.FirstOrDefault()?.Id;
            }
            
            var notes = _context.Notes.Include(n => n.Category).Where(n => n.UserId == userId);

            if (categoryId.HasValue)
            {
                notes = notes.Where(n => n.CategoryId == categoryId.Value);
            }

            Note? selectedNote = null;
            if (noteId.HasValue)
            {
                selectedNote = notes.FirstOrDefault(n => n.Id == noteId.Value);
            }

            ViewBag.Categories = categories;
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.SelectedNote = selectedNote;

            return View(notes.ToList());
        }

        // Get: Create
        public IActionResult Create()
        {
            PopulateCategories();
            return View();
        }

        // Post: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Note note)
        {
            if(ModelState.IsValid)
            {
                note.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                note.CreatedAt = DateTime.Now;
                _context.Add(note);
                await _context.SaveChangesAsync();
                TempData["Success"] = _localizer["NoteCreatedSuccess"].Value;
                return RedirectToAction(nameof(Index));
            }
            PopulateCategories();
            return View(note);
        }

        // Get: Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var note = await _context.Notes.
                FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);
            
            if (note == null) return NotFound();
            PopulateCategories();
            return View(note);
        }

        // Post: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Note note)
        {
            if(id != note.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var existingNote = await _context.Notes
                        .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);
                    
                    if(existingNote == null)
                        return NotFound();
                    
                    existingNote.Title = note.Title;
                    existingNote.Context = note.Context;
                    existingNote.CategoryId = note.CategoryId;
                    
                    await _context.SaveChangesAsync();
                    TempData["Success"] = _localizer["NoteEditedSuccess"].Value;
                    return RedirectToAction(nameof(Index));
                }
                catch(DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "An error occurred during the update.");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = _localizer["NoteEditedError"].Value + ex.Message;
                }
            }
            PopulateCategories();
            return View(note);
        }

        // Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var note = await _context.Notes
                .Include(n => n.Category)
                .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

            if(note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // Toggle Favorite
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleFavorite(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var note = await _context.Notes
                .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

            if (note == null)
            {
                return NotFound();
            }

            note.IsFavorite = !note.IsFavorite;
            await _context.SaveChangesAsync();

            TempData["Success"] = note.IsFavorite ? _localizer["NoteFavoriteAdded"].Value : _localizer["NoteFavoriteRemoved"].Value;

            return RedirectToAction(nameof(Index), new { noteId = id });
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);
                
                if(note == null)
                {
                    return NotFound();
                }
                
                _context.Notes.Remove(note);
                await _context.SaveChangesAsync();
                TempData["Success"] = _localizer["NoteDeletedSuccess"].Value;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = _localizer["NoteDeletedError"].Value + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        private void PopulateCategories()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.Categories = _context.Categories
                .Where(c => c.UserId == userId)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToList();
        }
    }
}
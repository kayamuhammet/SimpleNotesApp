using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SimpleNotesApp.Data;
using SimpleNotesApp.Models;

namespace SimpleNotesApp.Controllers
{
    public class NotesController : Controller
    {
        private readonly AppDbContext _context;

        public NotesController(AppDbContext context)
        {
            _context = context;
        }

        private void PopulateCategories()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
        }

        // Get: Notes
        public IActionResult Index(int? categoryId, int? noteId)
        {
            var categories = _context.Categories.ToList();
            
            // If no category is selected, select the first category
            if (!categoryId.HasValue && categories.Count > 0)
            {
                categoryId = categories.FirstOrDefault()?.Id;
            }
            
            var notes = _context.Notes.Include(n => n.Category).AsQueryable();

            if (categoryId.HasValue)
            {
                notes = notes.Where(n => n.CategoryId == categoryId.Value);
            }

            Note? selectedNote = null;
            if (noteId.HasValue)
            {
                selectedNote = _context.Notes.FirstOrDefault(n => n.Id == noteId.Value);
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
                try
                {
                    _context.Add(note);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Not başarıyla oluşturuldu.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Not oluşturulurken bir hata oluştu: " + ex.Message);
                }
            }
            PopulateCategories();
            return View(note);
        }

        // Get: Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var note = await _context.Notes.FindAsync(id);
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
                    _context.Update(note);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Not başarıyla güncellendi.";
                    return RedirectToAction("Index");
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!_context.Notes.Any(i => i.Id == id))
                    {
                        return NotFound();
                    }
                    else
                        throw;
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Not güncellenirken bir hata oluştu: " + ex.Message;
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
            var note = await _context.Notes
                .Include(n => n.Category)
                .FirstOrDefaultAsync(i => i.Id == id);

            if(note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var note = await _context.Notes.FindAsync(id);
                if(note != null)
                {
                    _context.Notes.Remove(note);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Not başarıyla silindi.";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Not silinirken bir hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
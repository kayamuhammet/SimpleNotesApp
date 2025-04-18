using Microsoft.AspNetCore.Mvc;
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

        // Get: Notes
        public async Task<IActionResult> Index()
        {
            var notes = await _context.Notes.OrderByDescending(n => n.CreatedAt).ToListAsync();
            return View(notes);
        }

        // Get: Create
        public IActionResult Create()
        {
            return View();
        }

        // Post: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Note note)
        {
            if(ModelState.IsValid)
            {
                _context.Add(note);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(note);
        }

        // Get: Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var note = await _context.Notes.FindAsync(id);
            if (note == null) return NotFound();

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
                return RedirectToAction("Index");
            }
            return View(note);
        }

        // Delete

        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var note = await _context.Notes.FirstOrDefaultAsync(i => i.Id == id);

            if(note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if(note != null)
            {
                _context.Notes.Remove(note);

                 await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View("Index");
            
        }
    }
}
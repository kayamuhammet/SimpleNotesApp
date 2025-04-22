using Microsoft.AspNetCore.Mvc;
using SimpleNotesApp.Data;
using SimpleNotesApp.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace SimpleNotesApp.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Categories.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category model)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }


        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    _context.SaveChanges();
                    TempData["Success"] = "Kategori başarıyla güncellendi.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _context.Categories
                .FirstOrDefault(m => m.Id == id);
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
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                try
                {

                    var unassignedCategory = _context.Categories.FirstOrDefault(c => c.Name == "Kategorisiz");
                    if (unassignedCategory == null)
                    {

                        unassignedCategory = new Category { Name = "Kategorisiz" };
                        _context.Categories.Add(unassignedCategory);
                        _context.SaveChanges();
                    }


                    var relatedNotes = _context.Notes.Where(n => n.CategoryId == id).ToList();
                    foreach (var note in relatedNotes)
                    {
                        note.CategoryId = unassignedCategory.Id;
                        note.Category = unassignedCategory;
                    }
                    _context.UpdateRange(relatedNotes);
                    _context.SaveChanges();


                    _context.Categories.Remove(category);
                    _context.SaveChanges();

                    TempData["Success"] = "Kategori başarıyla silindi.";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Kategori silinirken bir hata oluştu: " + ex.Message;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
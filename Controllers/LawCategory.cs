using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DRS;
using DRS.Models;
using Microsoft.AspNetCore.Authorization;

namespace DRS.Controllers
{
    [Authorize]
    public class LawCategoryController : Controller
    {
        private readonly DRSdbContext _context;

        public LawCategoryController(DRSdbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var lawCategories = await _context.LawCategories.ToListAsync();
            var commonModel = new CommonModel(){
                lawCategories = lawCategories
            };
            return View(commonModel);
        }

        // GET: LawCategory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lawCategory = await _context.LawCategories
                .FirstOrDefaultAsync(m => m.LawCategoryId == id);
            if (lawCategory == null)
            {
                return NotFound();
            }

            return View(lawCategory);
        }

        // GET: LawCategory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LawCategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LawCategoryId,LawCategoryName")] LawCategory lawCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lawCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lawCategory);
        }

        public async Task<IActionResult> Createfromdocument([Bind("LawCategoryId,LawCategoryName")] LawCategory lawCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lawCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        // GET: LawCategory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lawCategory = await _context.LawCategories.FindAsync(id);
            if (lawCategory == null)
            {
                return NotFound();
            }
            return View(lawCategory);
        }

        // POST: LawCategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LawCategoryId,LawCategoryName")] LawCategory lawCategory)
        {
            if (id != lawCategory.LawCategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lawCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LawCategoryExists(lawCategory.LawCategoryId))
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
            return View(lawCategory);
        }

        // GET: LawCategory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lawCategory = await _context.LawCategories
                .FirstOrDefaultAsync(m => m.LawCategoryId == id);
            if (lawCategory == null)
            {
                return NotFound();
            }

            return View(lawCategory);
        }

        // POST: LawCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lawCategory = await _context.LawCategories.FindAsync(id);
            if (lawCategory != null)
            {
                _context.LawCategories.Remove(lawCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LawCategoryExists(int id)
        {
            return _context.LawCategories.Any(e => e.LawCategoryId == id);
        }
    }
}

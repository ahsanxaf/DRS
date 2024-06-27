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
    public class MinistryController : Controller
    {
        private readonly DRSdbContext _context;

        public MinistryController(DRSdbContext context)
        {
            _context = context;
        }

        // GET: Ministry
        public async Task<IActionResult> Index()
        {
            return View(await _context.Ministries.ToListAsync());
        }

        // GET: Ministry/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ministry = await _context.Ministries
                .FirstOrDefaultAsync(m => m.MinistryId == id);
            if (ministry == null)
            {
                return NotFound();
            }

            return View(ministry);
        }

        // GET: Ministry/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ministry/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MinistryId,MinistryName")] Ministry ministry)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ministry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ministry);
        }

        // GET: Ministry/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ministry = await _context.Ministries.FindAsync(id);
            if (ministry == null)
            {
                return NotFound();
            }
            return View(ministry);
        }

        // POST: Ministry/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MinistryId,MinistryName")] Ministry ministry)
        {
            if (id != ministry.MinistryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ministry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MinistryExists(ministry.MinistryId))
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
            return View(ministry);
        }

        // GET: Ministry/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ministry = await _context.Ministries
                .FirstOrDefaultAsync(m => m.MinistryId == id);
            if (ministry == null)
            {
                return NotFound();
            }

            return View(ministry);
        }

        // POST: Ministry/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ministry = await _context.Ministries.FindAsync(id);
            if (ministry != null)
            {
                _context.Ministries.Remove(ministry);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MinistryExists(int id)
        {
            return _context.Ministries.Any(e => e.MinistryId == id);
        }
    }
}
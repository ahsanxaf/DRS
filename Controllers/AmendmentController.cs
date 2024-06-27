using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DRS.Models;
using Microsoft.AspNetCore.Authorization;

namespace DRS.Controllers
{
    [Authorize]
    public class AmendmentController : Controller
    {
        private readonly DRSdbContext _context;

        public AmendmentController(DRSdbContext context)
        {
            _context = context;
        }

        // GET: Amendment
        public async Task<IActionResult> Index(int id)
        {
            // Ensure id is valid (you might want to handle this differently based on your application logic)
            if (id != 0)
            {
                ViewBag.DocId = id; // Store DocId in ViewBag for display
                ViewData["DocId"] = id; // Optional: Also store in ViewData if needed
            }
            var viewModel = new AmendmentViewModel
            {
                NewAmendment = new Amendment(), // Initialize with a new amendment instance
                AmendmentsHistory = _context.Amendments.ToList() // Fetch the amendments history
            };
            // Fetch all versions of the document from the database



            return View(viewModel); // Pass the versions to the view
        }

        // GET: Amendment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var amendment = await _context.Amendments
                .FirstOrDefaultAsync(m => m.AmendId == id);
            if (amendment == null)
            {
                return NotFound();
            }

            return View(amendment);
        }
        public IActionResult GetAmendmentsByVersionId(int versionId)
        {
            // Assuming _context is your DbContext and Amendments is DbSet<Amendment>
            var amendments = _context.Amendments.Where(a => a.DocId == versionId).ToList();

            return Json(amendments); // Return amendments as JSON
        }
        // GET: Amendment/GetVersionsByDocId?docId=5
        public async Task<IActionResult> GetVersionsByDocId(int docId)
        {
            var versions = await _context.Versions
                .Where(v => v.DocId == docId)
                .ToListAsync();

            return Json(versions);
        }

        // GET: Amendment/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Amendment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AmendId,DocId,VersionNo,Sections,SNumber,AmendmentType,BookNo,SubSection,Clause,SubClause,Entry,AmendedContent,UpdatedBy,UpdatedTime")] Amendment amendment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(amendment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { id = amendment.DocId });
            }
            return View(amendment);
        }

        // GET: Amendment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var amendment = await _context.Amendments.FindAsync(id);
            if (amendment == null)
            {
                return NotFound();
            }
            return View(amendment);
        }

        // POST: Amendment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("AmendId,DocId,VersionNo,Sections,SNumber,AmendmentType,BookNo,SubSection,Clause,SubClause,Entry,AmendedContent,UpdatedBy,UpdatedTime")] Amendment amendment)
        {
            if (id != amendment.AmendId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(amendment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AmendmentExists(amendment.AmendId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { id = amendment.DocId });
            }
            return View(amendment);
        }

        // GET: Amendment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var amendment = await _context.Amendments
                .FirstOrDefaultAsync(m => m.AmendId == id);
            if (amendment == null)
            {
                return NotFound();
            }

            return View(amendment);
        }

        // POST: Amendment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var amendment = await _context.Amendments.FindAsync(id);
            if (amendment != null)
            {
                var docId = amendment.DocId; // Store the DocId before removing the amendment
                _context.Amendments.Remove(amendment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { id = docId });
            }

            return NotFound();
        }


        private bool AmendmentExists(int? id)
        {
            return _context.Amendments.Any(e => e.AmendId == id);
        }
    }
}
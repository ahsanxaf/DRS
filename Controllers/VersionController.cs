using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DRS;
using DRS.Models;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Hosting;
using System.Reflection.Metadata;
using System.Xml.Linq;
using NuGet.ProjectModel;
using Microsoft.AspNetCore.Authorization;

namespace DRS.Controllers
{
    [Authorize]
    public class VersionController(DRSdbContext context, IWebHostEnvironment webHostEnvironment) : Controller
    {
        private readonly DRSdbContext _context = context;


        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        // GET: Version
        public async Task<IActionResult> Index()
        {
            return View(await _context.Versions.ToListAsync());
        }
        [HttpPost]


        // GET: Version/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var version = await _context.Versions
                .FirstOrDefaultAsync(m => m.VersionId == id);
            if (version == null)
            {
                return NotFound();
            }

            return View(version);
        }

        // GET: Version/Create
        public IActionResult Create(int docid)
        {
            ViewBag.DocId = docid;
            return View();
        }
        public IActionResult GetMinistries()
        {
            var ministries = _context.Ministries
                .Where(m => _context.Documents.Any(d => d.DdlMinistries != null && d.DdlMinistries == m.MinistryName))
                .Select(m => m.MinistryName)
                .Distinct()
                .OrderBy(m => m) // Order the results alphabetically
                .ToList();

            return Json(ministries);
        }

        public IActionResult GetLawCategories()
        {
            var lawCategories = _context.LawCategories
                .Where(lc => _context.Documents.Any(d => d.DdlLawcategory != null && d.DdlLawcategory == lc.LawCategoryName))
                .Select(lc => lc.LawCategoryName)
                .Distinct()
                .OrderBy(lc => lc) // Order the results alphabetically
                .ToList();

            return Json(lawCategories);
        }



        public IActionResult GetVersionNumbers()
        {
            var versionNumbers = _context.Versions
                .Where(v => !string.IsNullOrWhiteSpace(v.Vnumber)) // Filter out null or empty values
                .Select(v => v.Vnumber.Trim()) // Trim leading and trailing spaces
                .Where(IsValidRomanNumeral) // Filter out invalid Roman numeral strings
                .Distinct()
                .OrderBy(v => RomanToInt(v)) // Order by the integer value of Roman numerals
                .ToList();

            return Json(versionNumbers);
        }

        private bool IsValidRomanNumeral(string s)
        {
            Dictionary<char, int> romanMap = new Dictionary<char, int>
            {
                {'I', 1}, {'V', 5}, {'X', 10}, {'L', 50}, {'C', 100}, {'D', 500}, {'M', 1000}
            };

            foreach (char c in s)
            {
                if (!romanMap.ContainsKey(c))
                {
                    return false;
                }
            }

            return true;
        }

        private int RomanToInt(string s)
        {
            Dictionary<char, int> romanMap = new Dictionary<char, int>
            {
                {'I', 1}, {'V', 5}, {'X', 10}, {'L', 50}, {'C', 100}, {'D', 500}, {'M', 1000}
            };

            int number = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (i + 1 < s.Length && romanMap[s[i]] < romanMap[s[i + 1]])
                {
                    number -= romanMap[s[i]];
                }
                else
                {
                    number += romanMap[s[i]];
                }
            }
            return number;
        }

        public IActionResult GetVersionYears()
        {
            var versionYears = _context.Versions
                .Where(v => v.VgazetteYear != null)
                .Select(v => v.VgazetteYear)
                .Distinct()
                .ToList();

            return Json(versionYears);
        }


        public IActionResult GetVersionData(int? versionId)
        {
            if (versionId == null)
            {
                // Handle the case where versionId is not provided in the request
                return BadRequest("Version ID is required.");
            }

            // Fetch version data from the database based on versionId
            var version = _context.Versions.FirstOrDefault(v => v.VersionId == versionId);

            if (version == null)
            {
                // Version not found, handle appropriately (e.g., return an error view)
                return NotFound();
            }

            // Assuming you want to return JSON data
            return Json(version);
        }




        // POST: Version/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("AddPermission", "DocumentModule")]

        public async Task<IActionResult> Create(int docid, IFormFile docFile, [Bind("VersionId,DocId,VersionNo,Vyear,Vnumber,VgazetteYear,VgazettePart,VgazettePage,DocumentName,DocUrl,CommencementDate,PublishingDate,ValidityDate,VdocNature,Vstatus,Remarks,UploadDate")] Models.Version version)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the maximum version number for the given document using LINQ to Objects
                var versions = await _context.Versions.Where(v => v.DocId == docid).ToListAsync();

                int? maxVersionNo = (int?)(versions.Count > 0 ? versions.Max(v => v.VersionNo) : 0);

                // Increment the version number
                version.VersionNo = maxVersionNo + 1;

                if (docFile != null && docFile.Length > 0)
                {

                    // Save the uploaded file
                    var documentFileName = Path.GetFileName(docFile.FileName);
                    var documentFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "UploadedDocuments", documentFileName);

                    using (var fileStream = new FileStream(documentFilePath, FileMode.Create))
                    {
                        await docFile.CopyToAsync(fileStream);
                    }

                    // Set the DocUrl property of the version object
                    version.DocUrl = "/UploadedDocuments/" + documentFileName;
                }

                version.UploadDate = DateTime.Now;
                version.DocId = docid;
                _context.Add(version);

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Document");
            }
            return View(version);
        }

        // GET: Version/Edit/5
        [CustomAuthorize("AddPermission", "DocumentModule")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var version = await _context.Versions.FindAsync(id);
            if (version == null)
            {
                return NotFound();
            }
            return View(version);
        }

        // POST: Version/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("AddPermission", "DocumentModule")]
        public async Task<IActionResult> Edit(int id, IFormFile? docFile, [Bind("VersionId,DocId,VersionNo,Vyear,Vnumber,VgazetteYear,VgazettePart,VgazettePage,DocumentName,DocUrl,CommencementDate,PublishingDate,ValidityDate,VdocNature,Vstatus,Remarks,UploadDate")] Models.Version version)
        {
            if (id != version.VersionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var previousVersion = await _context.Versions.AsNoTracking().FirstOrDefaultAsync(v => v.VersionId == id);
                if (previousVersion == null)
                {
                    return NotFound();
                }

                if (docFile != null && docFile.Length > 0)
                {
                    // Define the directory path for uploaded documents
                    var uploadDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "UploadedDocuments");

                    // Define the file path for the new file
                    var newFilePath = Path.Combine(uploadDirectory, docFile.FileName);

                    // Define the file path for the old file
                    var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, previousVersion.DocUrl.TrimStart('/'));

                    // Check if the old file exists and rename it
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        var oldFileExtension = Path.GetExtension(oldFilePath);
                        var oldFileWithoutExtension = Path.GetFileNameWithoutExtension(oldFilePath);
                        var renamedFilePath = Path.Combine(uploadDirectory, oldFileWithoutExtension + "_old" + oldFileExtension);
                        System.IO.File.Move(oldFilePath, renamedFilePath);
                    }

                    // Save the new file
                    using (var fileStream = new FileStream(newFilePath, FileMode.Create))
                    {
                        await docFile.CopyToAsync(fileStream);
                    }

                    // Set the DocUrl property of the version object
                    version.DocUrl = "/UploadedDocuments/" + docFile.FileName;
                }
                else
                {
                    // Preserve the old DocUrl if no new file is uploaded
                    version.DocUrl = previousVersion.DocUrl;
                }

                try
                {
                    _context.Update(version);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VersionExists(version.VersionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Index", "DocumentsVersion");
            }
            return View(version);
        }


        // GET: Version/Delete/5
        [CustomAuthorize("DeletePermission", "DocumentModule")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var version = await _context.Versions
                .FirstOrDefaultAsync(m => m.VersionId == id);
            if (version == null)
            {
                return NotFound();
            }

            return View(version);
        }

        public async Task<IActionResult> VersionHistory(int? Id = null, int? versionId = null)
        {
            if (Id == null && versionId == null)
            {
                return NotFound();
            }

            List<DRS.Models.Version> versionsList = new List<DRS.Models.Version>();

            if (Id != null)
            {

                var documentWithVersions = await _context.Documents
                    .Include(d => d.Versions)
                    .FirstOrDefaultAsync(m => m.DocId == Id);

                if (documentWithVersions == null)
                {
                    return NotFound();
                }



                ViewBag.LatestUrl = documentWithVersions.LatestUrl;
                ViewBag.DocumentTitle = documentWithVersions.DocTitle;

                var document = _context.DocumentTypes.FirstOrDefault(d => d.DocId == Id);


                var ordinanceDocId = document?.Ordinance;
                var rulesDocId = document?.Rules;

                var ordinanceDocuments = _context.DocumentTypes
                    .Where(d => d.SubcategoryId == 2 && (d.DocId == ordinanceDocId || Id == d.Acts))
                    .Select(d => new
                    {
                        d.DocId,
                        d.DocTitle
                    })
                    .ToList();



                var rulesDocuments = _context.DocumentTypes
                        .Where(d => d.SubcategoryId == 7 && (d.DocId == rulesDocId || Id == d.Acts))
                        .Select(d => new
                        {
                            d.DocId,
                            d.DocTitle
                        })
                        .ToList();

                ViewBag.HasOrdinanceDocuments = ordinanceDocuments.Any();
                ViewBag.OrdinanceDocuments = new SelectList(ordinanceDocuments, "DocId", "DocTitle");
                ViewBag.HasRulesDocuments = rulesDocuments.Any();
                ViewBag.RulesDocuments = new SelectList(rulesDocuments, "DocId", "DocTitle");

                // Retrieve all versions for the document
                versionsList = documentWithVersions.Versions.ToList();

                // Set the versionId to the first version's ID if versions are available
                if (versionsList.Count > 0)
                {
                    versionId = versionsList.First().VersionId;
                }
            }
            else if (versionId != null)
            {
                var version = await _context.Versions
                    .Include(v => v.Document)
                    .FirstOrDefaultAsync(v => v.VersionId == versionId);

                if (version == null)
                {
                    return NotFound();
                }

                ViewBag.LatestUrl = version.Document.LatestUrl;
                ViewBag.DocumentTitle = version.Document.DocTitle;

                var documentType = _context.DocumentTypes.FirstOrDefault(d => d.DocId == version.DocId);
                if (documentType != null)
                {
                    var ordinanceDocuments = _context.DocumentTypes
                        .Where(d => d.SubcategoryId == 2 && (d.DocId == documentType.Ordinance || version.DocId == d.Acts))
                        .Select(d => new { d.DocId, d.DocTitle })
                        .ToList();

                    var rulesDocuments = _context.DocumentTypes
                        .Where(d => d.SubcategoryId == 7 && (d.DocId == documentType.Rules || version.DocId == d.Acts))
                        .Select(d => new { d.DocId, d.DocTitle })
                        .ToList();

                    ViewBag.HasOrdinanceDocuments = ordinanceDocuments?.Any();
                    ViewBag.OrdinanceDocuments = new SelectList(ordinanceDocuments, "DocId", "DocTitle");
                    ViewBag.HasRulesDocuments = rulesDocuments?.Any();
                    ViewBag.RulesDocuments = new SelectList(rulesDocuments, "DocId", "DocTitle");
                }

                versionsList = await _context.Versions
                    .Where(v => v.DocId == version.DocId)
                    .ToListAsync();
            }


            ViewBag.VersionId = versionId;
            ViewBag.DocId = Id;
            return View("VersionHistory", versionsList);
        }



        // POST: Version/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("DeletePermission", "DocumentModule")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var version = await _context.Versions.FindAsync(id);
            if (version != null)
            {
                _context.Versions.Remove(version);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "DocumentsVersion");
        }

        private bool VersionExists(int id)
        {
            return _context.Versions.Any(e => e.VersionId == id);
        }
    }
}

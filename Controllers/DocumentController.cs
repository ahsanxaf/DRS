using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DRS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using DRS.Services;


namespace DRS.Controllers
{
    [Authorize]
    public class DocumentController : Controller
    {
        private readonly DRSdbContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly ILogService _logService;

        public DocumentController(DRSdbContext context, IWebHostEnvironment webHostEnvironment, ILogService logService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            /*_userManager = userManager;*/
            _logService = logService;
        }

        public async Task<IActionResult> Search()
        {
            // Retrieve all documents along with their associated versions
            var documentsWithVersions = await _context.Documents
                 .Include(d => d.Versions) // Include the related versions
                 .ToListAsync();

            return View(documentsWithVersions);
        }
        // GET: Document
        public async Task<IActionResult> Index()
        {
            var dRSdbContext = _context.Documents.Include(d => d.Category).Include(d => d.Subcategory);

            return View(await dRSdbContext.ToListAsync());
        }

        public IActionResult GetDocId(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var document = _context.Documents.FirstOrDefault(d => d.DocId == id);


            if (document == null)
            {
                return NotFound();
            }

            return Json(document);
        }

        [HttpPost]
        [CustomAuthorize("AddPermission", "DocumentModule")]
        public IActionResult AddLatestUrl(int documentId, string latestUrl)
        {
            // Find the document with the specified ID
            var document = _context.Documents.Find(documentId);

            if (document != null)
            {
                // Update the LatestUrl property of the document
                document.LatestUrl = latestUrl;
                _context.SaveChanges();
                return Json(new { success = true });
            }
            else
            {
                // Document not found, return an error response
                return Json(new { success = false, error = "Document not found" });
            }
        }

        // GET: Document/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .Include(d => d.Category)
                .Include(d => d.Subcategory)
                .FirstOrDefaultAsync(m => m.DocId == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        [HttpGet]
        public async Task<IActionResult> Download(int id)
        {
            var version = await _context.Versions
                                        .Where(v => v.DocId == id && v.VersionNo == 1)
                                        .FirstOrDefaultAsync();

            if (version != null)
            {
                var filePath = _webHostEnvironment.WebRootPath + version.DocUrl;
                var fileName = Path.GetFileName(filePath);
                var mimeType = "application/octet-stream";

                return PhysicalFile(filePath, mimeType, fileName);
            }

            return NotFound();
        }
        public ActionResult LawCategory()
        {

            return PartialView("_LawCategoryPartial");
        }
        // GET: Document/Create
        public IActionResult Create()
        {
            PopulateDropdowns();


            return View();
        }

        public Task<IActionResult> UploadAmendment(int? id)
        {
            if (id == null)
            {
                return Task.FromResult<IActionResult>(BadRequest("Document ID is not provided."));
            }

            // Redirect to the Create action of VersionController with the DocId parameter
            return Task.FromResult<IActionResult>(RedirectToAction("Create", "Version", new { docId = id }));
        }



        // POST: Document/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("AddPermission", "DocumentModule")]
        public async Task<IActionResult> Create([Bind("DocId,CategoryId,SubcategoryId,DocTitle,DdlMinistries,DdlLawcategory,PageNo,Volume,Year,RepealedBy,AmdtPrinciple,BookNoNew,BookNoOld,BookPageNo,GazetteYear,GazettePart,GazettePageNo,Srono,SubDepartment,Status,Remarks,CreatedDate,PublishingDate,CommencementDate,SecurityLevel,TargetSubCategory,TargetSearchDocument,TitleUrl,LatestUrl,Rules,Acts,Ordinance")] Document document, IFormFile documentFile, DocumentType type)
        {
            if (ModelState.IsValid)
            {
                if (documentFile != null && documentFile.Length > 0)
                {
                    // Generate a unique file name to prevent overwriting existing files
                    var documentFileName = Path.GetFileName(documentFile.FileName);
                    var documentFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "UploadedDocuments", documentFileName);

                    // Save the document file to the uploads/documents folder
                    using (var stream = new FileStream(documentFilePath, FileMode.Create))
                    {
                        await documentFile.CopyToAsync(stream);
                    }

                    // Set the document file path in the document object
                    document.DocUrl = "/UploadedDocuments/" + documentFileName; // Assuming uploads folder is in wwwroot
                }
                // Create and add the document to the context


                _context.Add(document);

                await _context.SaveChangesAsync();
                var documenttype = new DocumentType
                {
                    DocId = document.DocId,
                    DocTitle = document.DocTitle,
                    Acts = type.Acts,
                    Ordinance = type.Ordinance,
                    Rules = type.Rules,
                    SubcategoryId = document.SubcategoryId,

                };
                _context.Add(documenttype);
                await _context.SaveChangesAsync();
                // Create a version for the newly created document


                var version = new Models.Version
                {
                    DocId = document.DocId,
                    DocumentName = document.DocTitle,
                    DocUrl = document.DocUrl,
                    VdocNature = document.AmdtPrinciple,
                    VersionNo = 1,
                    UploadDate = DateTime.Now,
                    VgazetteYear = document.GazetteYear,
                    Vnumber = document.Srono,
                    VgazettePage = document.GazettePageNo,
                    VgazettePart = document.GazettePart,
                    CommencementDate = document.CommencementDate,
                    PublishingDate = document.PublishingDate,
                    ValidityDate = document.Validity,
                    Vstatus = document.Status,
                    Remarks = document.Remarks,
                };

                _context.Add(version);
                await _context.SaveChangesAsync();

                var userId = HttpContext.Session.GetString("UserId");

                /*var user = await _userManager.GetUserAsync(User);*/
                await _logService.LogAsync(56, "Add", document.DocId);


                return RedirectToAction(nameof(Index));
            }

            // Handle ModelState.IsValid == false
            ViewData["CategoryId"] = new SelectList(_context.DocumentCategories.Select(c => new { c.CategoryId, c.CategoryName }), "CategoryId", "CategoryName", document.CategoryId);
            ViewData["SubcategoryId"] = new SelectList(_context.Subcategories.Select(s => new { s.SubcategoryId, s.SubcategoryName }), "SubcategoryId", "SubcategoryName", document.SubcategoryId);
            ViewData["Ministries"] = new SelectList(_context.Ministries, "MinistryId", "MinistryName");
            ViewData["LawCategories"] = new SelectList(_context.LawCategories, "LawCategoryId", "LawCategoryName");
            return View(document);
        }
        public class DocumentViewModel
        {
            public int DocId { get; set; }
            public string DocTitle { get; set; }
        }


        // GET: Document/Edit/5
        [CustomAuthorize("AddPermission", "DocumentModule")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            var documentType = await _context.DocumentTypes.FirstOrDefaultAsync(dt => dt.DocId == document.DocId);
            if (documentType == null)
            {
                // Handle the case where the DocumentType entry doesn't exist
                documentType = new DocumentType
                {
                    DocId = document.DocId
                };
            }

            var viewModel = new DocumentVersion
            {
                Document = document,
                type = documentType
            };

            PopulateDropdowns();
            return View(viewModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("AddPermission", "DocumentModule")]
        public async Task<IActionResult> Edit(int id, IFormFile? docFile, [Bind("DocId,CategoryId,SubcategoryId,DocTitle,DdlMinistries,DdlLawcategory,Volume,VolumePageNo,Year,RepealedBy,AmdtPrinciple,BookNoNew,BookNoOld,BookPageNo,GazetteYear,GazettePart,GazettePageNo,Srono,SubDepartment,Status,Remarks,CreatedDate,PublishingDate,CommencementDate,SecurityLevel,TargetSubCategory,TargetSearchDocument,TitleUrl,LatestUrl,Rules,Acts,Ordinance,DocUrl")] Document document)
        {
            if (id != document.DocId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Fetch the associated DocumentType
                var documentType = await _context.DocumentTypes.FirstOrDefaultAsync(dt => dt.DocId == id);

                // Fetch the associated version with VersionNo = 1
                var version = await _context.Versions
                    .FirstOrDefaultAsync(v => v.DocId == id && v.VersionNo == 1);

                if (version != null)
                {
                    // Fetch previous version details for comparison
                    var previousVersion = await _context.Versions.AsNoTracking().FirstOrDefaultAsync(v => v.DocId == id && v.VersionNo == 1);

                    if (docFile != null && docFile.Length > 0)
                    {
                        // Define the directory path for uploaded documents
                        var uploadDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "UploadedDocuments");

                        // Ensure the directory exists
                        if (!Directory.Exists(uploadDirectory))
                        {
                            Directory.CreateDirectory(uploadDirectory);
                        }

                        // Define the file path for the new file
                        var newFilePath = Path.Combine(uploadDirectory, docFile.FileName);

                        // Define the file path for the old file
                        var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, version.DocUrl?.TrimStart('/'));

                        // If the old file exists, rename it (not mandatory but good practice)
                        if (oldFilePath != null && System.IO.File.Exists(oldFilePath))
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

                    // Update the version details
                    version.DocumentName = document.DocTitle;
                    version.VdocNature = document.AmdtPrinciple;
                    version.Vyear = document.Year;
                    version.Vnumber = document.Srono;
                    version.VgazettePart = document.GazettePart;
                    version.CommencementDate = document.CommencementDate;
                    version.PublishingDate = document.PublishingDate;
                    version.ValidityDate = document.Validity;
                    version.Vstatus = document.Status;
                    version.Remarks = document.Remarks;

                    _context.Update(version);
                }

                // Update the document details
                _context.Update(document);

                // Update the document type details
                if (documentType != null)
                {
                    documentType.Rules = document.Rules;
                    documentType.Acts = document.Acts;
                    documentType.Ordinance = document.Ordinance;
                    _context.Update(documentType);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns();
            ViewData["CategoryId"] = new SelectList(_context.DocumentCategories.Select(c => new { c.CategoryId, c.CategoryName }), "CategoryId", "CategoryName", document.CategoryId);
            ViewData["SubcategoryId"] = new SelectList(_context.Subcategories.Select(s => new { s.SubcategoryId, s.SubcategoryName }), "SubcategoryId", "SubcategoryName", document.SubcategoryId);
            return View(document);
        }


        // GET: Document/Delete/5
        [CustomAuthorize("DeletePermission", "DocumentModule")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .Include(d => d.Category)
                .Include(d => d.Subcategory)
                .FirstOrDefaultAsync(m => m.DocId == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Document/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("DeletePermission", "DocumentModule")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            var documenttype = await _context.DocumentTypes
                                             .FirstOrDefaultAsync(dt => dt.DocId == id);

            if (document != null && documenttype != null)
            {
                _context.DocumentTypes.Remove(documenttype);
                _context.Documents.Remove(document);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool DocumentExists(int id)
        {
            return _context.Documents.Any(e => e.DocId == id);
        }

        [HttpGet]
        public IActionResult GetSubcategoriesByCategoryId(int categoryId)
        {
            var subcategories = _context.Subcategories
                .Where(s => s.CategoryId == categoryId)
                .Select(s => new { s.SubcategoryId, s.SubcategoryName })
                .ToList();

            return Json(subcategories);
        }


        public IActionResult GetActCountByYearJson()
        {
            var data = _context.Documents
                .Where(d => d.CommencementDate != null) // Filter out documents with null CreatedDate
                .GroupBy(d => d.CommencementDate.Value.Year) // Group documents by year
                .Select(g => new
                {
                    Label = g.Key.ToString(), // Convert year to string for label
                    Count = g.Count() // Count number of documents for each year
                })
                .OrderByDescending(item => item.Count) // Order by count in descending order
                .Take(10) // Take only the top 10 items
                .ToList();

            return Json(data);
        }


        public IActionResult GetActCountByMinistriesJson()
        {
            var data = _context.Ministries
                .Where(lc => _context.Documents.Any(d => d.DdlMinistries != null && d.DdlMinistries == lc.MinistryName))
                .Select(lc => new
                {
                    Label = lc.MinistryName,
                    Count = _context.Documents.Count(d => d.DdlMinistries != null && d.DdlMinistries == lc.MinistryName)
                })
                .OrderByDescending(lc => lc.Count)  // Order by count in descending order
                .Take(10)  // Take the top 10
                .ToList();

            return Json(data);
        }
        public IActionResult GetAmendmentCountByYearJson()
        {
            var data = _context.Versions
                .Where(d => d.CommencementDate != null && d.VersionNo != 1) // Exclude VersionNo = 1
                .GroupBy(d => d.CommencementDate.Value.Year) // Group documents by year
                .Select(g => new
                {
                    Label = g.Key.ToString(), // Convert year to string for label
                    Count = g.Count() // Count number of documents for each year
                })
                .OrderByDescending(item => item.Count) // Order by count in descending order
                .Take(10) // Take only the top 10 items
                .ToList();

            return Json(data);
        }


        public IActionResult GetAmendmentCountByMinistriesJson()
        {
            var data = _context.Ministries
    .Select(lc => new
    {
        Label = lc.MinistryName,
        Count = _context.Versions
            .Count(v => v.Document.DdlMinistries == lc.MinistryName && v.VersionNo != 1) // Exclude VersionNo = 1
    })
    .OrderByDescending(lc => lc.Count)
    .Take(10)
    .ToList();


            return Json(data);
        }


        public IActionResult GetActCountByLawCategoryJson()
        {
            var data = _context.LawCategories
                .Where(lc => _context.Documents.Any(d => d.DdlLawcategory != null && d.DdlLawcategory.ToString() == lc.LawCategoryName))
                .Select(lc => new
                {
                    Label = lc.LawCategoryName,
                    Count = _context.Documents.Count(d => d.DdlLawcategory != null && d.DdlLawcategory.ToString() == lc.LawCategoryName)
                })
                .OrderByDescending(item => item.Count) // Order by count in descending order
                .Take(10) // Take only the top 10 items
                .ToList();

            return Json(data);
        }

        



        private void PopulateDropdowns()
        {
            ViewBag.Acts = _context.Documents
                .Where(d => d.Subcategory.SubcategoryName == "Act")
                .Select(d => new SelectListItem
                {
                    Value = d.DocId.ToString(),
                    Text = d.DocTitle
                })
                .ToList();
            ViewBag.Rules = _context.Documents
                .Where(d => d.Subcategory.SubcategoryName == "Rules")
                .Select(d => new SelectListItem
                {
                    Value = d.DocId.ToString(),
                    Text = d.DocTitle
                })
                .ToList();
            ViewBag.Ordinances = _context.Documents
                .Where(d => d.Subcategory.SubcategoryName == "Ordinance")
                .Select(d => new SelectListItem
                {
                    Value = d.DocId.ToString(),
                    Text = d.DocTitle
                })
                .ToList();

            ViewData["CategoryId"] = new SelectList(_context.DocumentCategories, "CategoryId", "CategoryName");
            ViewData["SubcategoryId"] = new SelectList(_context.Subcategories, "SubcategoryId", "SubcategoryName");
            ViewData["Ministries"] = new SelectList(_context.Ministries, "MinistryName", "MinistryName");
            ViewData["LawCategories"] = new SelectList(_context.LawCategories, "LawCategoryName", "LawCategoryName");
        }
    }

}
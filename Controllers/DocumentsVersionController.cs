using DRS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Linq;

namespace DRS.Controllers
{
    [Authorize]
    public class DocumentsVersionController : Controller
    {
        private readonly DRSdbContext _context;

        public DocumentsVersionController(DRSdbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            // Retrieve the list of documents with their associated versions
            var viewModel = (from d in _context.Documents
                             join lc in _context.LawCategories on d.DdlLawcategory.ToString() equals lc.LawCategoryName into dlc
                             from LawCategory in dlc.DefaultIfEmpty()
                             join m in _context.Ministries on d.DdlMinistries.ToString() equals m.MinistryName into dm
                             from Ministries in dm.DefaultIfEmpty()
                             join v in _context.Versions on d.DocId equals v.DocId
                             join s in _context.Subcategories on d.SubcategoryId equals s.SubcategoryId into ds
                             from subcategory in ds.DefaultIfEmpty()
                             orderby d.DocId, v.VersionNo ascending
                             select new DocumentVersion
                             {
                                 DocId = d.DocId,
                                 DocTitle = d.DocTitle,
                                 DocumentName = v.DocumentName,
                                 SubcategoryId = d.SubcategoryId,
                                 VersionNo = v.VersionNo,
                                 VersionId = v.VersionId,
                                 SubcategoryName = subcategory.SubcategoryName,
                                 Vyear = v.Vyear,
                                 Vnumber = v.Vnumber,

                                 DdlMinistries = Ministries != null ? Ministries.MinistryName.ToString() : string.Empty,
                                 DdlLawcategory = LawCategory != null ? LawCategory.LawCategoryName.ToString() : string.Empty,
                             }).ToList();

            // Pass the first version ID to the view
            int? firstVersionId = viewModel.FirstOrDefault()?.VersionId;
            ViewBag.VersionId = firstVersionId;
       
            return View(viewModel);
         
        }





    }
}
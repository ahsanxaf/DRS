using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.IO.Compression;

namespace DRS.Controllers
{
    [Authorize]
    public class BackupController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public BackupController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DRSdbContext");
        }

        [CustomAuthorize("BackupModule")]
        public IActionResult Index()
        {
            var backupFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "db_backup");
            if (!Directory.Exists(backupFolder))
            {
                Directory.CreateDirectory(backupFolder);
            }
            var backupFiles = Directory.GetFiles(backupFolder);
            var backupList = new List<BackupViewModel>();

            foreach (var backupFile in backupFiles)
            { 
                var fileName = Path.GetFileName(backupFile);
                var fileDate = ExtractDateFromFileName(fileName);
                backupList.Add(new BackupViewModel
                {
                    FileName = fileName,
                    DateCreated = fileDate
                });
            }

            return View(backupList);
        }

        [HttpPost]
        [CustomAuthorize("BackupModule")]
        public async Task<IActionResult> CreateBackup()
        {
            try
            {
                var backupFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "db_backup");
                if (!Directory.Exists(backupFolder))
                {
                    Directory.CreateDirectory(backupFolder);
                }

                var backupFileName = $"backup_{DateTime.Now:yyyyMMddHHmmss}.bak";
                var backupFilePath = Path.Combine(backupFolder, backupFileName);

                var connectionString = _configuration.GetConnectionString("DRSdbContext");
                using (var connection = new SqlConnection(connectionString))
                {
                    var query = $"BACKUP DATABASE [{connection.Database}] TO DISK = @backupFilePath";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@backupFilePath", backupFilePath);
                        connection.Open();
                        await command.ExecuteNonQueryAsync();
                    }
                }

                return Json(new { success = true, message = "Database backup created successfully.", backupFilePath });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        public IActionResult DownloadBackup(string fileName)
        {
            var backupFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "db_backup");
            var filePath = Path.Combine(backupFolder, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/octet-stream", fileName);
        }

        [HttpPost]
        public async Task<IActionResult> RestoreBackup(IFormFile backupFile)
        {
            if (backupFile == null || backupFile.Length == 0)
            {
                return Json(new { success = false, message = "Invalid backup file." });
            }
            var backupFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "db_backup");
            var backupFilePath = Path.Combine(backupFolder, backupFile.FileName);

            try
            {
                // Save the uploaded backup file to the backup folder
                using (var stream = new FileStream(backupFilePath, FileMode.Create))
                {
                    await backupFile.CopyToAsync(stream);
                }

               /* var builder = new SqlConnectionStringBuilder(_connectionString)
                {
                    InitialCatalog = "master", // Use the master database
                    ConnectTimeout = 600 // Set timeout to 10 minutes
                };*/

                // Restore the database
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    //var setSingleUserCommandText = "ALTER DATABASE [DRS] SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
                    var restoreCommandText = $"RESTORE DATABASE [DRS] FROM DISK = '{backupFilePath}' WITH REPLACE";
                    //var setMultiUserCommandText = "ALTER DATABASE [DRS] SET MULTI_USER";

                    /*using (var command = new SqlCommand(setSingleUserCommandText, connection))
                    {
                        command.CommandTimeout = 600;
                        command.ExecuteNonQuery();
                    }*/

                    using (var command = new SqlCommand(restoreCommandText, connection))
                    {
                        command.CommandTimeout = 600;
                        command.ExecuteNonQuery();
                    }

                    /*using (var command = new SqlCommand(setMultiUserCommandText, connection))
                    {
                        command.CommandTimeout = 600;
                        command.ExecuteNonQuery();
                    }*/
                }

                return Json(new { success = true, message = "Database restored successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error restoring database: {ex.Message}" });
            }
        }

        private DateTime? ExtractDateFromFileName(string fileName)
        {
            var dateTimeString = fileName.Replace("backup_", "").Replace(".bak", "");
            if (DateTime.TryParseExact(dateTimeString, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
            return null;
        }

        public IActionResult GetBackupFiles()
        {
            var backupFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "db_backup");
            var backupFiles = Directory.GetFiles(backupFolder)
                                       .Select(file => new BackupViewModel
                                       {
                                           FileName = Path.GetFileName(file),
                                           DateCreated = System.IO.File.GetCreationTime(file)
                                       })
                                       .ToList();

            return PartialView("_BackupTable", backupFiles);
        }

        private static int _backupProgress = 0;

        public IActionResult GetBackupProgress()
        {
            return Json(new { progress = _backupProgress });
        }

        [HttpPost]
        [CustomAuthorize("BackupModule")]
        public async Task<IActionResult> CreateDocumentsBackup()
        {
            var backupFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documents_backup");
            var documentsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedDocuments");

            if (!Directory.Exists(backupFolder))
            {
                Directory.CreateDirectory(backupFolder);
            }

            var backupFileName = $"DocumentsBackup_{DateTime.Now:yyyyMMddHHmmss}.zip";
            var backupFilePath = Path.Combine(backupFolder, backupFileName);

            try
            {
                var files = Directory.GetFiles(documentsFolder);
                var totalFiles = files.Length;
                _backupProgress = 0;

                using (var zipArchive = ZipFile.Open(backupFilePath, ZipArchiveMode.Create))
                {
                    for (int i = 0; i < totalFiles; i++)
                    {
                        var file = files[i];
                        zipArchive.CreateEntryFromFile(file, Path.GetFileName(file));
                        _backupProgress = (int)((i + 1) / (double)totalFiles * 100);
                        await Task.Delay(10); // Simulate work
                    }
                }

                _backupProgress = 100; // Ensure it's set to 100 when done
                return Json(new { success = true, message = "Documents backup created successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error creating documents backup: {ex.Message}" });
            }
            finally
            {
                _backupProgress = 0; // Reset progress after completion
            }
        }
        private List<DocumentsBackupViewModel> GetDocumentsBackupFilesModel()
        {
            var backupFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documents_backup");
            var files = Directory.GetFiles(backupFolder);
            var model = new List<DocumentsBackupViewModel>();

            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                model.Add(new DocumentsBackupViewModel
                {
                    FileName = fileInfo.Name,
                    DateCreated = fileInfo.CreationTime
                });
            }

            return model;
        }

        [CustomAuthorize("BackupModule")]
        public IActionResult DocumentsBackup()
        {
            var model = GetDocumentsBackupFilesModel();
            return View(model);
        }

        public IActionResult GetDocumentsBackupFiles()
        {
            var model = GetDocumentsBackupFilesModel();
            return PartialView("_DocumentsBackupTable", model);
        }

        public IActionResult DownloadDocumentsBackup(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("Invalid file name.");
            }

            var backupFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documents_backup");
            var backupFilePath = Path.Combine(backupFolder, fileName);

            if (!System.IO.File.Exists(backupFilePath))
            {
                return NotFound("File not found.");
            }

            var fileBytes = System.IO.File.ReadAllBytes(backupFilePath);
            return File(fileBytes, "application/zip", fileName);
        }
    }

    public class BackupViewModel
    {
        public string FileName { get; set; }
        public DateTime? DateCreated { get; set; }
    }

    public class DocumentsBackupViewModel
    {
        public string FileName { get; set; }
        public DateTime DateCreated { get; set; }
    }
}

using DRS.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRS.Services
{
    public class LogService : ILogService
    {
        private readonly DRSdbContext _context;

        public LogService(DRSdbContext context)
        {
            _context = context;
        }

        public async Task LogAsync(int userId, string action, int documentId)
        {
            var log = new Log
            {
                UserId = userId,
                Action = action,
                DocumentId = documentId,
                Timestamp = DateTime.Now
            };

            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Log>> GetAllLogs()
        {
            try
            {
                var logs = await _context.Logs
                    .Include(log => log.User)
                    .Include(log => log.Document)
                    .ToListAsync();

                if (logs == null || !logs.Any())
                {
                    // Log that no data was found
                    Console.WriteLine("No logs found.");
                }

                return logs;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error retrieving logs: {ex.Message}");
                return new List<Log>();
            }
        }

    }
}

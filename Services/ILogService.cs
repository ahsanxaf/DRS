using DRS.Models;

namespace DRS.Services
{
    public interface ILogService
    {
        Task LogAsync(int userId, string action, int documentId);

        Task<List<Log>> GetAllLogs();
    }
}

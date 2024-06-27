using DRS.Models;

namespace DRS.Services
{
    public interface IUserLogService
    {
        Task LogUserActivity(UserLog userLog);
        Task<UserLog> GetActiveLogForUser(int userId);
        Task UpdateUserLog(UserLog userLog);
        Task<List<UserLog>> GetAllUserLogs();
    }
}

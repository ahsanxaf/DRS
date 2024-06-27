using DRS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DRS.Services
{
    public class UserLogService : IUserLogService
    {
        private readonly DRSdbContext _dbContext;
        public UserLogService(DRSdbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task LogUserActivity(UserLog userLog)
        {
            _dbContext.UserLogs.Add(userLog);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<UserLog> GetActiveLogForUser(int userId)
        {
            return await _dbContext.UserLogs
                .Where(log => log.UserId == userId && log.LogOutTime == null)
                .OrderByDescending(log => log.LogInTime)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateUserLog(UserLog userLog)
        {
            _dbContext.UserLogs.Update(userLog);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<UserLog>> GetAllUserLogs()
        {
            return await _dbContext.UserLogs
                .Include(ul => ul.user)
                .ToListAsync();
        }
    }
}

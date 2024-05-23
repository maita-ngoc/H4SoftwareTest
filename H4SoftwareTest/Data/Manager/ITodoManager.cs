using H4SoftwareTest.Data.Models;

namespace H4SoftwareTest.Data.Manager
{
    public interface ITodoManager
    {
        public Task<List<Tasks>> GetAllTasksByUserIdAsync(int userId);
        public Task CreateTaskAsync(int userId, string task);
    }
}

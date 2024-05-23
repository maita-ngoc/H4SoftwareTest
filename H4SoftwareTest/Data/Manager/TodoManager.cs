using H4SoftwareTest.Code;
using H4SoftwareTest.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace H4SoftwareTest.Data.Manager
{
    public class TodoManager : ITodoManager
    {
        public TodoDbContext _context { get; set; }
        public EncryptionHandler _encryptionHandler { get; set; }
        public TodoManager(TodoDbContext context, EncryptionHandler encryptionHandler)
        {
            _context = context;
            _encryptionHandler = encryptionHandler;
        }

        public async Task CreateTaskAsync(int userId, string task)
        {
            string encryptedTask = await _encryptionHandler.EncryptAsymetriscParent(task);
            var _task = new Tasks
            {
                Task = encryptedTask,
                UserID = userId,
            };
            _context.Tasks.Add(_task);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Tasks>> GetAllTasksByUserIdAsync(int userId)
        {
            //List<Tasks> tasks = new List<Tasks>();
            //List<string> decryptedTasks = new List<string>();

            //var tst = _context.Tasks.Where(t => t.UserID == userId).ToList();

            return await _context.Tasks.Where(t => t.UserID == userId).ToListAsync();

        }
    }
}

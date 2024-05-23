using H4SoftwareTest.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace H4SoftwareTest.Data
{
    public class TodoDbContext:DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Cpr> Cprs { get; set; }
        public virtual DbSet<Tasks> Tasks { get; set; }
    }
}

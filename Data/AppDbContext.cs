using Microsoft.EntityFrameworkCore;
using SimpleNotesApp.Models;

namespace SimpleNotesApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Note> Notes {get; set;}
        public DbSet<Category> Categories {get; set;}

    }
}
using Microsoft.EntityFrameworkCore;

namespace ASPCore.Models
{
	public class FilmDbContext : DbContext
	{
		public FilmDbContext(DbContextOptions<FilmDbContext> options) : base(options) { }

		public DbSet<Film> Films { get; set; }
		public DbSet<Author> Authors { get; set; }
		public DbSet<Comment> Comments { get; set; }
	}
}
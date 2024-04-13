using NoteMobile.Models;

namespace NoteMobile.DataAccess;
using Microsoft.EntityFrameworkCore;

public class AppDbContext: DbContext
{
	public DbSet<NoteItem> NoteItems => Set<NoteItem>();

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		var dbPath = Path.Combine(FileSystem.AppDataDirectory, "myNotes.db");
		var connectionString = $"FileName={dbPath}";
		optionsBuilder.UseSqlite(connectionString);
	}
}
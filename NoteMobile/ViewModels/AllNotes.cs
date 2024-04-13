using System.Collections.ObjectModel;
using NoteMobile.DataAccess;

namespace NoteMobile.ViewModels;

public class AllNotes
{
	public ObservableCollection<Note> Notes { get; set; } = new ObservableCollection<Note>();

	public AllNotes(AppDbContext dbContext) =>
		LoadNotes(dbContext);

	public void LoadNotes(AppDbContext dbContext)
	{
		Notes.Clear();
		var notes = dbContext.NoteItems.Select(i=> new Note()
		{
			Id = i.Id,
			Text = i.Text,
			Date = i.Date
		});
		// Add each note into the ObservableCollection
		foreach (Note note in notes)
			Notes.Add(note);
	}
}
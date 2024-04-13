using NoteMobile.DataAccess;
using NoteMobile.Models;
using NoteMobile.ViewModels;

namespace NoteMobile.Views;

[QueryProperty(nameof(ItemId), nameof(ItemId))]
public partial class NotePage : ContentPage
{
	private readonly AppDbContext _dbContext;
	private Entry noteEntry;
	private const string NoteTextKey = "NoteText";
	private const string LastEditedNoteIdKey = "LastEditedNoteId";
	private bool _lastChangesSaved = false;

	public string ItemId
	{
		set { LoadNote(value); }
	}

	public NotePage(AppDbContext dbContext)
	{
		_dbContext = dbContext;
		InitializeComponent();
		
		string id;
		if (Preferences.ContainsKey(LastEditedNoteIdKey) && !string.IsNullOrEmpty(Preferences.Get(LastEditedNoteIdKey, string.Empty)))
		{
			id = Preferences.Get(LastEditedNoteIdKey, string.Empty);
		}
		else
		{
			id = Guid.NewGuid().ToString();
			Preferences.Set(LastEditedNoteIdKey, id);
		}
		
		LoadNote(id);
	}

	private async void SaveButton_Clicked(object sender, EventArgs e)
	{
		if (BindingContext is Note note)
		{
			var noteItem = _dbContext.NoteItems.FirstOrDefault(i => i.Id == note.Id);

			if (noteItem != null)
			{
				noteItem.Text = TextEditor.Text;
				noteItem.Date = DateTime.Now;
			}
			else
			{
				await _dbContext.NoteItems.AddAsync(new NoteItem()
				{
					Id = note.Id,
					Date = DateTime.Now,
					Text = TextEditor.Text
				});
			}
			Preferences.Set($"{NoteTextKey}_{note.Id}", string.Empty);
			Preferences.Set(LastEditedNoteIdKey, string.Empty);
			_lastChangesSaved = true;

			await _dbContext.SaveChangesAsync();
		}

		await Shell.Current.GoToAsync("..");
	}

	private async void DeleteButton_Clicked(object sender, EventArgs e)
	{
		if (BindingContext is Note note)
		{
			var noteItem = _dbContext.NoteItems.FirstOrDefault(i => i.Id == note.Id);

			if (noteItem != null)
			{
				_dbContext.NoteItems.Remove(noteItem);
			}

			Preferences.Set($"{NoteTextKey}_{note.Id}", string.Empty);
			Preferences.Set(LastEditedNoteIdKey, string.Empty);

			_lastChangesSaved = true;
			
			await _dbContext.SaveChangesAsync();
		}

		await Shell.Current.GoToAsync("..");
	}

	private void LoadNote(string id)
	{
		var noteModel = new Note()
		{
			Id = id
		};

		var note = _dbContext.NoteItems.FirstOrDefault(n => n.Id == id);

		if (note is not null)
		{
			noteModel.Date = note.Date;
			noteModel.Text = note.Text;
			if (!string.IsNullOrEmpty(Preferences.Get($"{NoteTextKey}_{note.Id}", string.Empty))) 
				noteModel.Text = Preferences.Get($"{NoteTextKey}_{note.Id}", note.Text);
		}
		else
		{
			var tempNoteText = Preferences.Get($"{NoteTextKey}_{id}", string.Empty);
			noteModel.Text = string.IsNullOrEmpty(tempNoteText) ? "" : tempNoteText;
		}

		BindingContext = noteModel;
	}
	
	protected override void OnDisappearing()
	{
		base.OnDisappearing();
		if (BindingContext is Note note)
		{
			var noteText = TextEditor.Text;
			if (!string.IsNullOrWhiteSpace(noteText))
			{
				// Только сохраняем текст, если заметка находится в процессе редактирования
				Preferences.Set($"{NoteTextKey}_{note.Id}", noteText);
				Preferences.Set(LastEditedNoteIdKey, note.Id);
			}
			else
			{
				Preferences.Set($"{NoteTextKey}_{note.Id}", string.Empty);
				Preferences.Set(LastEditedNoteIdKey, string.Empty);
			}

			if (_lastChangesSaved)
			{
				Preferences.Set($"{NoteTextKey}_{note.Id}", string.Empty);
				Preferences.Set(LastEditedNoteIdKey, string.Empty);
			}
		}
	}
}
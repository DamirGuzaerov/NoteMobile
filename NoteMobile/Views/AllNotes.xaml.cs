using NoteMobile.DataAccess;

namespace NoteMobile.Views;

public partial class AllNotes : ContentPage
{
	private readonly AppDbContext _dbContext;

	public AllNotes(AppDbContext dbContext)
	{
		_dbContext = dbContext;
		InitializeComponent();
		
		BindingContext = new ViewModels.AllNotes(_dbContext);
	}
	
	protected override void OnAppearing()
	{
		((ViewModels.AllNotes)BindingContext).LoadNotes(_dbContext);
	}

	private async void Add_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(NotePage));
	}

	private async void notesCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (e.CurrentSelection.Count != 0)
		{
			// Get the note model
			var note = (ViewModels.Note)e.CurrentSelection[0];

			// Should navigate to "NotePage?ItemId=path\on\device\XYZ.notes.txt"
			await Shell.Current.GoToAsync($"{nameof(NotePage)}?{nameof(NotePage.ItemId)}={note.Id}");

			// Unselect the UI
			notesCollection.SelectedItem = null;
		}
	}
}
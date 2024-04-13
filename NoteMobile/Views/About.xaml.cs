namespace NoteMobile.Views;

public partial class About : ContentPage
{
	public About()
	{
		InitializeComponent();
	}
	
	private async void LearnMore_Clicked(object sender, EventArgs e)
	{
		if (BindingContext is ViewModels.About about)
		{
			await Launcher.Default.OpenAsync(about.MoreInfoUrl);
		}
	}
}
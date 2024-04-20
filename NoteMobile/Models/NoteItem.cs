namespace NoteMobile.Models;

public class NoteItem
{
	public string Id { get; set; }
	public string Text { get; set; }
	public DateTime Date { get; set; }
	public Geolocation? Geolocation { get; set; }
    public string? AudioFilePath { get; set; }
}
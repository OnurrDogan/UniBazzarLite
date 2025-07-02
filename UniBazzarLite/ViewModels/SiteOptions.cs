namespace UniBazaarLite.ViewModels;

// Holds site-wide settings loaded from appsettings.json
public class SiteOptions
{
    public string OfficialName { get; set; } = "UniBazaar Lite"; // Site name
    public int MaxItemsPerPage { get; set; } = 20; // Pagination setting
}
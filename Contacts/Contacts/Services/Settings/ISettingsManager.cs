namespace Contacts.Services.Settings
{
    public interface ISettingsManager
    {
        bool IsAuthorized { get; set; }
    }
}
namespace Notification.Infrastructure.Persistance.Settings;

public class DatabaseSettings
{
    public const string OptionName = "MongoDatabaseSettings";
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string HostNotificationCollectionName { get; set; } = null!;
    public string GuestNotificationCollectionName { get; set; } = null!;
}
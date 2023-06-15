namespace Notification.Infrastructure.Persistance.Settings;

public class DatabaseSettings
{
    public const string OptionName = "MongoDatabaseSettings";
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string HostCollectionName { get; set; } = null!;
    public string GuestCollectionName { get; set; } = null!;
}
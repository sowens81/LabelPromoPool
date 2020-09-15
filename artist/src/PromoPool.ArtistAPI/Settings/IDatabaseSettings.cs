namespace PromoPool.ArtistAPI.Settings
{
    public interface IDatabaseSettings
    {
        string CollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}

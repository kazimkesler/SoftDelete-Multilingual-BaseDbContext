namespace SoftDeleteMultilingualBaseDbContext.Localization
{
    public interface ISharedResource
    {
        string this[string index] { get; }
    }
}

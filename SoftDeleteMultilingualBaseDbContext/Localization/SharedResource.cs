using Microsoft.Extensions.Localization;

namespace SoftDeleteMultilingualBaseDbContext.Localization
{
    public class SharedResource : ISharedResource
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public SharedResource(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        public string this[string index]
        {
            get
            {
                var val = _localizer[index];
                if (val.ResourceNotFound)
                    throw new ArgumentException($"Key not found, Searched Location: {val.SearchedLocation}");
                return _localizer[index];
            }
        }
    }
}

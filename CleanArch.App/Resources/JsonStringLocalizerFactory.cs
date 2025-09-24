using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;

namespace CleanArch.App.Resources
{
    public class JsonStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IDistributedCache _cache;

        public JsonStringLocalizerFactory(IDistributedCache cache)
        {
            _cache = cache;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            var genericType = typeof(JsonStringLocalizer<>).MakeGenericType(resourceSource);
            return (IStringLocalizer)Activator.CreateInstance(genericType, _cache);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return Create(typeof(SharedResource));
        }
    }
}

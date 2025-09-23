using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.Globalization;

namespace CleanArch.App.Resources
{
    public class JsonStringLocalizer<T> : IStringLocalizer<T>
    {
        private readonly IDistributedCache _cache;
        private readonly JsonSerializer _serializer = new();
        private readonly string _resourceName;
        private readonly string _resourcesPath = "Resources";

        public JsonStringLocalizer(IDistributedCache cache)
        {
            _cache = cache;
            _resourceName = typeof(T).Name; // عادةً "SharedResource"
        }

        public LocalizedString this[string name]
        {
            get
            {
                var value = GetString(name);
                return new LocalizedString(name, value ?? name, value == null);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var value = GetString(name);
                var formattedValue = value != null ? string.Format(value, arguments) : null;
                return new LocalizedString(name, formattedValue ?? name, formattedValue == null);
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var filePath = GetFilePath();
            if (filePath == null || !File.Exists(filePath))
                yield break;

            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var reader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(reader);

            while (jsonReader.Read())
            {
                if (jsonReader.TokenType == JsonToken.PropertyName)
                {
                    var key = jsonReader.Value as string;
                    jsonReader.Read();
                    var value = _serializer.Deserialize<string>(jsonReader);
                    yield return new LocalizedString(key, value);
                }
            }
        }

        private string GetString(string key)
        {
            var cultureName = CultureInfo.CurrentUICulture.Name;
            var cacheKey = $"locale_{cultureName}_{_resourceName}_{key}";

            var cachedValue = _cache.GetString(cacheKey);
            if (!string.IsNullOrEmpty(cachedValue))
                return cachedValue;

            var filePath = GetFilePath();
            if (filePath == null || !File.Exists(filePath))
                return null;

            var value = GetValueFromJSON(key, filePath);
            if (!string.IsNullOrEmpty(value))
                _cache.SetString(cacheKey, value);

            return value;
        }

        private string GetValueFromJSON(string propertyName, string filePath)
        {
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var reader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(reader);

            while (jsonReader.Read())
            {
                if (jsonReader.TokenType == JsonToken.PropertyName && (jsonReader.Value as string) == propertyName)
                {
                    jsonReader.Read();
                    return _serializer.Deserialize<string>(jsonReader);
                }
            }
            return null;
        }

        private string GetFilePath()
        {
            var cultureName = CultureInfo.CurrentUICulture.Name;
            var fileName = $"{_resourceName}.{cultureName}.json";
            var filePath = Path.Combine(_resourcesPath, fileName);

            return File.Exists(filePath) ? filePath : null; // ارجع null لو مش موجود
        }
    }
}

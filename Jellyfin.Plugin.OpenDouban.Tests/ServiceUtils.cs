using System;
using System.IO;
using System.Threading.Tasks;
using MediaBrowser.Model.Serialization;
using ServiceStack.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Jellyfin.Plugin.OpenDouban.Tests
{
    class ServiceUtils
    {
        public static ServiceProvider BuildServiceProvider<T>(ITestOutputHelper output) where T : class
        {
            var services = new ServiceCollection()
                .AddHttpClient()
                .AddLogging(builder => builder.AddXUnit(output).SetMinimumLevel(LogLevel.Debug))
                .AddSingleton<IJsonSerializer, MockJsonSerializer>()
                .AddSingleton<T>();

            var serviceProvider = services.BuildServiceProvider();

            // Used For FrodoAndroidClient which can not use typed ILogger.
            var logger = serviceProvider.GetService<ILogger<T>>();
            services.AddSingleton<ILogger>(logger);

            return services.BuildServiceProvider();
        }
    }

    class MockJsonSerializer : IJsonSerializer
    {
        public Task<T> DeserializeFromStreamAsync<T>(Stream stream)
        {
            return JsonSerializer.DeserializeFromStreamAsync<T>(stream);
        }

        public Task<object> DeserializeFromStreamAsync(Stream stream, Type t)
        {
            return JsonSerializer.DeserializeFromStreamAsync(t, stream);
        }

        public T DeserializeFromStream<T>(Stream s)
        {
            throw new NotImplementedException();
        }

        public object DeserializeFromStream(Stream s, Type t)
        {
            throw new NotImplementedException();
        }

        public T DeserializeFromFile<T>(string File) where T : class
        {
            throw new NotImplementedException();
        }

        public object DeserializeFromFile(Type t, string File)
        {
            throw new NotImplementedException();
        }

        public T DeserializeFromString<T>(string text)
        {
            return JsonSerializer.DeserializeFromString<T>(text);
        }

        public object DeserializeFromString(string Json, Type t)
        {
            throw new NotImplementedException();
        }

        public void SerializeToFile(object obj, string file)
        {
            throw new NotImplementedException();
        }

        public string SerializeToString(object obj)
        {
            return JsonSerializer.SerializeToString(obj);
        }

        public void SerializeToStream<T>(T obj, Stream stream)
        {
            JsonSerializer.SerializeToStream(obj, stream);
        }

        public void SerializeToStream(object obj, Stream stream)
        {
            JsonSerializer.SerializeToStream(obj, stream);
        }
    }
}

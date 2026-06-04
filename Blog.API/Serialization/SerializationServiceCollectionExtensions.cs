using System.Text.Json;
using Microsoft.Extensions.Options;

namespace Blog.API.Serialization;

public static class SerializationServiceCollectionExtensions
{
    public static IServiceCollection AddBlogApiSerialization(this IServiceCollection services, IConfiguration? configuration = null)
    {
        if (configuration is not null)
            services.Configure<SerializationOptions>(configuration.GetSection(SerializationOptions.SectionName));
        else
            services.Configure<SerializationOptions>(_ => { });

        services.AddSingleton(JsonSerializerOptionsFactory.Create());
        services.AddSingleton<IApiContentSerializer, JsonApiContentSerializer>();
        services.AddSingleton<IApiContentSerializerResolver, DefaultApiContentSerializerResolver>();

        services.ConfigureHttpJsonOptions(options =>
        {
            var shared = JsonSerializerOptionsFactory.Create();
            options.SerializerOptions.PropertyNamingPolicy = shared.PropertyNamingPolicy;
            options.SerializerOptions.PropertyNameCaseInsensitive = shared.PropertyNameCaseInsensitive;
            options.SerializerOptions.DefaultIgnoreCondition = shared.DefaultIgnoreCondition;
        });

        return services;
    }
}

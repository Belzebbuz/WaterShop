using System;
using WaterShop.ConfigOptions;

namespace WaterShop;

public static class ConfigOptionsExtensions
{
    public static IServiceCollection AddConfigOptions(this IServiceCollection services, IConfiguration configuration)
    {
		var saveImageOptions = configuration.GetSection(nameof(SaveImageOptions)).Get<SaveImageOptions>() ?? new();
		services.AddSingleton(saveImageOptions);
		var secretKeyOptions = configuration.GetSection(nameof(SecretKeyOptions)).Get<SecretKeyOptions>() ?? new();
		services.AddSingleton(secretKeyOptions);
		return services;
    }
}

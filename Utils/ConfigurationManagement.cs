namespace TeraClouds.Utils;

using Microsoft.Extensions.Configuration;

public class ConfigurationManagement
{
	public static IConfiguration BuildConfig()
	{
		var configuration = new ConfigurationBuilder()
								.SetBasePath(Directory.GetCurrentDirectory())
								.AddJsonFile("appsettings.json", optional: true)
								.Build();

		return configuration;
	}
}
using BE_CodeTest.BL;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace BE_CodeTest.Extensions
{
	[ExcludeFromCodeCoverage]
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddBusinessServices(this IServiceCollection services)
		{
			services.AddSingleton<ICasinoService, CasinoService>();
			services.AddSingleton<IWalletService, WalletService>();

			return services;
		}
	}
}

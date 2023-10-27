using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EllipticBit.Services.Dialog.WPF
{
	public static class Initializer
	{
		public static void RegisterDialogService(this IServiceCollection builder) {
			builder.AddSingleton<DialogService>();
			builder.TryAddTransient<MessageDialogSettings>();
			builder.TryAddSingleton<LoginDialogSettings>();
		}
	}
}

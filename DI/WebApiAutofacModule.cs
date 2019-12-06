using Autofac;
using Microsoft.AspNetCore.Http;
using TurboApi.Services;

namespace TurboApi.DI
{
	public class WebApiAutofacModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.Register(ctx =>
				new UserContext(ctx.Resolve<IHttpContextAccessor>().HttpContext.Request, ctx.Resolve<DbLocalContext>()))
				.AsImplementedInterfaces()
				.InstancePerLifetimeScope();
		}
	}
}

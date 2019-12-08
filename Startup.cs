using System;
using System.IO;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using TurboApi.DI;
using TurboApi.Exceptions;
using TurboApi.Services;

namespace TurboApi
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services.AddHttpContextAccessor();
			services.AddResponseCaching();
			services.AddDirectoryBrowser();
			services.AddMvc(
				config => { config.Filters.Add(typeof(ExceptionHandler)); });

			services.AddCors(options => { options.AddPolicy("TurboPolicy", builder => CorsPolicyBuilder(builder)); });
			var connection = Configuration.GetConnectionString("DefaultConnection");
			services.AddDbContext<DbLocalContext>(options =>
				options.UseSqlServer(connection));
			var containerBuilder = new ContainerBuilder();
			containerBuilder.RegisterModule(new WebApiAutofacModule());
			containerBuilder.Populate(services);
			return new AutofacServiceProvider(containerBuilder.Build());
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
				app.UseDeveloperExceptionPage();
			
			app.Use(async (context, next) =>
			{
				context.Response.GetTypedHeaders().CacheControl =
					new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
					{
						NoCache = true,
						NoStore = true,
						MustRevalidate = true,
						MaxAge = TimeSpan.Zero
					};
				context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
					new[] { "Accept-Encoding" };

				await next().ConfigureAwait(false);
			});

			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "turbo.front")),
				RequestPath = new PathString("")
			});

			app.UseDirectoryBrowser(new DirectoryBrowserOptions
			{
				FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "turbo.front")),
				RequestPath = new PathString("")
			});

			app.UseCors("TurboPolicy");
			app.UseMvc();
		}

		private CorsPolicyBuilder CorsPolicyBuilder(CorsPolicyBuilder builder)
		{
			builder.AllowAnyHeader();
			builder.AllowAnyMethod();
			builder.AllowCredentials();
			builder.SetIsOriginAllowed(origin => true);
			return builder;
		}
	}
}

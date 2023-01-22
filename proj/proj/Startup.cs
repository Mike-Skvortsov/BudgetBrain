using Auth.Common;
using Autofac;
using BL;
using DataAccess;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using proj.MappingProfiles;

namespace proj
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{

			var authOptionsConfiguration = Configuration.GetSection("Auth");
			services.Configure<AuthOptions>(authOptionsConfiguration);

			services.AddAutoMapper(x => x.AddProfile(new PresentationLayerMappingProfile()));
			services.AddDbContext<DataBaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DB")));
			services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "proj", Version = "v1" });
			});

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options => //CookieAuthenticationOptions
				{
					options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
				});
			services.AddControllersWithViews();

			services.AddCors(options =>
			{
				options.AddDefaultPolicy(
					builder =>
					{
						builder.AllowAnyOrigin()
						.AllowAnyMethod()
						.AllowAnyHeader();
					});
			});
		}
		public void ConfigureContainer(ContainerBuilder builder)
		{
			builder.RegisterModule<BlRegistrationModule>();
		}
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "proj v1"));
			}

			app.UseHttpsRedirection();

			app.UseRouting();
			app.UseCors();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}

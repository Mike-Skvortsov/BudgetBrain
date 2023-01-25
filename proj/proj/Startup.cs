using Auth.Common;
using Autofac;
using BL;
using DataAccess;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using proj.MappingProfiles;
using System;

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
			services.AddAutoMapper(x => x.AddProfile(new PresentationLayerMappingProfile()));
			services.AddDbContext<DataBaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DB")));
			services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "proj", Version = "v1" });
			});

			services.AddOptions<AuthOptions>().Bind(Configuration.GetSection("Auth"));
			var authOptionsConfiguration = Configuration.GetSection("Auth").Get<AuthOptions>();

			if (authOptionsConfiguration == null || string.IsNullOrEmpty(authOptionsConfiguration.Secret))
			{
				authOptionsConfiguration.Secret = Environment.GetEnvironmentVariable("AUTH_SECRET") ?? "defaultValue";
			}


			services.AddSingleton(authOptionsConfiguration);
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
		.AddJwtBearer(options =>
		{

			options.RequireHttpsMetadata = false;
			options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidIssuer = authOptionsConfiguration.Issuer,

				ValidateAudience = true,
				ValidAudience = authOptionsConfiguration.Audience,
				ValidateLifetime = true,

				IssuerSigningKey = authOptionsConfiguration.GetSynnetricSecurityKey(),
				ValidateIssuerSigningKey = true,
			};
		});

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

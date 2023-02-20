using Auth.Common;
using Autofac;
using BL;
using BL.Services;
using DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using proj.MappingProfiles;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Text;

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
			services.AddLogging();
			services.AddAutoMapper(x => x.AddProfile(new PresentationLayerMappingProfile()));
			services.AddDbContext<DataBaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DB")));
			services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
			services.AddSwaggerGen(c =>
			{
				c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
				{
					Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
					In = ParameterLocation.Header,
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey
				});
				c.OperationFilter<SecurityRequirementsOperationFilter>();
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "BudgetBrain", Version = "v1" });
			});


			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
				{
					ValidateIssuer = false,
					//ValidIssuer = Configuration["Auth:Issuer"],

					ValidateAudience = false,
					//ValidAudience = Configuration["Auth:Audience"],

					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
					.GetBytes(Configuration.GetSection("Auth:Secret").Value))
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
			services.AddSingleton<IUserService, UserService>();
			services.AddSingleton<ICardService, CardService>();
			services.AddSingleton<IOperationService, OperationService>();
		}
		public void ConfigureContainer(ContainerBuilder builder)
		{
			builder.RegisterModule<BlRegistrationModule>();
		}
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "BudgetBrain v1");
			});
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();
			app.UseCors();
			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}

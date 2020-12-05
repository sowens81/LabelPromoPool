using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using PromoPool.ArtistAPI.Managers;
using PromoPool.ArtistAPI.Managers.Implementations;
using PromoPool.ArtistAPI.Requirements;
using PromoPool.ArtistAPI.Services;
using PromoPool.ArtistAPI.Services.Implementations;
using PromoPool.ArtistAPI.Settings;
using PromoPool.ArtistAPI.Settings.Implementations;
using System;
using System.Collections.Generic;

namespace PromoPool.ArtistAPI
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
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder
                        .WithOrigins("http://localhost:3000", "http://localhost:8081" )
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
            });

            services.Configure<DatabaseSettings>(
                Configuration.GetSection(nameof(DatabaseSettings)));

            services.AddSingleton<IDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);
            
            // var appDomain = Configuration["Auth0Settings:AppDomain"];
            // var identifier = Configuration["Auth0Settings:Identifier"];
            var gitRepo = Configuration["GitHubSettings:GitRepo"];

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "PromoPool ArtistAPI",
                    Description = "PromoPool ArtistAPI REST API Service",
                    TermsOfService = new Uri($"{gitRepo}/blob/master/readme.md"),
                    Contact = new OpenApiContact
                    {
                        Name = "ArtistPromoPool",
                        Email = string.Empty,
                        Url = new Uri(gitRepo),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under License",
                        Url = new Uri($"{gitRepo}/license"),
                    }
                });
                /*
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
                */
            });

            services.AddScoped<IArtistManager, ArtistManager >();
            services.AddScoped<IMongoDBPersistance, MongoDBPersistance>();
            services.AddScoped<IValidation, Validation>();

            services.AddApiVersioning(o => {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });

            /*
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = appDomain;
                    options.Audience = identifier;
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("read:artists", policy => policy.Requirements.Add(new HasScopeRequirement("read:artists", appDomain)));
                options.AddPolicy("post:addartist", policy => policy.Requirements.Add(new HasScopeRequirement("post:addartist", appDomain)));
            });

            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            */

        }
         
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (env.IsProduction() || env.IsStaging() )
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowSpecificOrigin");

            // app.UseAuthentication();

            // app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

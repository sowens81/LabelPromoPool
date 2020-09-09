using System;
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
using PromoPool.LabelAPI.Managers;
using PromoPool.LabelAPI.Managers.Implementations;
using PromoPool.LabelAPI.Requirements;
using PromoPool.LabelAPI.Services;
using PromoPool.LabelAPI.Services.Implementations;
using PromoPool.LabelAPI.Settings;
using PromoPool.LabelAPI.Settings.Implementations;

namespace PromoPool.LabelAPI
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
            services.Configure<DatabaseSettings>(
                Configuration.GetSection(nameof(DatabaseSettings)));

            services.AddSingleton<IDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);
            
            var appDomain = Configuration["Auth0Settings:AppDomain"];
            var identifier = Configuration["Auth0Settings:Identifier"];
            var gitRepo = Configuration["OtherSettings:GitRepo"];

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "PromoPool LabelAPI",
                    Description = "PromoPool LabelAPI REST API Service",
                    TermsOfService = new Uri($"{gitRepo}/blob/master/readme.md"),
                    Contact = new OpenApiContact
                    {
                        Name = "LabelPromoPool",
                        Email = string.Empty,
                        Url = new Uri(gitRepo),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under License",
                        Url = new Uri($"{gitRepo}/license"),
                    }
                });

            });

            services.AddScoped<ILabelManager, LabelManager >();
            services.AddScoped<IMongoDBPersistance, MongoDBPersistance>();
            services.AddScoped<IValidation, Validation>();

            services.AddApiVersioning(o => {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = appDomain;
                options.Audience =  identifier;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("read:messages", policy => policy.Requirements.Add(new HasScopeRequirement("read:messages", appDomain)));
            });

            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();


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

            app.UseAuthentication();

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

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
using PromoPool.audioConverterAPI.Requirements;
using PromoPool.audioConverterAPI.Services;
using PromoPool.audioConverterAPI.Services.Implementations;
using PromoPool.audioConverterAPI.Settings;
using PromoPool.audioConverterAPI.Settings.Implementations;
using System;
using System.Collections.Generic;


namespace PromoPool.audioConverterAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder
                        .WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
            });

            services.Configure<MessageQueueSettings>(
                Configuration.GetSection(nameof(MessageQueueSettings)));

            services.AddSingleton<IMessageQueueSettings>(sp =>
                sp.GetRequiredService<IOptions<MessageQueueSettings>>().Value);

            var appDomain = Configuration["Auth0Settings:AppDomain"];
            var identifier = Configuration["Auth0Settings:Identifier"];
            var gitRepo = Configuration["GitHubSettings:GitRepo"];

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "PromoPool AudioConverterAPI",
                    Description = "PromoPool AudioConverterAPI REST API Service",
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
            });

            services.AddScoped<IMessageQueue, MessageQueue>();

            services.AddApiVersioning(o => {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = appDomain;
                    options.Audience = identifier;
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("read:labels", policy => policy.Requirements.Add(new HasScopeRequirement("read:labels", appDomain)));
                options.AddPolicy("post:addlabel", policy => policy.Requirements.Add(new HasScopeRequirement("post:addlabel", appDomain)));
            });

            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (env.IsProduction() || env.IsStaging())
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

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

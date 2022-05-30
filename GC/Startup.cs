using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using GC.Domain.Configurations;
using GC.Domain.RequestHandlers;
using GC.Domain.Services;
using GC.Domain.Services.Implementations;
using GC.Domain.Services.Repositories;
using GC.Adapters.EF;
using GC.API;
using System.Text;
using GC.Adapters.EF.Repositories;

namespace GC
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
            var key = Encoding.ASCII.GetBytes(Configuration["Auth:EncryptionKey"]);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddControllers();

            RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<AuthContextInitializerMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<AuthContextInitializerMiddleware>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IPasswordService, PasswordService>();
            services.AddScoped<IAuthContextInitializer, AuthContextInitializer>();
            services.AddTransient(x => x.GetService<IAuthContextInitializer>().GetCurrentContext());

            services.AddTransient<IAuthorizationConfiguration, AppSettingsConfiguration>();
            services.AddTransient<IEFConfiguration, AppSettingsConfiguration>();

            services.AddTransient<IDBContextFactory, DbContextFactory>();

            services.AddTransient<LoginRequestHandler>();
            services.AddTransient<AddPlacesRequestHandler>();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IPlaceRepository, PlaceRepository>();
        }
    }
}

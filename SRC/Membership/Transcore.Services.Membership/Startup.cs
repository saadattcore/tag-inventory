using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transcore.Services.Membership.Authentication;
using Transcore.Services.Membership.DataStore;
using Transcore.Services.Membership.IdentityModels;
using Transcore.Services.Membership.Models;

namespace Transcore.Services.Membership
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Transcore.Services.Membership", Version = "v1" });
            });

            var mapperConfig = new MapperConfiguration(Startup.MapperConfigure);
            var mapper = mapperConfig.CreateMapper();

            services.AddSingleton<IMapper>(mapper);

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ConnectionString")));

            services.AddIdentity<ApplicationUser, ApplicationRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<UserManager<ApplicationUser>>();

            services.AddSingleton<IConfiguration>(Configuration);


            if (Configuration.GetValue<string>("DataSource").Equals("EntityFramework"))
            {
                services.AddScoped<IRepository, EFRepository>();
            }
            else
            {
                services.AddScoped<IRepository, ActiveDirectoryRepository>();
            }

            services.AddScoped<ITokenManager, JwtTokenManager>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;               
                o.SaveToken = true;
                var Key = Encoding.UTF8.GetBytes(Configuration["JwtKey"]);
                o.TokenValidationParameters = new TokenValidationParameters
                {
               
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    
                };
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transcore.Services.Membership v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();           

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public static void MapperConfigure(IMapperConfigurationExpression config)
        {
            config.CreateMap<UserModel, ApplicationUser>().ReverseMap();
        }
    }
}

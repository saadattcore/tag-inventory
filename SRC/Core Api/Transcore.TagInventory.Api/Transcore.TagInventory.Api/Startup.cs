using AutoMapper;
using Inventory.BusinessLogic;
using log4net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transcore.TagInventory.Api.JsonConverter;
using Transcore.TagInventory.BusinessLogic;
using Transcore.TagInventory.BusinessLogic.LookUp;
using Transcore.TagInventory.Common.Caching;
using Transcore.TagInventory.Common.Reporting;
using Transcore.TagInventory.DataAccess;
using Transcore.TagInventory.DataAccess.ExportPackage;
using Transcore.TagInventory.DataAccess.Lookup;
using Transcore.TagInventory.Web.Common;
using Transcore.TagInventory.Web.Models;
using DTO = Transcore.TagInventory.Entity;

namespace Transcore.TagInventory.Api
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

            services.AddControllers().
                AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new ApplicationDateTimeConverter()))
                .AddJsonOptions(o => o.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApplication2", Version = "v1" });
            });


            var mapperConfig = new MapperConfiguration(Startup.MapperConfigure);
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton<IMapper>(mapper);

            services.AddSingleton<IConfiguration>(provider => Configuration);            

            services.AddScoped<IReceivedBoxProvider, ReceivedBoxProvider>();

            services.AddScoped(typeof(IReceivedBoxRepository), typeof(ReceivedBoxRepository));

            services.AddScoped<IShipmentProvider, ShipmentProvider>();

            services.AddScoped<IShipmentRepository, ShipmentRepository>();

            services.AddScoped<ITagProvider, TagProvider>();

            services.AddScoped<ITagRepository, TagRepository>();

            services.AddScoped<ICacheManager, CacheManager>();

            services.AddScoped<ILookupProvider, LookupProvider>();

            services.AddScoped<IIssuedBoxProvider, IssuedBoxProvider>();

            services.AddScoped<ILookupRepository, LookupRepository>();

            services.AddScoped<IIssuedBoxRepository, IssuedBoxRepository>();

            services.AddScoped<ILabelPrinter, LabelPrinter>();

            services.AddScoped<IExportPackageProvider, ExportPackageProvider>();

            services.AddScoped<IExportPackageRepository, ExportPackageRepository>();

            services.AddScoped<IHttpRequestHandler, HttpRequestHandler>();

            services.AddScoped<ILog>((provider) => LogManager.GetLogger("Default"));

            //var conString = Transcore.EncryptionLibraryCore.Encryption.Decrypt(Configuration.GetConnectionString("TagInventoryDataBase"), "Tr@N$c0Re@dXBtWo02t0o");
            

            services.AddScoped<IDataAccess>(provider => new DataAccess.SqlDataAccess(Configuration.GetConnectionString("TagInventoryDataBase")));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication2 v1"));
            }

            //app.UseHttpsRedirection(); disable it to allow request for http connections

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(options => options.WithOrigins("*").AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public static void MapperConfigure(IMapperConfigurationExpression config)
        {
            config.CreateMap<ShipmentCreateUpdate, DTO.Core.Shipment>().ReverseMap();

            config.CreateMap<Shipment, DTO.Core.Shipment>()
                .AfterMap((src, dest) =>
                {
                    if (src.ReceivedBoxes != null)
                    {
                        foreach (var item in dest.ReceivedBoxes)
                        {
                            item.Shipment = dest;
                        }
                    }

                })
                .ReverseMap()
                .AfterMap((src, dest) =>
                {
                    if (src.ReceivedBoxes != null)
                    {
                        foreach (var item in dest.ReceivedBoxes)
                        {
                            item.Shipment = dest;
                        }
                    }



                });


            config.CreateMap<ReceivedBox, DTO.Core.ReceivedBox>()
                .AfterMap((src, dest) =>
                {

                    if (src.Tags != null && src.Tags.Count > 0)
                    {
                        foreach (Tag item in src.Tags)
                        {
                            item.ReceivedBox = src;
                        }
                    }

                }).ReverseMap()
                .AfterMap((src, dest) =>
                {
                    if (src.Tags != null && src.Tags.Count > 0)
                    {
                        foreach (DTO.Core.Tag item in src.Tags)
                        {
                            item.ReceivedBox = src;
                        }
                    }
                });

            config.CreateMap<Tag, DTO.Core.Tag>().ReverseMap();

            config.CreateMap<ScannedTagUpdate, DTO.Core.Tag>().ReverseMap();

            config.CreateMap<IssuedBox, DTO.Core.IssuedBox>().ReverseMap();

            config.CreateMap<IssuedBoxBase, DTO.Core.IssuedBox>().ReverseMap();

            config.CreateMap<TagSearch, DTO.Model.TagSearch>();

            config.CreateMap<ShipmentSearch, DTO.Model.ShipmentSearch>();

            config.CreateMap<ReceivedBoxSearch, DTO.Model.ReceivedBoxSearch>();

            config.CreateMap<IssuedBoxSearch, DTO.Model.IssuedBoxSearch>();

            config.CreateMap<ScannedReceivedBoxUpdate, DTO.Model.ScannedReceivedBoxUpdate>().ReverseMap();

            config.CreateMap<ScannedTagUpdate, DTO.Model.ScannedTagUpdate>().ReverseMap();

            config.CreateMap<Distributor, DTO.Core.Distributor>().ReverseMap();

            config.CreateMap<DistributorType, DTO.Core.DistributorType>().ReverseMap();

            config.CreateMap<DistributorAndTypes, DTO.Core.DistributorAndTypes>().ReverseMap();

            config.CreateMap<TagActivityHistory, DTO.Model.TagActivityHistory>().ReverseMap();

            config.CreateMap<IssuedBoxActivityHistory, DTO.Model.IssuedBoxActivityHistory>().ReverseMap();

            config.CreateMap<IssuedBoxActivityHistory, DTO.Model.IssuedBoxActivityHistory>().ReverseMap();

            config.CreateMap<ReceivedBoxUpdate, DTO.Model.ReceivedBoxUpdate>().ReverseMap();



        }
    }
}

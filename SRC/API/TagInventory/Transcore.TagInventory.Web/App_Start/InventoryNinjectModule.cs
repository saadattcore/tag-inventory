using AutoMapper;
using Inventory.BusinessLogic;
using Transcore.TagInventory.Web.Common;
using log4net;
using log4net.Core;
//using Transcore.TagInventory.DataAccess;
using Ninject.Modules;
using Ninject.Planning.Bindings;
using Ninject.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Transcore.TagInventory.BusinessLogic;
using Transcore.TagInventory.DataAccess;
using System.Web.Http.Validation;
using Ninject.Web.WebApi.Validation;
using Transcore.TagInventory.Common.Caching;
using Transcore.TagInventory.BusinessLogic.LookUp;
using Transcore.TagInventory.DataAccess.Lookup;
using Transcore.TagInventory.Common.Reporting;
using Transcore.TagInventory.BusinessLogic;

//using Transcore.TagInventory.Common.Infrastructure;

namespace Transcore.TagInventory.App_Start
{
    public class InventoryNinjectModule : NinjectModule
    {
        public override void Load()
        {

            this.Rebind<ModelValidatorProvider>().To<NinjectDefaultModelValidatorProvider>();

            RegisterServices(this);
            RegisterAutoMapper(this);
            RegisterLogger(this);
        }

        public static void RegisterAutoMapper(NinjectModule module)
        {
            var mapperConfig = new MapperConfiguration(AutoMapperConfiguration.Configure);
            var mapper = mapperConfig.CreateMapper();
            module.Bind<IMapper>().ToConstant(mapper).InSingletonScope();
        }

        public static void RegisterLogger(NinjectModule module)
        {
            module.Bind<ILog>().ToMethod(ctx => LogManager.GetLogger("Default")).InRequestScope();
        }

        public static void RegisterServices(NinjectModule module)
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["TagInventoryDataBase"].ConnectionString;

            module.Bind<IReceivedBoxProvider>().To<ReceivedBoxProvider>().InRequestScope();
            module.Bind<IReceivedBoxRepository>().To<ReceivedBoxRepository>().InRequestScope();
            module.Bind<IHttpRequestHandler>().To<HttpRequestHandler>().InRequestScope();
            module.Bind<IShipmentProvider>().To<ShipmentProvider>().InRequestScope();
            module.Bind<IDataAccess>().ToMethod(ctx => new SqlDataAccess(connectionString)).InRequestScope();
            module.Bind<IShipmentRepository>().To<ShipmentRepository>().InRequestScope();
            module.Bind<ITagProvider>().To<TagProvider>().InRequestScope();
            module.Bind<ITagRepository>().To<TagRepository>().InRequestScope();
            module.Bind<ICacheManager>().To<CacheManager>().InSingletonScope();
            module.Bind<ILookupProvider>().To<LookupProvider>().InSingletonScope();
            module.Bind<ILookupRepository>().To<LookupRepository>().InRequestScope();
            module.Bind<IIssuedBoxProvider>().To<IssuedBoxProvider>().InRequestScope();
            module.Bind<IIssuedBoxRepository>().To<IssuedBoxRepository>().InRequestScope();
            module.Bind<ILabelPrinter>().To<LabelPrinter>().InRequestScope();
            module.Bind<ISettingsProvider>().To<SettingsProvider>().InRequestScope();


        }
    }
}

using log4net.Config;
//using log4net.Config;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common.WebHost;
using Ninject.Web.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Transcore.TagInventory.App_Start;
using Transcore.TagInventory.BusinessLogic;
using Transcore.TagInventory.Common;

namespace InventoryManagement
{
    public class Global : NinjectHttpApplication
    {
        
        protected override void OnApplicationStarted()
        {
            base.OnApplicationStarted();
            
            GlobalConfiguration.Configure(WebApiConfig.Register);

            XmlConfigurator.Configure();
        }

        protected override IKernel CreateKernel()
        {
            var modules = new NinjectModule[] { new InventoryNinjectModule() };

            var kernel = new StandardKernel(modules);        

            var resolver = new NinjectDependencyResolver(kernel);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;

            return kernel;

        }
    }
}

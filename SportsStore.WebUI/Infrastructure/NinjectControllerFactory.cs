using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using Ninject;
using SportsStore.Domain;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Concrete;
using SportsStore.WebUI.Infrastructure.Abstract;
using SportsStore.WebUI.Infrastructure.Concrete;

namespace SportsStore.WebUI.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext
            requestContext, Type controllerType)
        {
            return controllerType == null
                ? null
                : (IController) ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            // put bindings here
//            Mock<IProductRepository> mock = new Mock<IProductRepository>();
//            mock.Setup(m => m.Products).Returns(new List<Product>
//            {
//                new Product {Name = "Football", Price = 25},
//                new Product {Name = "Surf board", Price = 179},
//                new Product {Name = "Running shoes", Price = 95}
//            }.AsQueryable());
//            ninjectKernel.Bind<IProductRepository>().ToConstant(mock.Object);

            ninjectKernel.Bind<IProductRepository>().To<EFProductRepository>();

            // We specified a value for only one of the
            //EmailSettings properties: WriteAsFile. We read the value of this property using the
            //ConfigurationManager.AppSettings property, which allows us to access application settings we have
            //placed in the Web.config file (the one in the root project folder)

            EmailSettings emailSettings = new EmailSettings
            {
                WriteAsFile = bool.Parse(ConfigurationManager
                .AppSettings["Email.WriteAsFile"] ?? "false")
            };

            // We created an EmailSettings object, which we use with the Ninject WithConstructorArgument method
            //so that we can inject it into the EmailOrderProcessor constructor when new instances are created to
            //service requests for the IOrderProcessor interface.

            ninjectKernel.Bind<IOrderProcessor>()
            .To<EmailOrderProcessor>()
            .WithConstructorArgument("settings", emailSettings);

            //The implementation of the FormsAuthProvider's Authenticate method calls the static FormsAuthentication methods that
            //we wanted to keep out of the controller. 
            ninjectKernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
        }
    }
}

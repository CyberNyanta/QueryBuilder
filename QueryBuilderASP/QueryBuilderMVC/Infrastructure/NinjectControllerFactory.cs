using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using System.Web.Mvc;
using System.Web.Routing;
using QueryBuilder.DAL.Repositories;
using QueryBuilder.DAL.Infrastructure;
using QueryBuilder.DAL.Contracts;

namespace QueryBuilderMVC.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        DatabaseFactory DbFactory = new DatabaseFactory();

        private IKernel ninjectKernel;
        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBinding();
        }
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null
                ? null
                : (IController)ninjectKernel.Get(controllerType);
        }
        private void AddBinding()
        {
           ninjectKernel.Bind<IUnitOfWorkFactory>().To<UnitOfWorkFactory>();
        }
    }
}
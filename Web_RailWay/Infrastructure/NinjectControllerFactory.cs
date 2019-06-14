using EFKIS.Abstract;
using EFKIS.Concrete;
using EFMT.Abstract;
using EFMT.Concrete;
using EFRC.Abstract;
using EFRC.Concrete;
using EFRW.Abstract;
using EFRW.Concrete;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace Web_RailWay.Infrastructure
{
    //// реализация пользовательской фабрики контроллеров,
    //// наследуясь от фабрики используемой по умолчанию
    //public class NinjectControllerFactory : DefaultControllerFactory
    //{
    //    private IKernel ninjectKernel;
    //    public NinjectControllerFactory()
    //    {
    //        // создание контейнера
    //        ninjectKernel = new StandardKernel();
    //        AddBindings();
    //    }
    //    protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
    //    {
    //        // получение объекта контроллера из контейнера
    //        // используя его тип
    //        return controllerType == null
    //        ? null
    //        : (IController)ninjectKernel.Get(controllerType);
    //    }
    //    private void AddBindings()
    //    {
    //        // конфигурирование контейнера
    //        ninjectKernel.Bind<IMT>().To<EFMetallurgTrans>();
    //        ninjectKernel.Bind<ISAP>().To<EFSAP>();
    //        ninjectKernel.Bind<ITKIS>().To<EFTKIS>();
    //        ninjectKernel.Bind<IRailWay>().To<EFRailWay>();
    //        ninjectKernel.Bind<EFTD.Abstract.IRepository<EFTD.Entities.MarriageWork>>().To<EFOC.Concrete.EFMarriageWork>();    
    //    }
    //}
}
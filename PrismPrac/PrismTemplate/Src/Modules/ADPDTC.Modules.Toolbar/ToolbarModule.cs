using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Regions;

namespace ADPDTC.Modules.Toolbar
{
    public class ToolbarModule : IModule
    {
        IUnityContainer _container;
        IRegionManager _regionManager;
        IDependencyInjectionExample _idie;

        public ToolbarModule(IUnityContainer container, IRegionManager regionManager, IDependencyInjectionExample idie)
        {
            _container=container;
            _regionManager=regionManager;
            _idie = idie;
            _idie.val = 5;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("ToolbarArea", typeof(Toolbar.Views.ToolbarView));
        }
    }

    public interface IDependencyInjectionExample
    {
        int val { get; set; }
    }

    public class DependencyInjectionExample : IDependencyInjectionExample
    {
        public int val { get; set; }
    }
}

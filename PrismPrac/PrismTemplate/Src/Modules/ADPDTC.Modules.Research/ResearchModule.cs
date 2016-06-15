using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Regions;
using ADPDTC.Modules.Toolbar;


namespace ADPDTC.Modules.Research
{
    public class ResearchModule : IModule
    {
        IUnityContainer _container;
        IRegionManager _regionManager;
        IDependencyInjectionExample _idie;


        public ResearchModule(IUnityContainer container, IRegionManager regionManager, IDependencyInjectionExample idie)
        {
            _container = container;
            _regionManager = regionManager;
            _idie = idie;
            
            
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("ContentArea", typeof(Research.Views.ResearchView));
        }
    }

    
}

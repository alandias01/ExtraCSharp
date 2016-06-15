using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.UnityExtensions;
using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.ServiceLocation;
using ADPDTC.Modules.Toolbar;

namespace ADPDTC
{
    public class Bootstrapper : UnityBootstrapper
    {
        
        protected override System.Windows.DependencyObject CreateShell()
        {
            this.Container.RegisterType<IDependencyInjectionExample, DependencyInjectionExample>(new ContainerControlledLifetimeManager());  //lifetime lets you use the same object everywhere.  Ex. in toolbar and research module.  Watch val=5
            return this.Container.Resolve<Shell>();
            //return ServiceLocator.Current.GetInstance<Shell>();
            
        }


        protected override void InitializeShell()
        {
            //base.InitializeShell();
            App.Current.MainWindow = (Window)Shell;
            App.Current.MainWindow.Show();
        }

        protected override void ConfigureModuleCatalog()
        {
            //base.ConfigureModuleCatalog();
            ModuleCatalog _moduleCatalog = (ModuleCatalog)this.ModuleCatalog;
            _moduleCatalog.AddModule(typeof(ADPDTC.Modules.Toolbar.ToolbarModule));
            _moduleCatalog.AddModule(typeof(ADPDTC.Modules.Research.ResearchModule));
            
            
        }
    }
}

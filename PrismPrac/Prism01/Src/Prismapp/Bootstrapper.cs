﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Modularity;
using System.Windows;
using Microsoft.Practices.Prism.Regions;
using System.Windows.Controls;
using Prismapp.Infrastructure;

namespace Prismapp
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override System.Windows.DependencyObject CreateShell()
        {           
            return this.Container.Resolve<Shell>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();
            App.Current.MainWindow = (Window)Shell;
            App.Current.MainWindow.Show();
        }


        //All the modules needed to be loaded at runtime are defined in the modulecatalog
        //
        protected override void ConfigureModuleCatalog()
        {
            /* Implementation 1
            base.ConfigureModuleCatalog();
            ModuleCatalog moduleCatalog = (ModuleCatalog)this.ModuleCatalog;
            moduleCatalog.AddModule(typeof(ModuleA.ModuleAModule));
            */

            //Implementation 2
            Type moduleAType = typeof(ModuleA.ModuleAModule);
            ModuleCatalog.AddModule(new ModuleInfo() 
            {
                ModuleName=moduleAType.Name,
                ModuleType=moduleAType.AssemblyQualifiedName,
                InitializationMode=InitializationMode.WhenAvailable
            });

        }

        //Only used for RegionAdapter.  stackpanelregionadapter
        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            RegionAdapterMappings mappings = base.ConfigureRegionAdapterMappings();
            mappings.RegisterMapping(typeof(StackPanel), Container.Resolve<StackPanelRegionAdapter>());
            return mappings;
        }
    }
}

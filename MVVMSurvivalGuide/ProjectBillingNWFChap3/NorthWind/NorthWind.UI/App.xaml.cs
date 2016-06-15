using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace NorthWind.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        /* Use this if you dont want conn string in the app.config file
        // Code to update DataDirectory
        protected override void OnStartup(StartupEventArgs e)
        {
            string dataDirectory = @"C:\Code\Northwind\Northwind.Data";
            AppDomain.CurrentDomain.SetData("DataDirectory", dataDirectory);
            base.OnStartup(e);
        }
        */

    }
}

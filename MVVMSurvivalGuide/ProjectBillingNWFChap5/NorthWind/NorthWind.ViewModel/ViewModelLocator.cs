using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NorthWind.Application;
using System.ComponentModel;
using System.Windows;

namespace NorthWind.ViewModel
{
    public class ViewModelLocator
    {
        private static MainWindowViewModel _mainWindowViewModel;
        public static MainWindowViewModel MainWindowViewModelStatic
        {
            get
            {
                //This parts gets rid of the below XAML Error
                //ArgumentException was thrown on "StaticExtention": Exception has been thrown by the target of an invocation
                if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                    return null;
                
                if (_mainWindowViewModel == null)
                {
                    _mainWindowViewModel = new MainWindowViewModel(new UIDataProvider());
                }
                return _mainWindowViewModel;
            }
        }

    }
}

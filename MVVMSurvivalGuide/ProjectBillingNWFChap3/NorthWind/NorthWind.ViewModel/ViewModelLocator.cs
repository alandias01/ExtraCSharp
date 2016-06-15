using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NorthWind.Application;

namespace NorthWind.ViewModel
{
    public class ViewModelLocator
    {
        private static MainWindowViewModel _mainWindowViewModel;
        public static MainWindowViewModel MainWindowViewModelStatic
        {
            get
            {
                if (_mainWindowViewModel == null)
                {
                    _mainWindowViewModel = new MainWindowViewModel(new UIDataProvider());
                }
                return _mainWindowViewModel;
            }
        }

    }
}

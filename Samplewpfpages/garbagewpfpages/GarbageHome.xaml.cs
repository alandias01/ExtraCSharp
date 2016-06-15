using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace garbagewpfpages
{
    /// <summary>
    /// Interaction logic for GarbageHome.xaml
    /// </summary>
    public partial class GarbageHome : Page
    {
        public GarbageHome()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GarbageReportPage grp = new GarbageReportPage();
            this.NavigationService.Navigate(grp);
        }
    }
}

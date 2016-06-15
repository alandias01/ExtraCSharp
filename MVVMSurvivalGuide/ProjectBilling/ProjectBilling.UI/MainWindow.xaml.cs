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
using ProjectBilling.Application;
using ProjectBilling.DataAccess;



namespace ProjectBilling.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IProjectsModel _projectModel;

        public MainWindow()
        {
            InitializeComponent();
            _projectModel = new ProjectsModel(new DataServiceStub());
        }

        private void ShowProjectsButton_Click(object sender, RoutedEventArgs e)
        {
            
            ProjectsView view = new ProjectsView();
            view.DataContext = new ProjectsViewModel(_projectModel);
            view.Owner = this;
            view.Show();

        }
    }
}

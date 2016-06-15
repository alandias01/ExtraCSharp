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
using System.Windows.Shapes;

namespace Pledger.View
{
    /// <summary>
    /// Interaction logic for ManualPledgeViewStatus.xaml
    /// </summary>
    public partial class ViewStatus : Window
    {
        
        public ViewStatus()
        {
            InitializeComponent();
        }

        public void LoadWindow<TG, BG>(IDataViewStatus<TG, BG> ds)
        {
            dgManualPledge.ItemsSource = ds.GetTopGrid();
            dgManualUnpledge.ItemsSource = ds.GetBottomGrid();
            
            labelPledgeTotalAmtVal.Content = ds.GetTopTotal();
            labelUnpledgeTotalAmtVal.Content = ds.GetBottomTotal();
 
        }

        
        private void ColumnsToRemove(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "RealTimePosition")
            {
                e.Cancel = true;
            } 
        }
        
    }





}

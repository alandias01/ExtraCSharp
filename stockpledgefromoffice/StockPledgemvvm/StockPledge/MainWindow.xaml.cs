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
using Microsoft.Windows.Controls.Ribbon;
using Utils;
using Pledger.View;

namespace StockPledge
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        MWViewmodel MWV = new MWViewmodel();
        
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = MWV;            
        }



        private void MyRibbon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*
            this.MainArea.Children.Clear();
            string s = (e.AddedItems[0] as RibbonTab).Name.ToString();
            if (s == PledgerHomeTab.Name)
            {

                //if (APV == null)
                //{
                //    APV = new AutoPledgeView();
                //    APV.LoadRibbonWindowEvent += (a) => { this.txtblkStockBalance.Text = a.ToString(); };
                //}
                
            }
            */
        }


    }
}

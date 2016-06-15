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
using Krs.Ats.IBNet;
using Krs.Ats.IBNet.Contracts;
namespace garbageTWS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<String> stockList { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            LoadStockList();

            IBClient client = new IBClient();

            MarketData MD = new MarketData(client);
                        

        }

        

        private void LoadStockList()
        {
            stockList = new List<string>();
            stockList.Add("SPY");
            stockList.Add("SH");
            stockList.Add("LINE");
            stockList.Add("LRE");
            stockList.Add("GLD");
            stockList.Add("DIA");
            stockList.Add("DOG");
            stockList.Add("GLL");
            stockList.Add("DGZ");
            stockList.Add("GDX");
            stockList.Add("NLR"); 
        }
    }

    
}

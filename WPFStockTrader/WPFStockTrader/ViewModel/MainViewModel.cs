using GalaSoft.MvvmLight;
using WPFStockTrader.Model;
using Stock.Data;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using System.Windows;

namespace WPFStockTrader.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        
        public ObservableCollection<StockObject> StockQuotes { get; set; }

        public ObservableCollection<StockObject> PortfolioPositions { get; set; }

        public RelayCommand BuyStockCommand { get; set; }

        public string SelectedSymbol { get; set; }

        public MainViewModel(IDataService dataService)
        {
            StockQuotes = new ObservableCollection<StockObject>();
            StockQuotes = new StockDa(true).Positions;
            PortfolioPositions = new ObservableCollection<StockObject>();

            BuyStockCommand = new RelayCommand(Buy_BuyButtonExecuteCommand, () => { return true; });
        }

        public void Buy_BuyButtonExecuteCommand()
        {
            MessageBox.Show(SelectedSymbol);
        }
        
        
    }
}




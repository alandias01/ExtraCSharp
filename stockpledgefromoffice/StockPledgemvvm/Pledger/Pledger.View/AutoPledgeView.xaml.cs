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
using Pledger.Data;
using Utils;
using Pledger.Data.Blob.Slate;
using Pledger.Data.Blob.DTC;

namespace Pledger.View
{
    /* Uncomment the calculate methods in runreal
     * Undo changes in the realtimeposition stored proc
     * buffer is hardcoded
     */

    /// <summary>
    /// Interaction logic for AutoPledgeView.xaml
    /// </summary>
    public partial class AutoPledgeView : UserControl
    {        
        List<spAlanGetRealTimePositions_Pledger_Result> RealTimePos = new List<spAlanGetRealTimePositions_Pledger_Result>();
        List<Pledge> StocksUnpledged = new List<Pledge>();
        List<Pledge> StocksPledged = new List<Pledge>();

        public event Action<double?> LoadRibbonWindowEvent;

        double buffer = .7;
        

        public AutoPledgeView( )
        {
            InitializeComponent();

            DGMenu<DGStocksToUnpledge> DGMStocksToUnpledge = new DGMenu<DGStocksToUnpledge>();
            DGMStocksToUnpledge.SetGrid(dgvStocksToUnpledge);

            DGMenu<Pledge> DGMStocksUnpledged = new DGMenu<Pledge>();
            DGMStocksUnpledged.SetGrid(dgvStocksUnpledged);

            DGMenu<DGStocksToPledge> DGMStocksToPledge = new DGMenu<DGStocksToPledge>();
            DGMStocksToPledge.SetGrid(dgvStocksToPledge);

            DGMenu<Pledge> DGMStocksPledged = new DGMenu<Pledge>();
            DGMStocksPledged.SetGrid(dgvStocksPledged);

            LoadRealData();
            
        }


        private void LoadRealData()
        {
            PledgeDA SDA = new PledgeDA();
            //SDA.LoadUnpledged(StocksUnpledged, DateTime.Today);
            //dgvStocksUnpledged.ItemsSource = StocksUnpledged;
            //
            //SDA.LoadPledged(StocksPledged, DateTime.Today);
            //dgvStocksPledged.ItemsSource = StocksPledged;
        }

        private void RunReal()
        {
            spAlanGetRealTimePositions_Pledger_ResultDA DDA = new spAlanGetRealTimePositions_Pledger_ResultDA();
            DDA.Load(RealTimePos,"0269");
            RealTimePos = RealTimePos.Where(x => x.Price != null).ToList();
            LoadRibbonWindowEvent(RealTimePos.Sum(x => x.pledgedquantity.Value * Convert.ToDouble(x.Price.Value) * buffer));

            List<OCCStockToUnpledge> stocksToUnpledge = new List<OCCStockToUnpledge>();
            stocksToUnpledge = CalculateWhatToUnpledgeReal();
            CalculateWhatToPledgeReal(stocksToUnpledge);
        }

        private List<OCCStockToUnpledge> CalculateWhatToUnpledgeReal()
        {
            List<OCCStockToUnpledge> stocksToUnpledge = new List<OCCStockToUnpledge>();
            RealTimePos.Where(r => r.TodaysNet < 0 && Math.Abs(r.TodaysNet.Value) > r.unpledgedquantity && r.pledgedquantity > 0)
                .ToList()
                .ForEach(x => stocksToUnpledge.Add(new OCCStockToUnpledge(x)));
            
            dgvStocksToUnpledge.ItemsSource = (from i in stocksToUnpledge select new DGStocksToUnpledge() { CUSIP = i.RealTimePosition.cusip, SharesToUnpledge = i.SharesToUnpledge, TodaysNet = i.RealTimePosition.TodaysNet }).ToList();
                        
            return stocksToUnpledge;
        }

        private void CalculateWhatToPledgeReal(List<OCCStockToUnpledge> stocksToUnpledge)
        {
            int req = 0;
            if (!int.TryParse(textBoxRequirement.Text, out req))
            {
                MessageBox.Show("Requirement could not be understood as a number");
                return;
            }
            
            List<StartingPositionView> StartingPositions = new List<StartingPositionView>();
            StartingPositionViewDA SPVDA = new StartingPositionViewDA();
            SPVDA.Load(StartingPositions);
            List<OCCStockPledge> pledgeableStocks = new List<OCCStockPledge>();
                        
            var BorrowCusips = StartingPositions.Where(x => x.BorrowLoanIndicator.ToLower() == "b").Select(y => y.Cusip).Distinct().ToList();

            foreach (spAlanGetRealTimePositions_Pledger_Result pos in RealTimePos)
            {
                if (!BorrowCusips.Any(x => x.ToLower() == pos.cusip.ToLower()))
                {
                    if (pos.TodaysNet >= 0)
                    {
                        pledgeableStocks.Add(new OCCStockPledge(pos));
                    }
                    else if (pos.TodaysNet < 0 && pos.unpledgedquantity > Math.Abs(pos.TodaysNet.Value))
                    {
                        pledgeableStocks.Add(new OCCStockPledge(pos));
                    }
                }
            }
            
            //Amt we need to pledge =  Requirement - (Find sum of what we have pledged - What we are going to unpledge)

            decimal AmtWeHavePledged = RealTimePos.Where(x => x.Price != null).Sum(x => x.pledgedquantity * x.Price).Value * (decimal)buffer;
            decimal AmtWeAreGoingToUnpledge = stocksToUnpledge.Sum(y => y.SharesToUnpledge * y.RealTimePosition.Price).Value;
            decimal WhatWeNeedToPledge = req - (AmtWeHavePledged - AmtWeAreGoingToUnpledge);

            decimal current = 0;
            List<OCCStockPledge> StocksToPledge = new List<OCCStockPledge>();
            foreach (var p in pledgeableStocks)
            {
                decimal mktval = p.PledgeableShares * p.RealTimePosition.Price.Value;
                decimal valueNeeded = 0;

                if ((current + mktval) > WhatWeNeedToPledge)
                {
                    valueNeeded = WhatWeNeedToPledge - current;
                    p.QuantityToPledge = Convert.ToInt32(Math.Round(((valueNeeded / p.RealTimePosition.Price.Value) + 1), 0));
                    StocksToPledge.Add(p);
                    break;
                }

                else
                {
                    current += mktval;
                    p.QuantityToPledge = p.PledgeableShares;
                    StocksToPledge.Add(p);
                }
            }

            dgvStocksToPledge.ItemsSource = (from i in StocksToPledge select new DGStocksToPledge() { cusip = i.RealTimePosition.cusip, QuantityToPledge = i.QuantityToPledge, Price = i.RealTimePosition.Price }).ToList();
        }

        
        private void buttonCalc_Click(object sender, RoutedEventArgs e)
        {
            RunReal();
        }
    }

    

    

    //Classes created so we can use the utils class.  Needs a non anonymous type
    public class DGStocksToUnpledge
    {
        public string CUSIP { get; set; }
        public int SharesToUnpledge { get; set; }
        public int? TodaysNet { get; set; }
    }

    public class DGStocksToPledge
    {
        public string cusip { get; set; }
        public int QuantityToPledge { get; set; }
        public decimal? Price { get; set; }

    }
        
}

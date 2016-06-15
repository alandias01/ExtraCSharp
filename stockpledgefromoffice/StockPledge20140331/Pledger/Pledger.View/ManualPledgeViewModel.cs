using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFUtils;
using Data.DTC;
using LoadingControl.Control;
using System.ComponentModel;
using System.Windows;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Pledger.Data;

namespace Pledger.View
{
    public class ManualPledgeViewModel : NPC, IPledgeViewModel
    {
        public Command RefreshManualPledgeUnpledgeCommand { get; set; }
        public Command PledgeUnpledgeCommand { get; set; }
        
        spAlanGetRealTimePositions_Pledger_ResultDA DDA = new spAlanGetRealTimePositions_Pledger_ResultDA();
        public ObservableCollection<spAlanGetRealTimePositions_Pledger_Result> RealTimePos { get; set; }
        
        LoadingAnimation LA = new LoadingAnimation();

        public ManualPledgeViewModel()
        {
            RealTimePos = new AsyncObservableCollection<spAlanGetRealTimePositions_Pledger_Result>();
            
            RefreshManualPledgeUnpledgeCommand=new Command(LoadManualPledgeViewData);
            PledgeUnpledgeCommand = new Command(PledgeUnpledge);
            
            LoadManualPledgeViewData(null);
        }

        public void LoadManualPledgeViewData(object o)
        {
            //LoadAnim();
            RealTimePos.Clear();

            Task.Factory.StartNew(() =>
            {
                DDA.Load(RealTimePos, "0269");
            });

        }

        public void PledgeUnpledge(object o) 
        {
            var UnpledgeList = RealTimePos.Where(x => x.SharesYouWantToUnpledge > 0).ToList();
            var PledgeList = RealTimePos.Where(x => x.SharesYouWantToPledge > 0).ToList();

            
            //Get Cusips w/o a price
            List<string> StockswoPriceArr1 = PledgeList.Where(x => x.Price == null).Select(x => x.cusip).ToList();
            List<string> StockswoPriceArr2 = UnpledgeList.Where(x => x.Price == null).Select(x => x.cusip).ToList();
            
            List<string> StockswoPriceArr3 = StockswoPriceArr1.Concat(StockswoPriceArr2).ToList();

            if (StockswoPriceArr3.Count>0)
            {
                string StockswoPriceString = string.Join(System.Environment.NewLine, StockswoPriceArr3);
                MessageBox.Show("Cannot pledge since you selected stocks with no price available" + System.Environment.NewLine + StockswoPriceString);
                return;
            }

            List<OCCStockPledge> StocksToPledge = new List<OCCStockPledge>();
            List<OCCStockToUnpledge> StocksToUnpledge = new List<OCCStockToUnpledge>();

            UnpledgeList.ForEach(x => StocksToUnpledge.Add(new OCCStockToUnpledge(x, x.SharesYouWantToUnpledge)));
            PledgeList.ForEach(x => StocksToPledge.Add(new OCCStockPledge(x, x.SharesYouWantToPledge)));


            ViewStatus MPVS = new ViewStatus();

            ManualPledgeDataViewStatus idvs = new ManualPledgeDataViewStatus(StocksToPledge, StocksToUnpledge);
            
            MPVS.LoadWindow(idvs);
            MPVS.Show();
            
            //pledges/unpledges the stocks
            //new PledgeProcessor().PledgeListOfStocks(StocksToPledge, true, true);
            //new PledgeProcessor().UnpledgeListOfStocks(StocksToUnpledge, true, true);
        }

        


        public void LoadAnim()
        {
            LA.VerticalAlignment = VerticalAlignment.Center;
            LA.HorizontalAlignment = HorizontalAlignment.Center;
            //Grid.SetRow(LA, 1);
            //DGArea.Children.Add(LA);
        }

        public void UnloadAnim()
        {
            //DGArea.Children.Remove(LA);
        }


    }




}

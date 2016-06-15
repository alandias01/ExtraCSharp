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
using System.ComponentModel;

using Pledger.Data;
using WPFUtils;
using LoadingControl.Control;
using Data.Slate;
using Data.DTC;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Pledger.View
{
    /* 
     * Undo changes in the realtimeposition stored proc.  I commented out looking at incoming deliveryorders and pledgeorders so it's as if no 
     * activity was done in the morning.  Otherwise, I would have to test before Ops pledges in the morning, which is around 7am
     * 
     * Buffer is hardcoded
     * Remove this //alantesting BufferAmount = 0;
     * uncomment this //HCRDA.Update(HCItem);
     */



    public partial class AutoPledgeView : UserControl
    {
        List<spAlanGetRealTimePositions_Pledger_Result> RealTimePos = new List<spAlanGetRealTimePositions_Pledger_Result>();

        HedgeCashRequirementDA HCRDA = new HedgeCashRequirementDA();
        List<hedgecashrequirementView_AlanPledger> HCRVList = new List<hedgecashrequirementView_AlanPledger>();
        public hedgecashrequirementView_AlanPledger HCRVItem { get; set; }

        HedgeCashEventArgs he;

        List<OCCStockToUnpledge> stocksToUnpledge;
        List<OCCStockPledge> stocksToPledge;

        double StockBalanceFactor = .7;

        public decimal Buffer { get; set; }


        LoadingAnimation LA1 = new LoadingAnimation();
        LoadingAnimation LA2 = new LoadingAnimation();
        LoadingAnimation LA3 = new LoadingAnimation();
        LoadingAnimation LA4 = new LoadingAnimation();

        public event AutoPledgeEventHandler AutoPledgeCalculated;
        public event HedgeCashEventHandler HedgeCashDataLoaded;

        public AutoPledgeView()
        {
            InitializeComponent();
            this.DataContext = this;
            CheckIfPledgedToday();
            LoadRequirementAndCashBalance();
            #region DataGrid rightClick options
            DGMenu<DGStocksToUnpledge> DGMStocksToUnpledge = new DGMenu<DGStocksToUnpledge>();
            DGMStocksToUnpledge.SetGrid(dgvStocksToUnpledge);

            DGMenu<Pledge> DGMStocksUnpledged = new DGMenu<Pledge>();
            DGMStocksUnpledged.SetGrid(dgvStocksUnpledged);

            DGMenu<DGStocksToPledge> DGMStocksToPledge = new DGMenu<DGStocksToPledge>();
            DGMStocksToPledge.SetGrid(dgvStocksToPledge);

            DGMenu<Pledge> DGMStocksPledged = new DGMenu<Pledge>();
            DGMStocksPledged.SetGrid(dgvStocksPledged);
            #endregion

            LoadTodaysPledgedAndUnpledgedData();

            Buffer = .3m;

            AutoPledgeCalculated += (s, e) =>
            {
                textBoxStockBalance.Text = e.StockBalance.ToString("c");
                textBoxTotalPledged.Text = e.TotalPledged.ToString("c");
            };

        }

        private void CheckIfPledgedToday()
        {
            List<PledgeStatus> ps = new List<PledgeStatus>();
            PledgeStatusDA psd = new PledgeStatusDA();
            psd.Load(ps);

            PledgeStatus UnpledgeItem = ps.SingleOrDefault(x => x.PledgeType.ToLower() == "u");

            if (DateTime.Today <= UnpledgeItem.DateSent.Date)
            {
                btnUnpledge.IsEnabled = false;
                labelUnpledgeStatus.Content = "Unpledged by " + UnpledgeItem.SentBy + " at " + UnpledgeItem.DateSent;
            }



        }

        private void LoadRequirementAndCashBalance()
        {
            HCRVList.Clear();
            HCRDA.Load(HCRVList, DateTime.Today.AddDays(0));

            if (HCRVList.Count == 1)
            {
                HCRVItem = HCRVList[0];
                textBlockCashBalance.Text = HCRVItem.CashBalance.ToString("n");
                textBlockRequirement.Text = HCRVItem.Requirement.ToString("n");
            }
        }

        public void LoadHedgeCashData(DateTime dt)
        {
            try
            {
                HedgeCollateralViewDA HCDA = new HedgeCollateralViewDA();

                List<HedgeCollateralView_AlanPledger> HCVList = new List<HedgeCollateralView_AlanPledger>();
                HCDA.Load(HCVList, dt);

                List<HedgeMarkTotalView_AlanPledger> HMTVList = new List<HedgeMarkTotalView_AlanPledger>();
                HedgeMarkTotalViewDA HMTVDA = new HedgeMarkTotalViewDA();
                HMTVDA.Load(HMTVList, dt);

                double markTotal = 0;
                HMTVList.ForEach(x => markTotal += x.Total);

                double borrowPosition5239 = 0;
                double borrowPosition269 = 0;
                double loanPosition5239 = 0;
                double loanPosition269 = 0;

                foreach (var h in HCVList)
                {
                    if (h.BorrowLoan == "B")
                    {
                        if (h.ClearingNo == "05239")
                        {
                            borrowPosition5239 = h.total.Value;
                        }
                        if (h.ClearingNo == "00269")
                        {
                            borrowPosition269 = h.total.Value;
                        }
                    }
                    else if (h.BorrowLoan == "L")
                    {
                        if (h.ClearingNo == "05239")
                        {
                            loanPosition5239 = h.total.Value;
                        }
                        if (h.ClearingNo == "00269")
                        {
                            loanPosition269 = h.total.Value;
                        }
                    }
                }

                borrowPosition5239 *= -1;
                borrowPosition269 *= -1;

                double spread269 = borrowPosition269 + loanPosition269;
                double spread5239 = borrowPosition5239 + loanPosition5239;

                double difference = (spread269 + spread5239);
                double hedgeMark = markTotal;
                double hedgeSpread = difference - hedgeMark;

                HedgeCashEventArgs he = new HedgeCashEventArgs();
                he.borrowPosition269 = borrowPosition269;
                he.borrowPosition5239 = borrowPosition5239;
                he.loanPosition269 = loanPosition269;
                he.loanPosition5239 = loanPosition5239;
                he.spread269 = spread269;
                he.spread5239 = spread5239;
                he.difference = difference;
                he.hedgeMark = hedgeMark;
                he.hedgeSpread = hedgeSpread;

                onHedgeCashCalculated(he);
            }
            catch (Exception e)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    MessageBox.Show("Message: " + e.Message + Environment.NewLine + "Inner Exception: ");
                }));

                Utils.LogError("Hedge Cash Issue " + e.Message + Environment.NewLine + "Inner Exception: ");
            }
        }

        private void LoadTodaysPledgedAndUnpledgedData()
        {
            PledgeDA SDA = new PledgeDA();

            List<Pledge> StocksUnpledged = new List<Pledge>();
            List<Pledge> StocksPledged = new List<Pledge>();

            //Set up as binding
            SDA.LoadUnpledged(StocksUnpledged, DateTime.Today);
            dgvStocksUnpledged.ItemsSource = StocksUnpledged;

            SDA.LoadPledged(StocksPledged, DateTime.Today);
            dgvStocksPledged.ItemsSource = StocksPledged;
        }





        private void LoadAnim()
        {
            LA1.VerticalAlignment = VerticalAlignment.Center;
            LA1.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.SetColumn(LA1, 0);
            Grid.SetRow(LA1, 1);
            DGAutopledgeArea.Children.Add(LA1);

            LA2.VerticalAlignment = VerticalAlignment.Center;
            LA2.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.SetColumn(LA2, 0);
            Grid.SetRow(LA2, 2);
            DGAutopledgeArea.Children.Add(LA2);

            LA3.VerticalAlignment = VerticalAlignment.Center;
            LA3.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.SetColumn(LA3, 1);
            Grid.SetRow(LA3, 1);
            DGAutopledgeArea.Children.Add(LA3);

            LA4.VerticalAlignment = VerticalAlignment.Center;
            LA4.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.SetColumn(LA4, 1);
            Grid.SetRow(LA4, 2);
            DGAutopledgeArea.Children.Add(LA4);

        }

        private void UnloadAnim()
        {
            DGAutopledgeArea.Children.Remove(LA1);
            DGAutopledgeArea.Children.Remove(LA2);
            DGAutopledgeArea.Children.Remove(LA3);
            DGAutopledgeArea.Children.Remove(LA4);
        }

        private void Calculate()
        {
            double dblreq = 0;
            int req = 0;
            if (!double.TryParse(textBlockRequirement.Text, out dblreq))
            {
                MessageBox.Show("Requirement could not be understood as a number");
                return;
            }
            else
            {
                try
                {
                    req = Convert.ToInt32(dblreq);
                }
                catch (Exception e)
                {
                    MessageBox.Show("There was an issue converting the requirement to an integer");
                    throw;
                }


            }

            if (HCRVItem == null)
            {
                if (!string.IsNullOrEmpty(textBlockRequirement.Text))
                {

                    hedgecashrequirementView_AlanPledger HCItem = new hedgecashrequirementView_AlanPledger();
                    double dreq = 0;
                    if (double.TryParse(textBlockRequirement.Text, out dreq))
                    {
                        HCItem.Requirement = dreq;
                        //HCRDA.Update(HCItem);
                    }
                }

            }


            LoadAnim();

            BackgroundWorker BW = new BackgroundWorker();
            BW.DoWork += (s, e) =>
            {
                spAlanGetRealTimePositions_Pledger_ResultDA DDA = new spAlanGetRealTimePositions_Pledger_ResultDA();
                RealTimePos.Clear();
                DDA.Load(RealTimePos, "0269");

                //Check business rule...Todays net >=0 but we may not have any of that stock Used in getting pledgeable stock
                //var tst = RealTimePos.Where(x => x.TodaysNet > 0 && x.unpledgedquantity <= 0).ToList();

                stocksToUnpledge = CalculateWhatToUnpledge();

                //alandias testing 
                //stocksToUnpledge = stocksToUnpledge.Where(x => x.SharesToUnpledge == 1).Take(1).ToList(); 


                stocksToPledge = CalculateWhatToPledge(stocksToUnpledge, req);

                //alandias testing 
                //stocksToPledge = stocksToPledge.Where(x => x.QuantityToPledge == stocksToPledge.Min(y => y.QuantityToPledge)).ToList();
            };

            BW.RunWorkerCompleted += (s, e) =>
            {
                UnloadAnim();
                AutoPledgeEventArgs ae = new AutoPledgeEventArgs();

                ae.StockBalance = RealTimePos.Where(x => x.Price != null).Sum(x => x.pledgedquantity.Value * Convert.ToDouble(x.Price.Value) * StockBalanceFactor);
                ae.TotalPledged = RealTimePos.Where(x => x.Price != null).Sum(x => x.pledgedquantity * x.Price).Value;
                onAutoPledgeCalculated(ae);

                dgvStocksToUnpledge.ItemsSource = (from i in stocksToUnpledge select new DGStocksToUnpledge() { CUSIP = i.RealTimePosition.cusip, SharesToUnpledge = i.SharesToUnpledge, TodaysNet = i.RealTimePosition.TodaysNet }).ToList();

                dgvStocksToPledge.ItemsSource = (from i in stocksToPledge select new DGStocksToPledge() { cusip = i.RealTimePosition.cusip, QuantityToPledge = i.QuantityToPledge, Price = i.RealTimePosition.Price }).ToList();
            };

            BW.RunWorkerAsync();
        }



        private List<OCCStockToUnpledge> CalculateWhatToUnpledge()
        {
            List<OCCStockToUnpledge> stocksToUnpledge = new List<OCCStockToUnpledge>();
            RealTimePos.Where(r => r.TodaysNet < 0 && Math.Abs(r.TodaysNet.Value) > r.unpledgedquantity && r.pledgedquantity > 0)
                .ToList()
                .ForEach(x => stocksToUnpledge.Add(new OCCStockToUnpledge(x)));

            return stocksToUnpledge;
        }

        private List<OCCStockPledge> CalculateWhatToPledge(List<OCCStockToUnpledge> stocksToUnpledge, int req)
        {
            /* 1. Get starting position to know which of our inventory are borrows so we don't pledge those
             * 2. Remove stocks that are on the unpledgeable list
             * 3. Calculate the amt we need to pledge (The required amt needed in OCC - (Amt we have pledged already - Amt we are going to unpledge)
             * 4. Mult amt we need to pledge by a buffer
             * 5. Now choose stocks we can pledge
             */
            List<StartingPositionView> StartingPositions = new List<StartingPositionView>();
            StartingPositionViewDA SPVDA = new StartingPositionViewDA();
            SPVDA.Load(StartingPositions);
            List<OCCStockPledge> pledgeableStocks = new List<OCCStockPledge>();

            //1.
            var BorrowCusips = StartingPositions.Where(x => x.BorrowLoanIndicator.ToLower() == "b").Select(y => y.Cusip).Distinct().ToList();

            foreach (spAlanGetRealTimePositions_Pledger_Result pos in RealTimePos)
            {
                if (!BorrowCusips.Any(x => x.ToLower() == pos.cusip.ToLower()) && pos.Price != null && pos.Price > 10)
                {
                    if (pos.TodaysNet >= 0 && pos.unpledgedquantity > 0)
                    {
                        pledgeableStocks.Add(new OCCStockPledge(pos));
                    }
                    else if (pos.TodaysNet < 0 && pos.unpledgedquantity > Math.Abs(pos.TodaysNet.Value))
                    {
                        pledgeableStocks.Add(new OCCStockPledge(pos));
                    }
                }
            }

            //2.
            UnPledgeableDA upda = new UnPledgeableDA();
            List<UnPledgeableView_AlanPledger> UnPledgeable = new List<UnPledgeableView_AlanPledger>();
            upda.Load(UnPledgeable);
            pledgeableStocks.RemoveAll(x => UnPledgeable.Exists(y => y.Cusip == x.RealTimePosition.cusip));

            pledgeableStocks = pledgeableStocks.OrderByDescending(x => x.PledgeableShares * x.RealTimePosition.Price).ToList();

            //3.
            //Amt we need to pledge =  Requirement - (Find sum of what we have pledged - What we are going to unpledge)

            decimal AmtWeHavePledged = RealTimePos.Where(x => x.Price != null).Sum(x => x.pledgedquantity * x.Price).Value * (decimal)StockBalanceFactor;

            decimal AmtWeAreGoingToUnpledge = stocksToUnpledge.Where(p => p.RealTimePosition.Price != null).Sum(y => y.SharesToUnpledge * y.RealTimePosition.Price).Value;
            decimal WhatWeNeedToPledge = req - (AmtWeHavePledged - AmtWeAreGoingToUnpledge);

            //4.
            decimal BufferAmount = WhatWeNeedToPledge * Buffer;
            if (BufferAmount < 0)
            {
                BufferAmount = 0;
            }

            Dispatcher.BeginInvoke(new Action(() =>
            {
                labelAmtPledging.Content = WhatWeNeedToPledge.ToString("c0");
                labelBufferAmtPledging.Content = BufferAmount.ToString("c0");
            }));

            WhatWeNeedToPledge += BufferAmount;

            if (WhatWeNeedToPledge < 0)
            {
                WhatWeNeedToPledge = 0;
            }




            //5.
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

            return StocksToPledge;
        }

        #region Click Events
        private void buttonCalc_Click(object sender, RoutedEventArgs e)
        {
            Calculate();
        }

        public void btnUnpledge_Click(object sender, RoutedEventArgs e)
        {
            //below used for testing when data isn't available
            /*
            spAlanGetRealTimePositions_Pledger_Result spa = new spAlanGetRealTimePositions_Pledger_Result();
            spa.cusip = "123456789";
            spa.ParticipantNum = "0269";
            spa.Ticker = "test";
            OCCStockToUnpledge o = new OCCStockToUnpledge(spa);
            o.SharesToUnpledge = 1;
            stocksToUnpledge.Add(o);
            */

            new PledgeProcessor().UnpledgeListOfStocks(stocksToUnpledge, true, true);
        }

        private void btnPledge_Click(object sender, RoutedEventArgs e)
        {
            new PledgeProcessor().PledgeListOfStocks(stocksToPledge, true, true);
        }

        private void btnSendEmail_Click(object sender, RoutedEventArgs e)
        {
            Outlook.Application oApp = new Outlook.Application();
            Outlook._MailItem oMailItem = (Outlook._MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);

            //oMailItem.To = "hedgeforecast@mapleusa.com";
            oMailItem.To = "aland@mapleusa.com";

            oMailItem.Subject = "Hedge Cash Forecasting";
            oMailItem.BodyFormat = Outlook.OlBodyFormat.olFormatHTML;

            string htmlbody = CreateHTMLBody();

            oMailItem.HTMLBody = htmlbody;

            oMailItem.Display(false);
        }

        private void GetDataForSendEmailReport()
        {
 
        }


        #endregion

        private string CreateHTMLBody()
        {
            string htmlbody = @"
            <html xmlns=http://www.w3.org/1999/xhtml>
    <head> <title>Untitled Page</title>
        <style type=text/css>
            table{font-family:Verdana; width:40em; font-size:.9em;}            
            td{height:2.3em;width:20em;}            
            #c1{background-color:#EEEEEE;}            
            h2{color:White;}
            h3{color:White;}
        </style>
    </head>

    <body>
        
        <table  border=0 cellpadding=0 cellspacing=0>
            <tr height=60 bgcolor=444444>
                <td  colspan=2><h2> <center>HEDGE CASH FORECASTING</center></h2></td>                
            </tr>

            <tr bgcolor=666666>
                <td  colspan=2><h3>HEDGE COLLATERAL VALUES</h3></td>                
            </tr>

            <tr id=c1>
                <td>0269 Net Stock Loan Position Value</td> <td></td>
            </tr>

            <tr>
                <td>0269 Net Stock Borrow Position Value</td>  <td></td>
            </tr>

            <tr id=c1>
                <td><h4>0269 LOAN / BORROW SPREAD</h4></td>  <td></td>
            </tr>

            <tr>
                <td></td>  <td></td>
            </tr>

            <tr>
                <td>5239 Net Stock Loan Position Value</td> <td></td>
            </tr>

            <tr id=c1>
                <td>5239 Net Stock Borrow Position Value</td>  <td></td>
            </tr>

            <tr>
                <td><h4>5239 LOAN / BORROW SPREAD</h4></td>  <td></td>
            </tr>

            <tr>
                <td></td>  <td></td>
            </tr>

            <tr id=c1>
                <td>B/L Difference</td>  <td></td>
            </tr>

            <tr>
                <td>Today's Hedge Mark</td>  <td></td>
            </tr>

            <tr id=c1>
                <td><h4>NET HEDGE SPREAD:</h4></td> > <td></td>
            </tr>

            <tr>
                <td></td>  <td></td>
            </tr>


            <tr bgcolor=666666>
                <td  colspan=2><h3>CASH ON DEPOSIT</h3></td>                
            </tr>
            <tr>
                <td>beginning Cash Balance (CS)</td>  <td></td>
            </tr>
            <tr>
                <td>beginning Stock Balance (VS)</td>  <td></td>
            </tr>
            <tr>
                <td>total requirement</td>  <td></td>
            </tr>
            <tr>
                <td>excess ( + ) / deficit ( - )</td>  <td></td>
            </tr>

            <tr>
                <td><h4>ADDITIONAL RISK REQUIREMENT:</h4></td>  <td></td>
            </tr>

            <tr>
                <td>Total Pledged Stock Value</td>  <td></td>
            </tr>


        </table>
                
        
    </body>
</html>            

";

            return htmlbody;
        }

        #region Raising the Event
        public void onHedgeCashCalculated(HedgeCashEventArgs he)
        {
            if (HedgeCashDataLoaded != null)
            {
                HedgeCashDataLoaded(this, he);
            }
        }

        public void onAutoPledgeCalculated(AutoPledgeEventArgs e)
        {
            if (AutoPledgeCalculated != null)
            {
                AutoPledgeCalculated(this, e);
            }
        }

        #endregion

        #region KeyDown Events

        private void textBoxRequirement_KeyDown(object sender, KeyEventArgs e)
        {
            CalcEvent(e);
        }

        private void textBoxBuffer_KeyDown(object sender, KeyEventArgs e)
        {
            CalcEvent(e);
        }

        private void CalcEvent(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                buttonCalc.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        #endregion









    }

}

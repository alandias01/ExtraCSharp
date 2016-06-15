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
using WPFUtils;
using Pledger.View;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Media.Animation;


namespace StockPledge
{    
    public partial class MainWindow : RibbonWindow
    {   
        public Command ShowAutoPledgeViewCommand { get; set; }
        public Command ShowApexCommand { get; set; }
        
        public Command ShowManualPledgeViewCommand { get; set; }
        public Command ShowDragAndDropPledgeViewCommand { get; set; }

        AutoPledgeView APV;
        ManualPledgeView MPV;
        DragAndDropPledgeView DPV;

        const string CURR_FORMAT = "#,##0.##";

        //ApexView AV;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
                        
            ShowAutoPledgeViewCommand = new Command((s) =>
            {
                this.MainArea.Children.Clear();
                if (APV == null)
                {
                    APV = new AutoPledgeView();
                    
                    APV.HedgeCashDataLoaded += (s1, e) =>
                    {
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            txt269NetStockBorrowPositionValue.Text = e.borrowPosition269.ToString("c0");
                            txt269NetStockLoanPositionValue.Text = e.loanPosition269.ToString("c0");
                            txt269LoanBorrowSpread.Text = e.spread269.ToString("c0");

                            txt5239NetStockBorrowPositionValue.Text = e.borrowPosition5239.ToString("c0");
                            txt5239NetStockLoanPositionValue.Text = e.loanPosition5239.ToString("c0");
                            txt5239LoanBorrowSpread.Text = e.spread5239.ToString("c0");

                            txtBLDifference.Text = e.difference.ToString("c0");
                            txtTodaysHedgeMark.Text = e.hedgeMark.ToString("c0");
                            txtNetHedgeSpread.Text = e.hedgeSpread.ToString("c0");

                        }));
                    };

                    //I can't put below code in AutoPledgeView constructor because I need to subscribe to the event above first
                    Task.Factory.StartNew(() =>
                    {
                        APV.LoadHedgeCashData(Utils.GetLastTradeDate());
                    });                    
                                     

                }

                //alan dias 20140129
                /*
                DoubleAnimation a = new DoubleAnimation
                {
                    From = 0.0,
                    To = 2.0,
                    FillBehavior = FillBehavior.Stop,
                    BeginTime = TimeSpan.FromSeconds(2),
                    Duration = new Duration(TimeSpan.FromSeconds(0.5))
                };
                Storyboard storyboard = new Storyboard();

                storyboard.Children.Add(a);
                Storyboard.SetTarget(a, APV);
                Storyboard.SetTargetProperty(a, new PropertyPath(OpacityProperty));
                storyboard.Completed += delegate { APV.Visibility = System.Windows.Visibility.Visible; };
                APV.Visibility = Visibility.Hidden;

                storyboard.Begin();
                */
                this.MainArea.Children.Add(APV);    

            });


            ShowApexCommand = new Command((s) =>
            {

            });

            ShowManualPledgeViewCommand = new Command((s) =>
            {
                this.MainArea.Children.Clear();
                if (MPV == null)
                {
                    MPV = new ManualPledgeView();
                }
                this.MainArea.Children.Add(MPV);
            });

            ShowDragAndDropPledgeViewCommand = new Command((s) =>
            {
                this.MainArea.Children.Clear();
                if (DPV == null)
                {
                    DPV = new DragAndDropPledgeView();
                }
                this.MainArea.Children.Add(DPV);
            });

        }
        

        private void MyRibbon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            string s = (e.AddedItems[0] as RibbonTab).Name.ToString();
            
            if (s == PledgerHomeTab.Name)
            {
                ShowAutoPledgeViewCommand.Execute(null);                
            }

            /*
            else if (s == APEXHomeTab.Name)
            {
                if (AV == null)
                {
                    AV = new ApexView();
                }
                this.MainArea.Children.Add(AV);
            }
            */
        }

        
    }
}

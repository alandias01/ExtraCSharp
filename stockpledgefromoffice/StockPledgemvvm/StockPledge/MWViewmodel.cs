using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;
using Pledger.View;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace StockPledge
{
    
    public class MWViewmodel:NPC
    {

        public Command ShowAutoPledgeViewCommand { get; set; }
        public Command ShowApexCommand { get; set; }
        
        public Command ShowManualPledgeViewCommand { get; set; }
        public Command ShowDragAndDropPledgeViewCommand { get; set; }

        //Testing
        public Command ShowTestCommand { get; set; }
        public ObservableCollection<PledgeViewModel> PVMCollection { get; set; }

        public ObservableCollection<UserControl> UCCollection { get; set; }

        AutoPledgeView APV;
        ManualPledgeView MPV;
        DragAndDropPledgeView DPV;

        public string stockBalanceUI;
        public string StockBalanceUI
        {
            get
            {
                return stockBalanceUI;
            }
            set
            { 
                stockBalanceUI = value;
                OnPropertyChanged("StockBalanceUI");
            }
        }

        const string CURR_FORMAT = "#,##0.##";

        public MWViewmodel()
        {            
            UCCollection = new ObservableCollection<UserControl>();
            PVMCollection = new ObservableCollection<PledgeViewModel>();

            ShowAutoPledgeViewCommand = new Command((s) =>
            {
                UCCollection.Clear();
                if (APV == null)    //We check for null because we don't want to initialize a window everytime user presses the button
                {
                    APV = new AutoPledgeView();
                    APV.LoadRibbonWindowEvent += (a) => { StockBalanceUI = a.Value.ToString(CURR_FORMAT); };

                    
                }

                //UCCollection.Add(APV);
                PVMCollection.Clear();
                PVMCollection.Add(new TempPledgeViewModel());
            });

            
            
            ShowTestCommand = new Command(s =>
            {
                PVMCollection.Clear();
                PVMCollection.Add(new TempPledgeViewModel());
            });
            

            ShowApexCommand = new Command((s) =>
            {

            });

            ShowManualPledgeViewCommand = new Command((s) =>
            {
                UCCollection.Clear();
                if (MPV == null)
                {
                    MPV = new ManualPledgeView();
                }

                UCCollection.Add(MPV);                
            });

            ShowDragAndDropPledgeViewCommand = new Command((s) =>
            {
                UCCollection.Clear();
                if (DPV == null)
                {
                    DPV = new DragAndDropPledgeView();
                }

                UCCollection.Add(DPV);
            });
        }



        
    }
}

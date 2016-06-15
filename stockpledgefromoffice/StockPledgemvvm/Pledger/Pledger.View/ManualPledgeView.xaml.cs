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
using Utils;
using Pledger.Data;
using Pledger.Data.Blob.DTC;
using System.ComponentModel;
using LoadingControl.Control;

namespace Pledger.View
{
    /// <summary>
    /// Interaction logic for ManualPledgeView.xaml
    /// </summary>
    public partial class ManualPledgeView : UserControl
    {
        spAlanGetRealTimePositions_Pledger_ResultDA DDA = new spAlanGetRealTimePositions_Pledger_ResultDA();
        List<spAlanGetRealTimePositions_Pledger_Result> RealTimePos = new List<spAlanGetRealTimePositions_Pledger_Result>();


        public Command RefreshManualPledgeUnpledgeCommand { get; set; }

        public ManualPledgeView()
        {
            InitializeComponent();
            this.DataContext = this;
            
            DGMenu<spAlanGetRealTimePositions_Pledger_Result> dgm = new DGMenu<spAlanGetRealTimePositions_Pledger_Result>();
            dgm.SetGrid(dgvManualPledgeUnpledge);

            LoadingAnimation LA = new LoadingAnimation();
            LA.VerticalAlignment = VerticalAlignment.Center;
            LA.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.SetRow(LA, 1);
            DGArea.Children.Add(LA);

            BackgroundWorker BW = new BackgroundWorker();
            BW.DoWork += (s, e) =>
            {
                DDA.Load(RealTimePos,"0269");
            };

            BW.RunWorkerCompleted += (s, e) =>
            {
                DGArea.Children.Remove(LA);
                dgvManualPledgeUnpledge.ItemsSource = RealTimePos;
            };

            BW.RunWorkerAsync();
            
            RefreshManualPledgeUnpledgeCommand = new Command((s) => 
            {
                RealTimePos.Clear();
                DDA.Load(RealTimePos,"0269");
                dgvManualPledgeUnpledge.ItemsSource = RealTimePos; 
            });

        }



    }

    
}

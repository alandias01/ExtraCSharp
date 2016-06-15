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
using WPFUtils;


using System.ComponentModel;
using LoadingControl.Control;
using Data.DTC;

namespace Pledger.View
{
    /// <summary>
    /// Interaction logic for ManualPledgeView.xaml
    /// </summary>
    public partial class ManualPledgeView : UserControl
    {
        
        ManualPledgeViewModel MPVM = new ManualPledgeViewModel();

        public ManualPledgeView()
        {
            InitializeComponent();
            this.DataContext = MPVM;
            
            DGMenu<spAlanGetRealTimePositions_Pledger_Result> dgm = new DGMenu<spAlanGetRealTimePositions_Pledger_Result>();
            dgm.SetGrid(dgvManualPledgeUnpledge);            
        }

        private void dgvManualPledgeUnpledge_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SharesYouWantToUnpledge": e.Column.IsReadOnly = false;
                    break;

                case "SharesYouWantToPledge": e.Column.IsReadOnly = false;
                    break;

                default: e.Column.IsReadOnly = true;
                    break;
            }

        }

        

        
        
    }

    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pledger.View
{
    public delegate void HedgeCashEventHandler(object sender, HedgeCashEventArgs e);

    public class HedgeCashEventArgs : EventArgs
    {
        public double borrowPosition5239 { get; set; }
        public double loanPosition5239 { get; set; }
        public double spread5239 { get; set; }

        public double borrowPosition269 { get; set; }
        public double loanPosition269 { get; set; }
        public double spread269 { get; set; }
        
        public double difference { get; set; }
        public double hedgeMark { get; set; }
        public double hedgeSpread { get; set; }
    }



    public delegate void AutoPledgeEventHandler(object sender, AutoPledgeEventArgs e);
    /*
    public event AutoPledgeEventHandler AutoPledgeCalculated;
        
    public void onAutoPledgeCalculated(AutoPledgeEventArgs e)
    {
        if (AutoPledgeCalculated != null)
        {
            AutoPledgeCalculated(this, e);
        }
    }
    */
    public class AutoPledgeEventArgs : EventArgs
    {
        public double StockBalance { get; set; }
        public decimal TotalPledged { get; set; }
        
    }
			
}


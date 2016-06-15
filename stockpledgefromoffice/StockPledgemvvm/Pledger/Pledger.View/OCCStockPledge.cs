using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pledger.Data;

namespace Pledger.View
{
    public class OCCStockPledge
    {
        public spAlanGetRealTimePositions_Pledger_Result RealTimePosition { get; set; }
        public int PledgeableShares { get; set; }
        public int QuantityToPledge { get; set; }

        public OCCStockPledge()
        {

        }

        public OCCStockPledge(spAlanGetRealTimePositions_Pledger_Result rtp)
        {
            this.RealTimePosition = rtp;
            Calc(this.RealTimePosition);
        }

        private void Calc(spAlanGetRealTimePositions_Pledger_Result r)
        {
            if (r.TodaysNet >= 0)
            {
                this.PledgeableShares = Convert.ToInt32(r.unpledgedquantity);
            }
            else if (r.TodaysNet < 0 && r.unpledgedquantity > Math.Abs(r.TodaysNet.Value))
            {
                this.PledgeableShares = Convert.ToInt32(r.unpledgedquantity) - Math.Abs(r.TodaysNet.Value);
            }
        }
    }
}

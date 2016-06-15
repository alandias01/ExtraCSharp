using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pledger.Data;

namespace Pledger.View
{
    public class OCCStockToUnpledge
    {
        public spAlanGetRealTimePositions_Pledger_Result RealTimePosition { get; set; }
        public int SharesStillNeeded { get; set; }
        public int SharesToUnpledge { get; set; }

        public OCCStockToUnpledge(spAlanGetRealTimePositions_Pledger_Result rtp)
        {
            this.RealTimePosition = rtp;
            Calc(this.RealTimePosition);
        }

        public void Calc(spAlanGetRealTimePositions_Pledger_Result r)
        {
            //If we have a need, need>dtc qty, and we have something pledged to unpledge
            if (r.TodaysNet < 0 && Math.Abs(r.TodaysNet.Value) > r.unpledgedquantity && r.pledgedquantity > 0)
            {
                int sharesNeeded = Math.Abs(r.TodaysNet.Value) - Convert.ToInt32(r.unpledgedquantity);
                this.SharesStillNeeded = sharesNeeded > r.pledgedquantity.Value ? sharesNeeded - r.pledgedquantity.Value : 0; //Used for reporting
                this.SharesToUnpledge = sharesNeeded > r.pledgedquantity.Value ? r.pledgedquantity.Value : sharesNeeded;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.DTC;


namespace Pledger.Data
{
    public class OCCStockToUnpledge
    {
        public spAlanGetRealTimePositions_Pledger_Result RealTimePosition { get; set; }
        public int SharesStillNeeded { get; set; }
        public int SharesToUnpledge { get; set; }

        /// <summary>
        /// When using this constructor, it sets the value for SharesToUnpledge since app knows what we need to unpledge
        /// </summary>
        /// <param name="rtp"></param>
        public OCCStockToUnpledge(spAlanGetRealTimePositions_Pledger_Result rtp)
        {
            this.RealTimePosition = rtp;
            Calc(this.RealTimePosition);
        }

        /// <summary>
        /// This method doesn't do any calcs and is used for manual unpledging
        /// </summary>
        /// <param name="rtp"></param>
        /// <param name="qtup"></param>
        public OCCStockToUnpledge(spAlanGetRealTimePositions_Pledger_Result rtp, int qtup)
        {
            this.RealTimePosition = rtp;
            this.SharesToUnpledge = qtup;
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

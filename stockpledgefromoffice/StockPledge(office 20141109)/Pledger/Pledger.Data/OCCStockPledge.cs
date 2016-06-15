using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.DTC;


namespace Pledger.Data
{
    public class OCCStockPledge
    {
        public spAlanGetRealTimePositions_Pledger_Result RealTimePosition { get; set; }
        public int PledgeableShares { get; set; }
        public int QuantityToPledge { get; set; }

        public OCCStockPledge()
        {

        }

        /// <summary>
        /// Sets pledgeable shares but doesn't set Quantity to pledge as it depends on total cash amt to pledge
        /// </summary>
        /// <param name="rtp"></param>
        public OCCStockPledge(spAlanGetRealTimePositions_Pledger_Result rtp)
        {
            this.RealTimePosition = rtp;
            Calc(this.RealTimePosition);
        }

        public OCCStockPledge(spAlanGetRealTimePositions_Pledger_Result rtp, int qtp)
        {
            this.RealTimePosition = rtp;
            this.QuantityToPledge = qtp;
            
            //20140313 Don't think we need this since this is for manual pledging
            //Calc(this.RealTimePosition);
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

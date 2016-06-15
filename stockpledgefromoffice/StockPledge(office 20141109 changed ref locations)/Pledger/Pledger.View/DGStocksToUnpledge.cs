using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pledger.View
{
    public class DGStocksToUnpledge
    {
        public string CUSIP { get; set; }
        public int SharesToUnpledge { get; set; }
        public int? TodaysNet { get; set; }
    }
}

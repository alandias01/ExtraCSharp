using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pledger.Data;

namespace Pledger.View
{
    

    public class ManualPledgeDataViewStatus : IDataViewStatus<GridData, GridData>
    {
        List<OCCStockPledge> _STPList;
        List<OCCStockToUnpledge> _STUPList;
        
        public ManualPledgeDataViewStatus(List<OCCStockPledge> STPList, List<OCCStockToUnpledge> STUPList)
        {
            _STPList = STPList;
            _STUPList = STUPList;
        }

        public ICollection<GridData> GetTopGrid()
        {
            List<GridData> l1 = new List<GridData>();

            _STPList.ForEach(x => l1.Add(new GridData(x.RealTimePosition.cusip, x.QuantityToPledge, x.RealTimePosition.Price, (x.RealTimePosition.Price * x.QuantityToPledge))));

            return l1;
        }

        public ICollection<GridData> GetBottomGrid()
        {
            List<GridData> l1 = new List<GridData>();

            _STUPList.ForEach(x => l1.Add(new GridData(x.RealTimePosition.cusip, x.SharesToUnpledge, x.RealTimePosition.Price, (x.RealTimePosition.Price * x.SharesToUnpledge))));

            return l1;
        }

        public string GetTopTotal()
        {
            return _STPList.Sum(x => x.QuantityToPledge * x.RealTimePosition.Price).Value.ToString("c0");
        }

        public string GetBottomTotal()
        {
            return _STUPList.Sum(x => x.SharesToUnpledge * x.RealTimePosition.Price).Value.ToString("c0");
        }

    }

    public class GridData
    {
        public string Cusip { get; set; }
        public int Qty { get; set; }
        public decimal? Price { get; set; }
        public decimal? Amount { get; set; }

        public GridData(string c, int q, decimal? p, decimal? a)
        {
            Cusip = c; Qty = q; Price = p; Amount = a;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Krs.Ats.IBNet;
using Krs.Ats.IBNet.Contracts;

namespace garbageTWS
{
    public class MarketData
    {
        IBClient _client;

        public MarketData(IBClient client)
        {
            this._client = client;
        }

        public void Run()
        {
            _client.Connect("localhost", 7496, 2);

            SetEvents();
            GetMarketData();
            
        }

        private void SetEvents()
        {
            _client.TickPrice += new EventHandler<TickPriceEventArgs>(client_TickPrice); 
            _client.tick
        }

        private void GetMarketData()
        {
            _client.RequestMarketData(1, new Equity("IBM"), null, false, false);
        }

        #region Event Handlers
        void client_TickPrice(object sender, TickPriceEventArgs e)
        {
            decimal s = e.Price;
        }


        #endregion
    }
}

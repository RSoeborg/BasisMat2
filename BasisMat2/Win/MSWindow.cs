using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BasisMat2.Win
{
    public class MSWindow : IWindow
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
        
        public enum SW : int
        {
            HIDE = 0,
            SHOWNORMAL = 1,
            SHOWMINIMIZED = 2,
            SHOWMAXIMIZED = 3,
            SHOWNOACTIVATE = 4,
            SHOW = 5,
            MINIMIZE = 6,
            SHOWMINNOACTIVE = 7,
            SHOWNA = 8,
            RESTORE = 9,
            SHOWDEFAULT = 10
        }

        public IntPtr Handle { get; }

        public MSWindow(IntPtr Handle) {
            this.Handle = Handle;
        }

        public string Title
        {
            get
            {
                int length = GetWindowTextLength(Handle);
                StringBuilder sb = new StringBuilder(length + 1);
                GetWindowText(Handle, sb, sb.Capacity);
                return sb.ToString();
            }
        }

        public void Hide()
        {
            ShowWindowAsync(Handle, (int)SW.HIDE);
        }

        public void Show()
        {
            ShowWindowAsync(Handle, (int)SW.SHOW);
        }
    }
}

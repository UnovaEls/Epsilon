using Epsilon.Alpha.WinApis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epsilon.Alpha.Hooks
{
    internal class ForegroundHook
    {
        private IntPtr _key;
        private User32.WinEventDelegate _winEvent;

        public delegate void OnWindowChanged(IntPtr hwnd, string title);
        public event OnWindowChanged? WindowActivated;
        public event OnWindowChanged? WindowMoveSizeStart;
        public event OnWindowChanged? WindowMoveSizeEnd;
        public event OnWindowChanged? WindowMinimizeStart;
        public event OnWindowChanged? WindowMinimizeEnd;

        public ForegroundHook()
        {
            _winEvent = WinEvent;
        }

        public void Hook()
        {
            _key = User32.SetWinEventHook(0x0003, 0x0017, IntPtr.Zero, _winEvent, 0, 0, 0x0000 | 0x0002);
        }

        public void Unhook()
        {
            User32.UnhookWinEvent(_key);
        }

        private void WinEvent(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            switch (eventType)
            {
                case 0x0003:
                    WindowActivated?.Invoke(hwnd, GetWindowTitle(hwnd));
                    break;
                case 0x000A:
                    WindowMoveSizeStart?.Invoke(hwnd, GetWindowTitle(hwnd));
                    break;
                case 0x000B:
                    WindowMoveSizeEnd?.Invoke(hwnd, GetWindowTitle(hwnd));
                    break;
                case 0x0016:
                    WindowMinimizeStart?.Invoke(hwnd, GetWindowTitle(hwnd));
                    break;
                case 0x0017:
                    WindowMinimizeEnd?.Invoke(hwnd, GetWindowTitle(hwnd));
                    break;
            }
        }

        private string GetWindowTitle(IntPtr hwnd)
        {
            int length = User32.GetWindowTextLength(hwnd);

            if (length > 0)
            {
                StringBuilder sb = new StringBuilder(length + 1);
                User32.GetWindowText(hwnd, sb, sb.Capacity);

                return sb.ToString();
            }

            return string.Empty;
        }
    }
}

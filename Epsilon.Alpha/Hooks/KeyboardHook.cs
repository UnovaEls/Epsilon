using Epsilon.Alpha.WinApis;
using Epsilon.Alpha.WinApis.Structures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Epsilon.Alpha.Hooks
{
    internal class KeyboardHook
    {
        private IntPtr _key;
        private User32.HookProc _hookProc;

        public delegate void OnKey(uint key);
        public event OnKey? KeyUp;
        public event OnKey? KeyDown;

        public KeyboardHook()
        {
            _hookProc = HookProc;
        }

        public void Hook()
        {
            //_key = User32.SetWindowsHookEx(13, _hookProc, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
            using (Process p = Process.GetCurrentProcess())
            using (ProcessModule? m = p.MainModule)
            {
                if (m == null)
                    throw new Exception("Failed to obtain main module base address.");

                _key = User32.SetWindowsHookEx(13, _hookProc, m.BaseAddress, 0);
            }
        }

        public void Unhook()
        {
            User32.UnhookWindowsHookEx(_key);
        }

        private IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code == 0)
            {
                int keyEvent = (int)wParam;
                KeyStruct? keyStruct = Marshal.PtrToStructure<KeyStruct>(lParam);

                if (keyStruct != null)
                {
                    switch (keyEvent)
                    {
                        case 0x100:
                        case 0x104:
                            KeyDown?.Invoke(keyStruct.vkCode);
                            break;
                        case 0x101:
                        case 0x105:
                            KeyUp?.Invoke(keyStruct.vkCode);
                            break;
                    }
                }
            }

            return User32.CallNextHookEx(_key, code, wParam, lParam);
        }
    }
}

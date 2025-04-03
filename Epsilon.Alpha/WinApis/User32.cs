using Epsilon.Alpha.WinApis.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Epsilon.Alpha.WinApis
{
    internal static class User32
    {
        [DllImport(LibraryNames.User32)]
        public static extern IntPtr SetWindowsHookEx(int hookType, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport(LibraryNames.User32)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport(LibraryNames.User32)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        public delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);

        [DllImport(LibraryNames.User32)]
        public static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        [DllImport(LibraryNames.User32)]
        public static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        [DllImport(LibraryNames.User32, CharSet = CharSet.Auto)]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport(LibraryNames.User32, CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport(LibraryNames.User32)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport(LibraryNames.User32)]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport(LibraryNames.User32)]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport(LibraryNames.User32)]
        public static extern bool ReleaseCapture();
    }
}

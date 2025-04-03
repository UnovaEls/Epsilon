using Epsilon.Alpha.WinApis.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Epsilon.Alpha.WinApis
{
    internal static class DwmApi
    {
        [DllImport(LibraryNames.DwmApi)]
        public static extern int DwmGetWindowAttribute(IntPtr hwnd, uint dwAttribute, out WindowRect pvAttribute, int cbAttribute);
    }
}

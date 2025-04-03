using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epsilon.Alpha.Controls
{
    internal interface IOverlayDraw
    {
        Image? GetIcon();

        Color GetColor();

        string GetText();

        void Reset();
    }
}

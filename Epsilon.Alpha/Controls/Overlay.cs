using Epsilon.Alpha.WinApis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epsilon.Alpha.Controls
{
    internal class Overlay : Form
    {
        private bool _draw;
        private List<IOverlayDraw> _draws;
        public event EventHandler? OnMoved;

        public Overlay()
        {
            _draw = false;
            _draws = new List<IOverlayDraw>();

            this.BackColor = Color.Fuchsia;
            this.DoubleBuffered = true;
            this.Font = new Font("Segoe UI", 9.75F);
            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.TransparencyKey = Color.Fuchsia;
            this.DrawItemHeight = 45;
            this.DrawItemWidth = 26;
            this.DrawImagePadding = new Padding(3, 3, 3, 3);
            this.DrawTextPadding = new Padding(0);
            this.DrawIconSize = new Size(20, 20);
            this.DrawLocation = new Point(0, 0);
            this.Refresh = 250;

            this.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);

            _ = RefreshInvalidate();
        }

        private async Task RefreshInvalidate()
        {
            while (true)
            {
                await Task.Delay(this.Refresh);
                Invalidate();
            }
        }

        public void AddDraw(IOverlayDraw draw)
        {
            lock (_draws)
                _draws.Add(draw);
        }

        public void RemoveDraw(IOverlayDraw draw)
        {
            lock (_draws)
                _draws.Remove(draw);
        }

        public void HideDraw()
        {
            lock (_draws)
                _draw = false;
        }

        public void ShowDraw()
        {
            lock (_draws)
                _draw = true;
        }

        public void Materialize()
        {
            User32.SetWindowLong(this.Handle, -20, User32.GetWindowLong(this.Handle, -20) ^ 0x20);
        }

        public void Dematerialize()
        {
            User32.SetWindowLong(this.Handle, -20, User32.GetWindowLong(this.Handle, -20) | 0x20);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            User32.SetWindowLong(this.Handle, -20, User32.GetWindowLong(this.Handle, -20) | 0x80000 | 0x20 | 0x80);
            this.TopMost = true;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                User32.ReleaseCapture();
                User32.SendMessage(Handle, 0xA1, 0x2, 0);
                OnMoved?.Invoke(this, EventArgs.Empty);
            }

            base.OnMouseDown(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            lock (_draws)
            {
                if (_draw)
                {
                    for (int i = 0; i < _draws.Count; i++)
                    {
                        IOverlayDraw draw = _draws[i];
                        string text = draw.GetText();
                        Color clr = draw.GetColor();
                        Image? icon = draw.GetIcon();

                        using (SolidBrush brush = new SolidBrush(clr))
                            e.Graphics.FillRectangle(brush, new Rectangle(this.DrawLocation.X + i * this.DrawItemWidth, this.DrawLocation.Y, this.DrawItemWidth, this.DrawItemHeight));

                        if (icon != null)
                            e.Graphics.DrawImage(icon, this.DrawLocation.X + i * this.DrawItemWidth + this.DrawImagePadding.Left, this.DrawLocation.Y + this.DrawImagePadding.Top, this.DrawIconSize.Width, this.DrawIconSize.Height);

                        int textLeft = i * this.DrawItemWidth + this.DrawTextPadding.Left;
                        int textTop = this.DrawImagePadding.Top + this.DrawIconSize.Height + this.DrawImagePadding.Bottom + this.DrawTextPadding.Top;
                        Rectangle textRec = new Rectangle(this.DrawLocation.X + textLeft, this.DrawLocation.Y + textTop, this.DrawItemWidth - this.DrawTextPadding.Left - this.DrawTextPadding.Right, this.DrawItemHeight - textTop - this.DrawTextPadding.Bottom);

                        TextRenderer.DrawText(e.Graphics, text, this.Font, textRec, this.ForeColor, TextFormatFlags.HorizontalCenter);
                    }
                }
            }

            base.OnPaint(e);
        }

        public int DrawItemHeight { get; set; }

        public int DrawItemWidth { get; set; }

        public Padding DrawImagePadding { get; set; }

        public Padding DrawTextPadding { get; set; }

        public Size DrawIconSize { get; set; }

        public Point DrawLocation { get; set; }

        public new int Refresh { get; set; }
    }
}

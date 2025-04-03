using Epsilon.Alpha.Configuration;
using Epsilon.Alpha.Controls;
using Epsilon.Alpha.Elsword.Title;
using Epsilon.Alpha.Hooks;
using Epsilon.Alpha.WinApis.Structures;
using Epsilon.Alpha.WinApis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Epsilon.Alpha.Elsword.Buff;

namespace Epsilon.Alpha.Elsword
{
    internal class ElswordProcessor
    {
        private bool _active;
        private ElswordTitleStateMachine _titleStateMachine;
        private ElswordBuffs _buffs;
        private EpsilonConfig _config;
        private EpsilonConfigurationController _configController;
        private KeyboardHook _keyboardHook;
        private ForegroundHook _foregroundHook;
        private Overlay _overlay;
        private NotifyIcon _notifyIcon;
        private IntPtr _last;

        public ElswordProcessor(EpsilonConfigurationController configController, KeyboardHook keyboardHook, ForegroundHook foregroundHook, Overlay overlay, NotifyIcon notifyIcon)
        {
            _active = false;
            _titleStateMachine = new ElswordTitleStateMachine();
            _buffs = new ElswordBuffs();
            _configController = configController;
            _keyboardHook = keyboardHook;
            _foregroundHook = foregroundHook;
            _overlay = overlay;
            _notifyIcon = notifyIcon;
            _last = IntPtr.Zero;

            AttachBuffs();
            InitializeEvents();
        }

        private void AttachBuffs()
        {
            _overlay.AddDraw(_buffs.FreedShadow);
            _overlay.AddDraw(_buffs.NightParade);
            _overlay.AddDraw(_buffs.TheSettingSun);
            _overlay.AddDraw(_buffs.Transcendence);
        }

        private void InitializeEvents()
        {
            _overlay.HandleCreated += Overlay_HandleCreated;
            _overlay.OnMoved += Overlay_OnMoved;

            _keyboardHook.KeyDown += KeyboardHook_KeyDown;

            _foregroundHook.WindowActivated += ForegroundHook_WindowActivated;
            _foregroundHook.WindowMoveSizeStart += ForegroundHook_WindowMoveSizeStart;
            _foregroundHook.WindowMoveSizeEnd += ForegroundHook_WindowMoveSizeEnd;
            _foregroundHook.WindowMinimizeStart += ForegroundHook_WindowMinimizeStart;
            _foregroundHook.WindowMinimizeEnd += ForegroundHook_WindowMinimizeEnd;
        }

        private void CreateNotifyIconMenu()
        {
            ContextMenuStrip cms = new ContextMenuStrip() { ShowCheckMargin = true, ShowImageMargin = false };

            cms.Items.Add(new ToolStripMenuItem("Allow Relocation", null, AllowRelocationMenuItem_Click) { CheckOnClick = true });
            cms.Items.Add(new ToolStripMenuItem("Exit", null, ExitMenuItem_Click));

            _notifyIcon.ContextMenuStrip = cms;
            _notifyIcon.Visible = true;
        }

        private void InitializeHooks()
        {
            _keyboardHook.Hook();
            _foregroundHook.Hook();
        }

        private void AllowRelocationMenuItem_Click(object? sender, EventArgs e)
        {
            ToolStripMenuItem? tsmi = sender as ToolStripMenuItem;

            if (tsmi != null)
            {
                if (tsmi.Checked)
                    _overlay.Materialize();
                else
                    _overlay.Dematerialize();
            }
        }

        private void ExitMenuItem_Click(object? sender, EventArgs e)
        {
            _overlay.Close();
        }

        private async void Overlay_HandleCreated(object? sender, EventArgs e)
        {
            _config = await _configController.ReadAsync();
            CreateNotifyIconMenu();
            InitializeHooks();
        }

        private async void Overlay_OnMoved(object? sender, EventArgs e)
        {
            AdjustConfigLocation(_overlay.Handle);
            await _configController.WriteAsync(_config);
        }

        private void KeyboardHook_KeyDown(uint key)
        {
            if (!_active)
                return;

            Keys keys = (Keys)key;

            if (_config.AwakeningKeys.Contains(keys))
            {
                if (_titleStateMachine.CurrentState == ElswordTitleState.FreedShadow)
                    _buffs.FreedShadow.Reset();
                else if (_titleStateMachine.CurrentState == ElswordTitleState.TheSettingSun)
                    _buffs.TheSettingSun.Reset();
            }
            else if (keys == _config.TitleSwapKey)
            {
                _titleStateMachine.MoveNext(ElswordTitleCommand.TitleSwap);
            }
            else if (keys == _config.NightParadeArrowKey)
            {
                if (_titleStateMachine.CurrentState == ElswordTitleState.TitleSwapping)
                    _titleStateMachine.MoveNext(ElswordTitleCommand.SelectNightParade);
            }
            else if (keys == _config.FreedShadowArrowKey)
            {
                if (_titleStateMachine.CurrentState == ElswordTitleState.TitleSwapping)
                    _titleStateMachine.MoveNext(ElswordTitleCommand.SelectFreedShadow);
            }
            else if (keys == _config.TheSettingSunArrowKey)
            {
                if (_titleStateMachine.CurrentState == ElswordTitleState.TitleSwapping)
                    _titleStateMachine.MoveNext(ElswordTitleCommand.SelectTheSettingSun);
            }
            else if (_config.OtherTitleArrowKeys.Contains(keys))
            {
                if (_titleStateMachine.CurrentState == ElswordTitleState.TitleSwapping)
                    _titleStateMachine.MoveNext(ElswordTitleCommand.SelectOtherTitle);
            }
            else if (keys == _config.ResetTranscendenceKey)
            {
                _buffs.Transcendence.Reset();
            }
            else if (_config.SpecialActiveSkillKeys.Contains(keys) && _titleStateMachine.CurrentState == ElswordTitleState.NightParade)
            {
                _buffs.NightParade.Reset();
            }
        }

        private void ForegroundHook_WindowActivated(IntPtr hwnd, string title)
        {
            if (title.StartsWith(_config.ElswordTitle))
            {
                AdjustOverlayLocation(hwnd);
                ActivateAndShowOverlay();
            }
            else
            {
                DeactivateAndHideOverlay();
            }
        }

        private void ForegroundHook_WindowMoveSizeStart(IntPtr hwnd, string title)
        {
            if (title.StartsWith(_config.ElswordTitle))
                DeactivateAndHideOverlay();
        }

        private void ForegroundHook_WindowMoveSizeEnd(IntPtr hwnd, string title)
        {
            if (title.StartsWith(_config.ElswordTitle))
            {
                AdjustOverlayLocation(hwnd);
                ActivateAndShowOverlay();
            }
        }

        private void ForegroundHook_WindowMinimizeStart(IntPtr hwnd, string title)
        {
            if (title.StartsWith(_config.ElswordTitle))
                DeactivateAndHideOverlay();
        }

        private void ForegroundHook_WindowMinimizeEnd(IntPtr hwnd, string title)
        {
            if (title.StartsWith(_config.ElswordTitle))
            {
                AdjustOverlayLocation(hwnd);
                ActivateAndShowOverlay();
            }
        }

        private void AdjustOverlayLocation(IntPtr hwnd)
        {
            _last = hwnd;

            WindowRect windowFrame = new WindowRect();
            int result = DwmApi.DwmGetWindowAttribute(hwnd, 9, out windowFrame, Marshal.SizeOf<WindowRect>());

            if (result == 0)
            {
                lock (_config)
                {
                    int width = windowFrame.Right - windowFrame.Left;
                    int height = windowFrame.Bottom - windowFrame.Top;

                    _overlay.Location = new Point(windowFrame.Left + (int)(_config.BuffLocationX / 100.0 * width), windowFrame.Top + (int)(_config.BuffLocationY / 100.0 * height));
                }
            }
        }

        private void AdjustConfigLocation(IntPtr hwnd)
        {
            WindowRect windowFrame = new WindowRect();
            int result = DwmApi.DwmGetWindowAttribute(_last, 9, out windowFrame, Marshal.SizeOf<WindowRect>());

            if (result == 0)
            {
                lock (_config)
                {
                    int width = windowFrame.Right - windowFrame.Left;
                    int height = windowFrame.Bottom - windowFrame.Top;

                    _config.BuffLocationX = 100.0 * (_overlay.Location.X - windowFrame.Left) / width;
                    _config.BuffLocationY = 100.0 * (_overlay.Location.Y - windowFrame.Top) / height;
                }
            }
        }

        private void ActivateAndShowOverlay()
        {
            _active = true;
            _overlay.ShowDraw();
            _overlay.Visible = true;
        }

        private void DeactivateAndHideOverlay()
        {
            _active = false;
            _overlay.HideDraw();
            _overlay.Visible = false;
        }
    }
}

﻿#region Copyrights
/*
Gibbo2D License - Version 1.0
Copyright (c) 2013 - Gibbo2D Team
Founders Joao Alves <joao.cpp.sca@gmail.com> & Luis Fernandes <luisapidcloud@hotmail.com>

Permission is granted to use this software and associated documentation files (the "Software") free of charge, 
to any person or company. The code can be used, modified and merged without restrictions, but you cannot sell 
the software itself and parts where this license applies. Still, permission is granted for anyone to sell 
applications made using this software (for example, a game). This software cannot be claimed as your own, 
except for copyright holders. This license notes should also be available on any of the changed or added files.

The software is provided "as is", without warranty of any kind, express or implied, including but not limited to 
the warranties of merchantability, fitness for a particular purpose and non-infrigement. In no event shall the 
authors or copyright holders be liable for any claim, damages or other liability.

The license applies to all versions of the software, both newer and older than the one listed, unless a newer copy 
of the license is available, in which case the most recent copy of the license supercedes all others.

*/
#endregion

using System.Collections.Generic;
using System.Windows.Forms;
using XKeys = Microsoft.Xna.Framework.Input.Keys;

namespace Gibbo.Editor.WPF
{
    /// <summary>
    /// 
    /// </summary>
    public static class KeyboardUtil
    {
        private static Dictionary<Keys, XKeys> _map;

        static KeyboardUtil()
        {
            _map = new Dictionary<Keys, XKeys>() {
                { Keys.A, XKeys.A },
                { Keys.Add, XKeys.Add },
                { Keys.Alt, XKeys.LeftAlt },
                { Keys.Apps, XKeys.Apps },
                { Keys.Attn, XKeys.Attn },
                { Keys.B, XKeys.B },
                { Keys.Back, XKeys.Back },
                { Keys.BrowserBack, XKeys.BrowserBack },
                { Keys.BrowserFavorites, XKeys.BrowserFavorites },
                { Keys.BrowserForward, XKeys.BrowserForward },
                { Keys.BrowserHome, XKeys.BrowserHome },
                { Keys.BrowserRefresh, XKeys.BrowserRefresh },
                { Keys.BrowserSearch, XKeys.BrowserSearch },
                { Keys.BrowserStop, XKeys.BrowserStop },
                { Keys.C, XKeys.C },
                { Keys.CapsLock, XKeys.CapsLock },
                { Keys.Crsel, XKeys.Crsel },
                { Keys.D, XKeys.D },
                { Keys.D0, XKeys.D0 },
                { Keys.D1, XKeys.D1 },
                { Keys.D2, XKeys.D2 },
                { Keys.D3, XKeys.D3 },
                { Keys.D4, XKeys.D4 },
                { Keys.D5, XKeys.D5 },
                { Keys.D6, XKeys.D6 },
                { Keys.D7, XKeys.D7 },
                { Keys.D8, XKeys.D8 },
                { Keys.D9, XKeys.D9 },
                { Keys.Decimal, XKeys.Decimal },
                { Keys.Delete, XKeys.Delete },
                { Keys.Divide, XKeys.Divide },
                { Keys.Down, XKeys.Down },
                { Keys.E, XKeys.E },
                { Keys.End, XKeys.End },
                { Keys.Enter, XKeys.Enter },
                { Keys.EraseEof, XKeys.EraseEof },
                { Keys.Escape, XKeys.Escape },
                { Keys.Execute, XKeys.Execute },
                { Keys.Exsel, XKeys.Exsel },
                { Keys.F, XKeys.F },
                { Keys.F1, XKeys.F1 },
                { Keys.F10, XKeys.F10 },
                { Keys.F11, XKeys.F11 },
                { Keys.F12, XKeys.F12 },
                { Keys.F13, XKeys.F13 },
                { Keys.F14, XKeys.F14 },
                { Keys.F15, XKeys.F15 },
                { Keys.F16, XKeys.F16 },
                { Keys.F17, XKeys.F17 },
                { Keys.F18, XKeys.F18 },
                { Keys.F19, XKeys.F19 },
                { Keys.F2, XKeys.F2 },
                { Keys.F20, XKeys.F20 },
                { Keys.F21, XKeys.F21 },
                { Keys.F22, XKeys.F22 },
                { Keys.F23, XKeys.F23 },
                { Keys.F24, XKeys.F24 },
                { Keys.F3, XKeys.F3 },
                { Keys.F4, XKeys.F4 },
                { Keys.F5, XKeys.F5 },
                { Keys.F6, XKeys.F6 },
                { Keys.F7, XKeys.F7 },
                { Keys.F8, XKeys.F8 },
                { Keys.F9, XKeys.F9 },
                { Keys.G, XKeys.G },
                { Keys.H, XKeys.H },
                { Keys.Help, XKeys.Help },
                { Keys.Home, XKeys.Home },
                { Keys.I, XKeys.I },
                { Keys.IMEConvert, XKeys.ImeConvert },
                { Keys.IMENonconvert, XKeys.ImeNoConvert },
                { Keys.Insert, XKeys.Insert },
                { Keys.J, XKeys.J },
                { Keys.K, XKeys.K },
                { Keys.KanaMode, XKeys.Kana },
                { Keys.KanjiMode, XKeys.Kanji },
                { Keys.NumPad0, XKeys.NumPad0 },
                { Keys.NumPad1, XKeys.NumPad1 },
                { Keys.NumPad2, XKeys.NumPad2 },
                { Keys.NumPad3, XKeys.NumPad3 },
                { Keys.NumPad4, XKeys.NumPad4 },
                { Keys.NumPad5, XKeys.NumPad5 },
                { Keys.NumPad6, XKeys.NumPad6 },
                { Keys.NumPad7, XKeys.NumPad7 },
                { Keys.NumPad8, XKeys.NumPad8 },
                { Keys.NumPad9, XKeys.NumPad9 },
                { Keys.Multiply, XKeys.Multiply },
                { Keys.L, XKeys.L },
                { Keys.LaunchApplication1, XKeys.LaunchApplication1 },
                { Keys.LaunchApplication2, XKeys.LaunchApplication2 },
                { Keys.LaunchMail, XKeys.LaunchMail },
                { Keys.LControlKey, XKeys.LeftControl },
                { Keys.LButton | Keys.ShiftKey, XKeys.LeftControl },
                { Keys.ShiftKey, XKeys.LeftShift },
                { Keys.Left, XKeys.Left },
                { Keys.LShiftKey, XKeys.LeftShift },
                { Keys.LWin, XKeys.LeftWindows },
                { Keys.M, XKeys.M },
                { Keys.MediaNextTrack, XKeys.MediaNextTrack },
                { Keys.MediaPlayPause, XKeys.MediaPlayPause },
                { Keys.MediaPreviousTrack, XKeys.MediaPreviousTrack },
                { Keys.MediaStop, XKeys.MediaStop },
                { Keys.N, XKeys.N },
                { Keys.None, XKeys.None },
                { Keys.NumLock, XKeys.NumLock },
                { Keys.O, XKeys.O },
                { Keys.Oem8, XKeys.Oem8 },
                { Keys.OemBackslash, XKeys.OemPipe },   // MonoGame Issue 1012
                { Keys.OemClear, XKeys.OemClear },
                { Keys.OemCloseBrackets, XKeys.OemCloseBrackets },
                { Keys.Oemcomma, XKeys.OemComma },
                { Keys.OemMinus, XKeys.OemMinus },
                { Keys.OemOpenBrackets, XKeys.OemOpenBrackets },
                { Keys.OemPeriod, XKeys.OemPeriod },
                { Keys.Oemplus, XKeys.OemPlus },
                { Keys.OemQuestion, XKeys.OemQuestion },
                { Keys.OemQuotes, XKeys.OemQuotes },
                { Keys.OemSemicolon, XKeys.OemSemicolon },
                { Keys.Oemtilde, XKeys.OemTilde },
                { Keys.P, XKeys.P },
                { Keys.Pa1, XKeys.Pa1 },
                { Keys.PageDown, XKeys.PageDown },
                { Keys.PageUp, XKeys.PageUp },
                { Keys.Pause, XKeys.Pause },
                { Keys.Play, XKeys.Play },
                { Keys.Print, XKeys.Print },
                { Keys.PrintScreen, XKeys.PrintScreen },
                { Keys.ProcessKey, XKeys.ProcessKey },
                { Keys.Q, XKeys.Q },
                { Keys.R, XKeys.R },
                { Keys.RControlKey, XKeys.RightControl },
                { Keys.Right, XKeys.Right },
                { Keys.RShiftKey, XKeys.RightShift },
                { Keys.RWin, XKeys.RightWindows },
                { Keys.S, XKeys.S },
                { Keys.Scroll, XKeys.Scroll },
                { Keys.SelectMedia, XKeys.SelectMedia },
                { Keys.Separator, XKeys.Separator },
                { Keys.Sleep, XKeys.Sleep },
                { Keys.Space, XKeys.Space },
                { Keys.T, XKeys.T },
                { Keys.Tab, XKeys.Tab },
                { Keys.U, XKeys.U },
                { Keys.Up, XKeys.Up },
                { Keys.V, XKeys.V },
                { Keys.VolumeDown, XKeys.VolumeDown },
                { Keys.VolumeMute, XKeys.VolumeMute },
                { Keys.VolumeUp, XKeys.VolumeUp },
                { Keys.W, XKeys.W },
                { Keys.X, XKeys.X },
                { Keys.Y, XKeys.Y },
                { Keys.Z, XKeys.Z },
                { Keys.Zoom, XKeys.Zoom },
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static XKeys ToXna(Keys key)
        {
            XKeys xkey;
            if (_map.TryGetValue(key, out xkey))
                return xkey;
            else
                return XKeys.None;
        }
    }
}

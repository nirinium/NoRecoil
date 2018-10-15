﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Corale;

namespace NoRecoil
{
    class MagicMouse
    {

        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey);
        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        public static Corale.Colore.Core.IChroma iChroma;
        public static Corale.Colore.Razer.Mouse.Effects.Static eGreen;
        public static Corale.Colore.Razer.Mouse.Effects.Static eRed;
        public static bool _CHROMA_INITIALIZED = false;

        public static int _ScreenWidth { get; } = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
        public static int _ScreenHeight { get; } = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
        public static int _ScreenCenterX { get; } = _ScreenWidth / 2;
        public static int _ScreenCenterY { get; } = _ScreenHeight / 2;
        public static int[] LoadIntConfig(string FilePath)
        {
            using (StreamReader sr = new StreamReader(FilePath))
            {
                string buffer = sr.ReadLine();
                sr.Close();
                if (buffer == null)
                {
                    int[] i = { 0 };
                    return i;
                }
                else
                {
                    try
                    {
                        int[] t;
                        var sL = buffer.Split(':');
                        List<int> tL = new List<int>();
                        foreach (String s in sL)
                        {
                            tL.Add(Int32.Parse(s));
                        }
                        t = tL.ToArray();
                        return t;
                    }
                    catch (Exception ex)
                    {
                        int[] i = { 0 };
                        return i;
                    }
                }
            }
        }
        public static bool bGetAsyncKeyState(System.Windows.Forms.Keys vKey)
        {
            int x = GetAsyncKeyState(vKey);
            if ((x == 1) || (x == Int16.MinValue))
                return true;
            else
                return false;
        }

        private const int MOUSEEVENTF_MOVE = 0x0001;

        public static void Move(int xDelta, int yDelta)
        {
            mouse_event(MOUSEEVENTF_MOVE, xDelta, yDelta, 0, 0);
        }
        public static void SaveIntConfig(string FilePath, int[]Settings)
        {
            string ToWrite = "";
            int i = 0;
            StreamWriter sw = new StreamWriter(FilePath);
            foreach(int t in Settings)
            {
                if(i < (Settings.Length -1))
                {
                    ToWrite = ToWrite + t.ToString() + ":";
                    i++;
                }
                else
                {
                    ToWrite = ToWrite + t.ToString();
                }
            }
            sw.WriteLine(ToWrite);
            sw.Close();
        }
        public static void InitializeChroma()
        {
            try
            {
                iChroma = Corale.Colore.Core.Chroma.Instance;
                eRed = new Corale.Colore.Razer.Mouse.Effects.Static(Corale.Colore.Razer.Mouse.Led.All, Corale.Colore.Core.Color.Red);
                eGreen = new Corale.Colore.Razer.Mouse.Effects.Static(Corale.Colore.Razer.Mouse.Led.All, Corale.Colore.Core.Color.Green);
                _CHROMA_INITIALIZED = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error");
            }
        }
        public static void StateEnabled()
        {
            iChroma.Mouse.SetStatic(eGreen);
        }
        public static void StateDisabled()
        {
            iChroma.Mouse.SetStatic(eRed);
        }

    }
}

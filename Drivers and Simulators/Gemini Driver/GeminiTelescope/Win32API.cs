﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ASCOM.GeminiTelescope
{
    class Win32API
    {
        ///////WIN32 functions//////////

        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr hwnd);

        public static IntPtr HWND_TOP = (IntPtr)(0);
        public static IntPtr HWND_BOTTOM = (IntPtr)1;
        public static IntPtr HWND_TOPMOST = (IntPtr)(-1);
        public const UInt32 SWP_NOSIZE = 0x0001;
        public const UInt32 SWP_NOMOVE = 0x0002;
        public const UInt32 SWP_NOZORDER = 0x0004;
        public const UInt32 SWP_NOREDRAW = 0x0008;
        public const UInt32 SWP_NOACTIVATE = 0x0010;
        public const UInt32 SWP_FRAMECHANGED = 0x0020;  /* The frame changed: send WM_NCCALCSIZE */
        public const UInt32 SWP_SHOWWINDOW = 0x0040;
        public const UInt32 SWP_HIDEWINDOW = 0x0080;
        public const UInt32 SWP_NOCOPYBITS = 0x0100;
        public const UInt32 SWP_NOOWNERZORDER = 0x0200;  /* Don't do owner Z ordering */
        public const UInt32 SWP_NOSENDCHANGING = 0x0400;  /* Don't send WM_WINDOWPOSCHANGING */

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern bool SetWindowPos(
           IntPtr hWnd,               // window handle
           IntPtr hWndInsertAfter,    // placement-order handle
           int X,                  // horizontal position
           int Y,                  // vertical position
           int cx,                 // width
           int cy,                 // height
           uint uFlags);           // window positioning flags



        #region JOYSTICK API
  
        [StructLayout(LayoutKind.Sequential)]
        public struct JOYCAPS
        {
            public ushort wMid;
            public ushort wPid;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String szPname;
            public Int32 wXmin;
            public Int32 wXmax;
            public Int32 wYmin;
            public Int32 wYmax;
            public Int32 wZmin;
            public Int32 wZmax;
            public Int32 wNumButtons;
            public Int32 wPeriodMin;
            public Int32 wPeriodMax;
            public Int32 wRmin;
            public Int32 wRmax;
            public Int32 wUmin;
            public Int32 wUmax;
            public Int32 wVmin;
            public Int32 wVmax;
            public Int32 wCaps;
            public Int32 wMaxAxes;
            public Int32 wNumAxes;
            public Int32 wMaxButtons;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String szRegKey;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public String szOEMVxD;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct JOYINFO
        {
            public Int32 wXpos;
            public Int32 wYpos;
            public Int32 wZpos;
            public Int32 wButtons;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct JOYINFOEX
        {
            public Int32 dwSize;
            public Int32 dwFlags;
            public Int32 dwXpos;
            public Int32 dwYpos;
            public Int32 dwZpos;
            public Int32 dwRpos;
            public Int32 dwUpos;
            public Int32 dwVpos;
            public Int32 dwButtons;
            public Int32 dwButtonNumber;
            public Int32 dwPOV;
            public Int32 dwReserved1;
            public Int32 dwReserved2;
        }



        public static Int32 JOY_BUTTON1 = 1;
        public static Int32 JOY_BUTTON2 = 2;
        public static Int32 JOY_BUTTON3 = 4;
        public static Int32 JOY_BUTTON4 = 8;
        public static Int32 JOYCAPS_HASZ = 1;
        public static Int32 JOYCAPS_HASR = 2;
        public static Int32 JOYCAPS_HASU = 4;
        public static Int32 JOYCAPS_HASV = 8;
        public static Int32 JOYCAPS_HASPOV = 16;
        public static Int32 JOYCAPS_POV4DIR = 32;
        public static Int32 JOYCAPS_POVCTS = 64;
        public static Int32 JOY_RETURNX = 0x00000001;
        public static Int32 JOY_RETURNY = 0x00000002;
        public static Int32 JOY_RETURNZ = 0x00000004;
        public static Int32 JOY_RETURNR = 0x00000008;
        public static Int32 JOY_RETURNU = 0x00000010;
        public static Int32 JOY_RETURNV = 0x00000020;
        public static Int32 JOY_RETURNPOV = 0x00000040;
        public static Int32 JOY_RETURNBUTTONS = 0x00000080;
        public static Int32 JOY_RETURNRAWDATA = 0x00000100;
        public static Int32 JOY_RETURNPOVCTS = 0x00000200;
        public static Int32 JOY_RETURNCENTERED = 0x00000400;
        public static Int32 JOY_USEDEADZONE = 0x00000800;
        public static Int32 JOY_RETURNALL = (JOY_RETURNX | JOY_RETURNY | JOY_RETURNZ | JOY_RETURNR | JOY_RETURNU | JOY_RETURNV | JOY_RETURNPOV | JOY_RETURNBUTTONS);
        
        public static Int32 JOY_RETURNXY = (JOY_RETURNX | JOY_RETURNY);

        public static Int32 JOY_CAL_READALWAYS = 0x00010000;
        public static Int32 JOY_CAL_READXYONLY = 0x00020000;
        public static Int32 JOY_CAL_READ3 = 0x00040000;
        public static Int32 JOY_CAL_READ4 = 0x00080000;
        public static Int32 JOY_CAL_READXONLY = 0x00100000;
        public static Int32 JOY_CAL_READYONLY = 0x00200000;
        public static Int32 JOY_CAL_READ5 = 0x00400000;
        public static Int32 JOY_CAL_READ6 = 0x00800000;
        public static Int32 JOY_CAL_READZONLY = 0x00800000;
        public static Int32 JOY_CAL_READRONLY = 0x02000000;
        public static Int32 JOY_CAL_READUONLY = 0x04000000;
        public static Int32 JOY_CAL_READVONLY = 0x08000000;
        public static Int32 JOY_POVCENTERED = -1;
        public static Int32 JOY_POVFORWARD = 0;
        public static Int32 JOY_POVRIGHT = 9000;
        public static Int32 JOY_POVBACKWARD = 18000;
        public static Int32 JOY_POVLEFT = 27000;


        [DllImport("winmm.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 joyConfigChanged(Int64 dwFlags);

        [DllImport("winmm.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 joyGetDevCaps(IntPtr uJoyID, ref JOYCAPS pjc, Int32 cbjc);

        [DllImport("winmm.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 joyGetNumDevs();

        [DllImport("winmm.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 joyGetPos(Int32 uJoyID, ref JOYINFO pji);

        [DllImport("winmm.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 joyGetPosEx(Int32 uJoyID, ref JOYINFOEX pji);

        [DllImport("winmm.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 joyGetThreshold(Int32 uJoyID, IntPtr puThreshold);

        [DllImport("winmm.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 joyReleaseCapture(Int32 uJoyID);

        [DllImport("winmm.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 joySetCapture(int hwnd, Int32 uJoyID, Int32 uPeriod, bool fChanged);

        [DllImport("winmm.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 joySetThreshold(Int32 uJoyID, Int32 uThreshold);

        #endregion JOYSTICK API
    }

    class Joystick
    {
        private IntPtr m_JoyNbr = IntPtr.Zero;
        private Win32API.JOYINFOEX m_JEX = new Win32API.JOYINFOEX();
        private Win32API.JOYCAPS m_JCAPS = new Win32API.JOYCAPS();


        public int MinX
        {
            get { return m_JCAPS.wXmin; }
        }

        public int MaxX
        {
            get { return m_JCAPS.wXmax; }
        }

        public int MinY
        {
            get { return m_JCAPS.wYmin; }
        }

        public int MaxY
        {
            get { return m_JCAPS.wYmax; }
        }

        

        private uint m_Buttons;

        private double m_PosX;


        public double PosX
        {
            get {
                m_JEX.dwSize = 64;
                m_JEX.dwFlags = Win32API.JOY_RETURNXY | Win32API.JOY_RETURNBUTTONS;
                if (Win32API.joyGetPosEx(m_JoyNbr.ToInt32(), ref m_JEX) == 0)
                {
                    m_PosX = (double)(m_JEX.dwXpos - m_CenterX)*2 / (MaxX-MinX);
                    m_PosY = (double)(m_JEX.dwYpos - m_CenterY)*2 / (MaxY-MinY);
                    m_Buttons = (uint)m_JEX.dwButtons;
                }
                return m_PosX; 
            }
        }
     
        private double m_PosY;

        public double PosY
        {
            get { return m_PosY; }
        }

        public uint ButtonState
        {
            get { return m_Buttons; }
        }

        public bool IsButtonPressed(int nbr)
        {
            return (m_Buttons & (1 << nbr)) != 0;
        }

        private int m_CenterX = 0;
        private int m_CenterY = 0;

        /// <summary>
        /// get a list of valid joystic ID's connected to the system,
        /// null if none.
        /// </summary>
        public static int [] Joysticks
        {
            get {
                Win32API.JOYINFOEX jex = new Win32API.JOYINFOEX();

                jex.dwSize = 64;
                jex.dwFlags = Win32API.JOY_RETURNXY | Win32API.JOY_RETURNBUTTONS;

                List<int> id_list = new List<int>();

                for (int i = 0; i < Win32API.joyGetNumDevs(); ++i)
                    if (Win32API.joyGetPosEx(i, ref jex) == 0) id_list.Add(i);

                if (id_list.Count > 0) 
                    return id_list.ToArray();
                else
                    return null;
            }
        }

        public static string[] JoystickNames
        {
            get
            {

                Win32API.JOYINFOEX jex = new Win32API.JOYINFOEX();

                jex.dwSize = 64;
                jex.dwFlags = Win32API.JOY_RETURNXY;

                List<string> id_list = new List<string>();
                Win32API.JOYCAPS jc = new Win32API.JOYCAPS();

                for (int i = 0; i < Win32API.joyGetNumDevs(); ++i)
                    if (Win32API.joyGetPosEx(i, ref jex) == 0) {
                        if (Win32API.joyGetDevCaps((IntPtr)i, ref jc, 404) == 0) 
                        id_list.Add(i.ToString() + ":" + jc.szPname);
                    }
                if (id_list.Count > 0)
                    return id_list.ToArray();
                else
                    return null;
            }
        }

        public bool Initialize(IntPtr jnbr)
        {
            m_JoyNbr = jnbr;

            if (Win32API.joyGetDevCaps(jnbr, ref m_JCAPS, 404) != 0) return false;
            m_JEX.dwSize = 64;
            m_JEX.dwFlags = Win32API.JOY_RETURNXY;
            if (Win32API.joyGetPosEx(m_JoyNbr.ToInt32(), ref m_JEX) != 0) return false;
            m_CenterX = m_JEX.dwXpos;
            m_CenterY = m_JEX.dwYpos;

            return true;
        }

        public bool Initialize(string name)
        {
            string[] names = JoystickNames;
            int idx = Array.IndexOf(names, name);
            if (idx >= 0)
                return this.Initialize((IntPtr)idx);

            // if didn't match the name, just use the first available joystick port:
            if (names.Length > 0)
                return this.Initialize((IntPtr)0);  

            return false;
        }

#if false
        static List<object> _Joysticks = new List<object>();

        public static List<object> JoySticks
        {
            get
            {
                    _Joysticks = new List<object>();
                    DeviceList devices = Manager.GetDevices(DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly);
                    if (devices != null)
                    {
                        foreach (DeviceInstance deviceInstance in devices)
                        {
                            _Joysticks.Add(deviceInstance);
                            Device device = new Device(deviceInstance.InstanceGuid);
                            device.SetCooperativeLevel(null, CooperativeLevelFlags.Background | CooperativeLevelFlags.NonExclusive);
                            device.SetDataFormat(DeviceDataFormat.Joystick);
                            device.Acquire();
                            JoystickState js = device.CurrentJoystickState;
                        }
                    }
                
                return _Joysticks;
            }
        }

#endif

    }
}

/*
    Copyright (C) 2019, Kristian Lamb

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.

    Module: CheckPid.cs

    Author: Kristian Lamb

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace pidchk
{
    class PidChecker
    {
        [DllImport("pidgenx.dll", EntryPoint = "PidGenX", CharSet = CharSet.Auto)]
        static extern int PidGenX(string productKey, string configFile, string mpc, int UnknownUsage, IntPtr productId2, IntPtr productId3, IntPtr productId4);

        internal string CheckProductKey(string productKey)
        {
            string result = "";
            int RetID;
            byte[] pid2 = new byte[0x32];
            byte[] pid3 = new byte[0xA4];
            byte[] pid4 = new byte[0x04F8];

            IntPtr PID = Marshal.AllocHGlobal(0x32);
            IntPtr DPID = Marshal.AllocHGlobal(0xA4);
            IntPtr DPID4 = Marshal.AllocHGlobal(0x04F8);

            string PKeyPath = Environment.SystemDirectory + @"\spp\tokens\pkeyconfig\pkeyconfig.xrm-ms";
            string MSPID = "XXXXX";

            pid2[0] = 0x32;
            pid3[0] = 0xA4;
            pid4[0] = 0xF8;
            pid4[1] = 0x04;

            Marshal.Copy(pid2, 0, PID, 0x32);
            Marshal.Copy(pid3, 0, DPID, 0xA4);
            Marshal.Copy(pid4, 0, DPID4, 0x04F8);

            RetID = PidGenX(productKey, PKeyPath, MSPID, 0, PID, DPID, DPID4);

            if (RetID == 0)
            {
                Marshal.Copy(PID, pid2, 0, pid2.Length);
                Marshal.Copy(DPID4, pid4, 0, pid4.Length);
                string id = GetString(pid4, 0x0008);
                string act = GetString(pid4, 0x0088);
                string edi = GetString(pid4, 0x0118);
                string edid = GetString(pid4, 0x0378);
                string lit = GetString(pid4, 0x03F8);
                string pchan = GetString(pid4, 0x0478);
                Console.WriteLine(" Product Key:         = {0}",productKey);
                Console.WriteLine(" Key Status:          = Valid");
                Console.WriteLine(" Extended PID:        = " + id);
                Console.WriteLine(" Activation ID:       = " + act);
                Console.WriteLine(" Edition Type:        = " + edi);
                Console.WriteLine(" Edition ID:          = " + edid);
                Console.WriteLine(" License Type:        = " + lit);
                Console.WriteLine(" License Channel:     = " + pchan);

                result = "S_OK";
                
            }
            else if (RetID == -2147024809)
            {
                result = " [ERR] PidGenX :: Invalid Arguments.";
            }
            else if (RetID == -1979645695)
            {
                result = " [ERR] PidGenX :: Not a Windows Product Key.";
            }
            else if (RetID == -2147024894)
            {
                result = " [ERR] PidGenX :: pkeyconfig.xrm.ms file not found.";
            }
            else
            {
                result = " [ERR] PidGenX :: Unkown Error.";
            }
            Marshal.FreeHGlobal(PID);
            Marshal.FreeHGlobal(DPID);
            Marshal.FreeHGlobal(DPID4);
            //FreeLibrary(dllHandle);
            return result;
        }

        string GetString(byte[] bytes, int index)
        {
            int n = index;
            while (!(bytes[n] == 0 && bytes[n + 1] == 0)) n++;
            return Encoding.ASCII.GetString(bytes, index, n - index).Replace("\0", "");
        }
    }
}
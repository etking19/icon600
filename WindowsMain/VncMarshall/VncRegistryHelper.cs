using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VncMarshall
{
    public class VncRegistryHelper
    {
        private static string sPath = "HKEY_CURRENT_USER\\Software\\TightVNC\\Server";

        public static void SetServerPort(int portNum)
        {
            Registry.SetValue(sPath, "RfbPort", portNum);
        }

        public static int GetServerPort()
        {
            int port = -1;
            port = (int)Registry.GetValue(sPath, "RfbPort", port);

            return port;
        }

        public static void RemoveWallpaper(bool allow)
        {
            Registry.SetValue(sPath, "RemoveWallpaper", Convert.ToInt32(allow));
        }

        public static void EnableMirrorDriver(bool enable)
        {
            Registry.SetValue(sPath, "UseMirrorDriver", Convert.ToInt32(enable));
        }
    }
}

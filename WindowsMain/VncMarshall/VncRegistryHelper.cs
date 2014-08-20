using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace VncMarshall
{
    public class VncRegistryHelper
    {
        private static string sPath = @"software\TightVNC\Server";

        private static string sServerListeningPort = "RfbPort";
        private static string sServerExtraPorts = "ExtraPorts";

        public static int GetListeningPort()
        {
            int port = 0;

            try
            {
                port = (int)GetRegistryValue(sPath, sServerListeningPort);
            }
            catch (Exception)
            {
            }
            
            return port;
        }

        public static int[] GetExtraListeningPorts()
        {
            List<int> ports = new List<int>();

            try
            {
                string extraPortStr = (string)GetRegistryValue(sPath, sServerExtraPorts);
                if (extraPortStr != null && extraPortStr.Length != 0)
                {
                    if (extraPortStr.Length != 0)
                    {
                        // format eg: 5901:640x480+0+0,5902:444x444+0+0
                        // split the ","
                        string[] extraStrs = extraPortStr.Split(',');

                        // split the ":" to get the ports
                        foreach (string portStr in extraStrs)
                        {
                            string[] extraPorts = portStr.Split(':');
                            // should have two entries, first is port number, second is area
                            ports.Add(int.Parse(extraPorts[0]));
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            return ports.ToArray();
        }

        public static void AddExtraListeningPorts(int listeningPort, int left, int top, int width, int height)
        {
            // format eg: 5901:640x480+0+0,5902:444x444+0+0
            // port:width+height+left+top
            try
            {
                string extraPortStr = (string)GetRegistryValue(sPath, sServerExtraPorts);
                if (extraPortStr.Length == 0)
                {
                    extraPortStr += String.Format("{0}:{1}x{2}+{3}+{4}", listeningPort, width, height, left, top);
                }
                else
                {
                    extraPortStr += String.Format(",{0}:{1}x{2}+{3}+{4}", listeningPort, width, height, left, top);
                }
                

                RegistryKey key = GetRegistryKey(sPath);
                key.SetValue(sServerExtraPorts, extraPortStr, RegistryValueKind.String);
            }
            catch (Exception)
            {
            }
            
        }

        public static void RemoveExtrasListeningPort()
        {
            try
            {
                RegistryKey key = GetRegistryKey(sPath);
                key.SetValue(sServerExtraPorts, String.Empty, RegistryValueKind.String);
            }
            catch (Exception)
            {
            }
            
        }

        private static RegistryKey GetRegistryKey()
        {
            return GetRegistryKey(null);
        }

        private static RegistryKey GetRegistryKey(string keyPath)
        {
            RegistryKey localMachineRegistry
                = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
                                          Environment.Is64BitOperatingSystem
                                              ? RegistryView.Registry64
                                              : RegistryView.Registry32);

            return string.IsNullOrEmpty(keyPath)
                ? localMachineRegistry
                : localMachineRegistry.OpenSubKey(keyPath, true);
        }

        private static object GetRegistryValue(string keyPath, string keyName)
        {
            RegistryKey registry = GetRegistryKey(keyPath);

            try
            {
                return registry.GetValue(keyName);
            }
            catch (Exception)
            {
                return null;
            }
            
        }
    }
}

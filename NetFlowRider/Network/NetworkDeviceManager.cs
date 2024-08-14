using Microsoft.Win32;
using SharpPcap;
using SharpPcap.LibPcap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NetFlowRider.Network {
    public static class NetworkDeviceManager {
        public static List<NetworkDevice> NetworkDevices { get; private set; }

        public static void Init() {
            NetworkDevices = new List<NetworkDevice>();
            RefreshAdapters();
        }

        private static void RefreshAdapters() {
            CaptureDeviceList allDevices;
            try {
                allDevices = CaptureDeviceList.Instance;
            } catch (Exception ex) {
                // no npcap
                Environment.Exit(1);
                return;
            }

            foreach (var device in allDevices) {
                var friendlyName = FindFriendlyName(device);
                if (friendlyName == "?" || friendlyName.StartsWith("?")) {
                    continue;
                }

                var ethCatcher = new EthCatcherPcap(device);
                var ipAddress = GetDeviceIpAddress(device);

                if (ipAddress != null) {
                    var networkDevice = new NetworkDevice(ethCatcher, friendlyName, ipAddress);
                    NetworkDevices.Add(networkDevice);
                }
            }
        }

        private static IPAddress GetDeviceIpAddress(ICaptureDevice device) {
            if (device is LibPcapLiveDevice liveDevice) {
                foreach (var address in liveDevice.Addresses) {
                    if (address.Addr.ipAddress != null &&
                        address.Addr.ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) {
                        return address.Addr.ipAddress; // Return the first IPv4 address found
                    }
                }
            }
            return null; // No valid IP address found
        }

        private static string FindFriendlyName(ICaptureDevice dev) {
            var regexGuid = new Regex(@"{[a-zA-Z0-9-]+}");
            var match = regexGuid.Match(dev.Name);

            if (!match.Success) {
                return "?";
            }

            string guid = match.Value;
            string keyName = $"SYSTEM\\CurrentControlSet\\Control\\Network\\{{4D36E972-E325-11CE-BFC1-08002BE10318}}\\{guid}\\Connection";

            try {
                using (var key = Registry.LocalMachine.OpenSubKey(keyName)) {
                    if (key == null) {
                        return "??";
                    }
                    object oname = key.GetValue("Name");
                    if (oname == null) {
                        return "???";
                    }
                    return oname.ToString();
                }
            } catch (Exception ex) {
                return "????";
            }
        }
    }
}

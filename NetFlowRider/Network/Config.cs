using SharpPcap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFlowRider.Network{
    public class Config{
        public static List<ICaptureDevice> CaptureDevices { get; private set; }

        public static void InitConfig(){
            NetworkDeviceManager.Init();
        }

        private static void initAdapters() {
            CaptureDevices = new List<ICaptureDevice>(CaptureDeviceList.Instance);
        }
    }
}

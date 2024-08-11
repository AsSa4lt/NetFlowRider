using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetFlowRider.Network {
    public class NetworkDevice {
        private EthCatcherPcap _catcher;
        public string Name { get; private set; }
        public IPAddress Address { get; private set; }


        public NetworkDevice(EthCatcherPcap catcher) {
            _catcher = catcher;
        }

    }
}

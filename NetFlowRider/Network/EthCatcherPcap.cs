using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpPcap;
using SharpPcap.LibPcap;

namespace NetFlowRider.Network {
    public class EthCatcherPcap {
        private readonly PcapDevice _device;
        private readonly ConcurrentQueue<RawCapture> _sendQueue;
        private readonly object _lock = new object();
        private bool isRunning;
        private readonly Task _senderTask;
    }
}

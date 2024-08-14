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
        private readonly ICaptureDevice _device;
        private readonly ConcurrentQueue<RawCapture> _sendQueue;
        private readonly object _lock = new object();
        private bool _isRunning;
        Thread _senderThread;
        public EthCatcherPcap(ICaptureDevice device) {
            _device = device;

            _device.OnPacketArrival += new PacketArrivalEventHandler(OnPacketArrival);
            _device.Open(DeviceModes.Promiscuous, 1000);

        }

        public void Start() {
            _isRunning = true;
            _senderThread = new Thread(packetSender);
            _senderThread.Start();
            _device.StartCapture();
        }

        public void Stop() {
            _isRunning = false;
            _device.StopCapture();
            _senderThread?.Join();
        }

        private void OnPacketArrival(object sender, PacketCapture e) {
            RawCapture rawCapture = e.GetPacket();
            Console.WriteLine($"Raw packet recieved: {BitConverter.ToString(rawCapture.Data)}");
        }

        private void packetSender() {
            while (true) {
                bool isStillRunning;
                lock (_lock) {
                    isStillRunning = _isRunning;
                }

                if (!isStillRunning) {
                    break;
                }

                if (_sendQueue.TryDequeue(out RawCapture rawPacket)) {
                    lock (_lock) {
                        if (_device is IInjectionDevice injector) {
                            injector.SendPacket(rawPacket);
                            Console.WriteLine($"Raw packet sent: {BitConverter.ToString(rawPacket.Data)}");
                        }
                    }
                }
            }
        }


        public void SendPacket(RawCapture rawCapture) {
            _sendQueue.Enqueue(rawCapture);
        }
    }
}

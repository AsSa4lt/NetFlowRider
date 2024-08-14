using NetFlowRider.Network;
using SharpPcap;

namespace NetFlowRider {
    public partial class MainPage : ContentPage {
        int count = 0;

        public MainPage() {
            InitializeComponent();
            UpdateAdapters();
        }

        private void OnCounterClicked(object sender, EventArgs e) {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private void UpdateAdapters() {
            var stackLayoutAdapters = new StackLayout();
            List<NetworkDevice> captureDevices = NetworkDeviceManager.NetworkDevices;

            // Create and add checkboxes 
            foreach (var captureDevice in captureDevices) {
                var checkBox = new CheckBox {
                    Color = Colors.Black
                };

                // Optionally handle CheckedChanged event
                checkBox.CheckedChanged += (sender, e) => {
                    // Handle the event here if needed
                };

                // Create a Label
                var label = new Label {
                    Text = captureDevice.Name + " " + captureDevice.Address.ToString(),
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Colors.Black
                };

                // Create a horizontal StackLayout to hold the CheckBox and Label
                var checkBoxLayout = new StackLayout {
                    Orientation = StackOrientation.Horizontal,
                    Children = { checkBox, label },
                };

                // Add the horizontal StackLayout to the main StackLayout
                stackLayoutAdapters.Children.Add(checkBoxLayout);
            }

            // Assuming adaptersList is a ContentView or similar container
            adaptersList.Content = stackLayoutAdapters;
        }
    }

}

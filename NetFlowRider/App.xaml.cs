using NetFlowRider.Network;

namespace NetFlowRider
{
    public partial class App : Application {
        public App() {
            InitializeComponent();
            Config.InitConfig();
            MainPage = new AppShell();
        }
        
    }
}

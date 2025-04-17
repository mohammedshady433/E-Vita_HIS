using E_Vita.Views;

namespace E_Vita
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(PassReset), typeof(PassReset));
            Routing.RegisterRoute(nameof(NurseDashboard), typeof(NurseDashboard));
            Routing.RegisterRoute(nameof(Patient_info), typeof(Patient_info));
            Routing.RegisterRoute(nameof(NewPage1), typeof(NewPage1));

        }
    }
}

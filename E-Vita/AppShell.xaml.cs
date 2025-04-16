namespace E_Vita
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Routing.RegisterRoute(nameof(PassReset), typeof(PassReset));
            Routing.RegisterRoute(nameof(Patientinfo), typeof(Patientinfo));

        }
    }
}

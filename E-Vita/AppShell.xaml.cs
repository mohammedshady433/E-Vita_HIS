namespace E_Vita
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(PassReset), typeof(PassReset));
        }
    }
}

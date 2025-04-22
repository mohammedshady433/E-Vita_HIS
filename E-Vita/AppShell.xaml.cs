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
            Routing.RegisterRoute(nameof(DoctorDashboard), typeof(DoctorDashboard));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(ReceptionistDashboard), typeof(ReceptionistDashboard));
            Routing.RegisterRoute(nameof(BookAppointment), typeof(BookAppointment));
            Routing.RegisterRoute(nameof(AddPatient), typeof(AddPatient));
            Routing.RegisterRoute(nameof(AddDoctor), typeof(AddDoctor));
            Routing.RegisterRoute(nameof(Patient_data), typeof(Patient_data));
            Routing.RegisterRoute(nameof(LabImages), typeof(LabImages));
            Routing.RegisterRoute(nameof(Prescription), typeof(Prescription));
            Routing.RegisterRoute(nameof(OperationRoomReservation), typeof(OperationRoomReservation));
            Routing.RegisterRoute(nameof(Finance), typeof(Finance));

        }
    }
}

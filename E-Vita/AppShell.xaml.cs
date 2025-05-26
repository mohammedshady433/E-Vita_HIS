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
            Routing.RegisterRoute(nameof(LabImages), typeof(LabImages));
            Routing.RegisterRoute(nameof(Prescription), typeof(Prescription));
            Routing.RegisterRoute(nameof(OperationRoomReservation), typeof(OperationRoomReservation));
            Routing.RegisterRoute(nameof(Finance), typeof(Finance));
            Routing.RegisterRoute(nameof(CancelAppointment), typeof(CancelAppointment));
            Routing.RegisterRoute(nameof(Room_Reservation), typeof(Room_Reservation));
            Routing.RegisterRoute(nameof(AddNurse), typeof(AddNurse));
            Routing.RegisterRoute(nameof(InpatientDoctorDashboard), typeof(InpatientDoctorDashboard));
            Routing.RegisterRoute(nameof(WardRounds), typeof(WardRounds));
            Routing.RegisterRoute(nameof(DischargePlanning), typeof(DischargePlanning));
            Routing.RegisterRoute(nameof(RoomReservationForm), typeof(RoomReservationForm));
            Routing.RegisterRoute(nameof(InpatientOrdersPage), typeof(InpatientOrdersPage));
            Routing.RegisterRoute(nameof(PatientSearch), typeof(PatientSearch));

        }
    }
}

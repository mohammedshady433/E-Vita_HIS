namespace E_Vita.Views;

public partial class Room_Reservation : ContentPage
{
	List<int> Rooms = new List<int>();
	List<int> Reserved_Rooms = new List<int>();
    public Room_Reservation()
	{
		InitializeComponent();
	}

    private void Save(object sender, EventArgs e)
    {
		string pass = "123";

		if (pass == Pass.Text)
		{
			for(int i= 1; i< int.Parse(count.Text); i++)
			{
				Rooms.Add(i + 100);

			}
            avialable_rooms.ItemsSource = Rooms;
        }

    }

    private void SavePatient(object sender, EventArgs e)
    {
		Rooms.Remove((int)avialable_rooms.SelectedItem);
        Reserved_Rooms.Add((int)avialable_rooms.SelectedItem);
		avialable_rooms.ItemsSource = Rooms;
    }

    private async void close(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ReceptionistDashboard));
    }
}
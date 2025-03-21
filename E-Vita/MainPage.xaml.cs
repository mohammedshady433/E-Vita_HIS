namespace E_Vita
{
    public partial class MainPage : ContentPage
    {
        double screenWidth;
        double screenHeight;
        double windowWidth = 350; // Example value, replace with actual window width
        double windowHeight = 390; // Example value, replace with actual window height
        double x;
        double y;

        public MainPage()
        {
            InitializeComponent();
            screenWidth = DeviceDisplay.MainDisplayInfo.Width;
            screenHeight = DeviceDisplay.MainDisplayInfo.Height;

            // Calculate the position to center the window
            x = (screenWidth - windowWidth) / 2;
            y = (screenHeight - windowHeight) / 2;
        }

        private async void login(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(PassReset));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                System.Diagnostics.Debug.WriteLine($"Navigation error: {ex}");
            }
        }

        private void Forgot_pass(object sender, EventArgs e)
        {
            // Create a new window
            var newWindow = new Window
            {
                X = x-200, // Set the x position
                Y = y+15, // Set the y position
                Width = windowWidth, // Set the width
                Height = windowHeight, // Set the height
                Title = "Reset Password" // Optional: Set the window title
            };

            // Create a new page (or use an existing one) to set as the content of the window
            //var newPage = new ContentPage
            //{
            //    Content = new Label { Text = "This is a new window!" }
            //};

            // Set the page as the content of the new window
            newWindow.Page = new PassReset();

            // Open the new window
            Application.Current.OpenWindow(newWindow);
            //Application.Current?.OpenWindow(new Window(new PassReset()));
        }
    }
}

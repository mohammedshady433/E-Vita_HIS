namespace E_Vita
{
    public partial class LabImages : ContentPage
    {
        public LabImages()
        {
            InitializeComponent();
        }

        // Search button
        private void OnSearchClicked(object sender, EventArgs e)
        {
            var patientID = PatientIDEntry.Text;

            // Check if the entry is empty
            if (string.IsNullOrEmpty(patientID))
            {
                DisplayAlert("Error", "Please enter a Patient ID.", "OK");
                return;
            }


            // If it's valid, proceed with the search
            DisplayAlert("Search Result", $"You entered: {patientID}", "OK");
        }

        // Upload new image
        private async void Upload(object sender, EventArgs e)
        {
            try
            {
                // Pick an image from the device
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Please select an image",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    var stream = await result.OpenReadAsync();

                    // Create an Image control dynamically
                    var image = new Image
                    {
                        Source = ImageSource.FromStream(() => stream),
                        HeightRequest = 200,
                        WidthRequest = 200,
                        Aspect = Aspect.AspectFit
                    };

                    // Add the image to the grid
                    AddImageToGrid(image);
                }
            }
            catch (Exception ex)
            {
                // Handle any errors
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        // Add image to grid dynamically
        private void AddImageToGrid(Image image)
        {
            // Get the current row count of the grid
            int currentRowCount = ImagesGrid.RowDefinitions.Count;

            // Calculate the number of rows based on images
            int columnCount = 3; // Adjust based on how many images you want per row

            int rowIndex = currentRowCount / columnCount; // Calculate row based on the number of images
            int columnIndex = currentRowCount % columnCount; // Determine which column to place the image in

            // Create a new row if needed
            if (columnIndex == 0)
            {
                ImagesGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            // Place the image in the appropriate row and column
            Grid.SetRow(image, rowIndex);
            Grid.SetColumn(image, columnIndex);

            // Add the image to the grid
            ImagesGrid.Children.Add(image);
        }

        // Handle checkbox changes
        private void OnCheckboxChanged(object sender, CheckedChangedEventArgs e)
        {
            if (!(sender is CheckBox changedCheckBox) || !changedCheckBox.IsChecked)
                return;

            // Uncheck all other checkboxes except the one just checked
            if (changedCheckBox != RadiologyCheckBox)
                RadiologyCheckBox.IsChecked = false;

            if (changedCheckBox != DermatologyCheckBox)
                DermatologyCheckBox.IsChecked = false;

            if (changedCheckBox != LaserTherapyCheckBox)
                LaserTherapyCheckBox.IsChecked = false;
        }
    }
}


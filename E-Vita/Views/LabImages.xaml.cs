using E_Vita.Services;
using E_Vita_APIs.Models;
using System.Collections.ObjectModel;
using System.IO;

namespace E_Vita.Views
{
    public partial class LabImages : ContentPage
    {
        private readonly RadiologyService _radiologyService = new RadiologyService();
        public ObservableCollection<ImageSource> Images { get; set; } = new ObservableCollection<ImageSource>();

        public LabImages()
        {
            InitializeComponent();
            BindingContext = this;
        }

        // Search button
        private async void OnSearchClicked(object sender, EventArgs e)
        {
            var patientID = PatientIDEntry.Text;

            if (string.IsNullOrEmpty(patientID) || !int.TryParse(patientID, out int patientId))
            {
                await DisplayAlert("Error", "Please enter a valid Patient ID.", "OK");
                return;
            }

            await LoadPatientImages(patientId);
        }

        // Load images for the patient from the API
        private async Task LoadPatientImages(int patientId)
        {
            try
            {
                Images.Clear();
                var allRadiology = await _radiologyService.GetAllAsync();
                var patientImages = allRadiology
                    .Where(r => r.PatientId == patientId && r.Photo != null && r.Photo.Length > 0)
                    .ToList();

                if (!patientImages.Any())
                {
                    await DisplayAlert("Info", "No lab images found for this patient.", "OK");
                    return;
                }

                foreach (var radiology in patientImages)
                {
                    // Convert byte[] to ImageSource
                    ImageSource imgSource = ImageSource.FromStream(() => new MemoryStream(radiology.Photo));
                    Images.Add(imgSource);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load images: {ex.Message}", "OK");
            }
        }

        // Upload new image
        private async void Upload(object sender, EventArgs e)
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Please select an image",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    using var stream = await result.OpenReadAsync();
                    using var ms = new MemoryStream();
                    await stream.CopyToAsync(ms);
                    var imageBytes = ms.ToArray();

                    // Get patient ID from entry
                    if (!int.TryParse(PatientIDEntry.Text, out int patientId))
                    {
                        await DisplayAlert("Error", "Please enter a valid Patient ID before uploading.", "OK");
                        return;
                    }

                    // Create a new Radiology object
                    var newRadiology = new Radiology
                    {
                        PatientId = patientId,
                        Photo = imageBytes,
                        Date = DateTime.Now,
                        Note = "Uploaded from MAUI app"
                    };

                    // Save to API
                    var success = await _radiologyService.AddAsync(newRadiology);
                    if (success)
                    {
                        await DisplayAlert("Success", "Image uploaded successfully.", "OK");
                        // Refresh images
                        await LoadPatientImages(patientId);
                    }
                    else
                    {
                        await DisplayAlert("Error", "Failed to upload image.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
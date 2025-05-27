using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using E_Vita_APIs.Models;
using E_Vita.Services;
using Microsoft.Maui.Controls;

namespace E_Vita.Views
{
    public partial class LabImages : ContentPage
    {
        private readonly RadiologyService _radiologyService = new();
        private int _currentPatientId=0;

        public LabImages()
        {
            InitializeComponent();
            SetupGridColumns();
        }

        private void SetupGridColumns()
        {
            ImagesGrid.ColumnDefinitions.Clear();
            for (int i = 0; i < 3; i++)
            {
                ImagesGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        private async void OnSearchClicked(object sender, EventArgs e)
        {
            if (!int.TryParse(PatientIdEntry.Text, out int patientId))
            {
                await DisplayAlert("Error", "Invalid Patient ID", "OK");
                return;
            }

            _currentPatientId = patientId;
            await LoadImages();
        }

        private async Task LoadImages()
        {
            try
            {
                ImagesGrid.Children.Clear();
                ImagesGrid.RowDefinitions.Clear();

                var allImages = await _radiologyService.GetAllAsync();
                if (allImages == null)
                {
                    await DisplayAlert("Error", "Failed to retrieve images", "OK");
                    return;
                }

                var images = allImages.Where(img => img.PatientId == _currentPatientId).ToList();

                if (images.Count == 0)
                {
                    await DisplayAlert("Info", "No images found", "OK");
                    return;
                }

                int columnCount = 3;
                int currentIndex = 0;

                foreach (var image in images)
                {
                    if (currentIndex % columnCount == 0)
                    {
                        ImagesGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    }

                    var img = new Image
                    {
                        Source = ImageSource.FromStream(() => new MemoryStream(image.Photo)),
                        Aspect = Aspect.AspectFit,
                        HeightRequest = 400,
                        WidthRequest = 400
                    };

                    Grid.SetRow(img, currentIndex / columnCount);
                    Grid.SetColumn(img, currentIndex % columnCount);
                    ImagesGrid.Children.Add(img);

                    currentIndex++;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void Upload(object sender, EventArgs e)
        {
            try
            {
                if (_currentPatientId == 0)
                {
                    await DisplayAlert("Error", "Search for patient first", "OK");
                    return;
                }

                var result = await FilePicker.Default.PickAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Images
                });

                if (result == null) return;

                using var stream = await result.OpenReadAsync();
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                Radiology radiology = new Radiology();
                radiology.PatientId = _currentPatientId;
                radiology.Photo = memoryStream.ToArray();
                radiology.Date = DateTime.Now;
                radiology.Note = "no notes";
                var success = await _radiologyService.AddAsync(radiology);

                if (success)
                {
                    await DisplayAlert("Success", "Image uploaded", "OK");
                    await LoadImages();
                }
                else
                {
                    await DisplayAlert("Error", "Upload failed", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}

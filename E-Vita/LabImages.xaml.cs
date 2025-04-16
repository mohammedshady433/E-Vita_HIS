using Microsoft.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.IO;

namespace E_Vita.Views;

public partial class LabImages : ContentPage
{
    

    public ObservableCollection<LabTestViewModel> labsofthepatient { get; set; } = new();

    public LabImages()
    {
        InitializeComponent();
        BindingContext = this;

        var services = ((App)Application.Current)._serviceProvider;
      
    }

    private async void OnSearchClicked(object sender, EventArgs e)
    {
        if (int.TryParse(SearchText.Text, out int patientId))
        {
            await LoadLabTests(patientId);
        }
        else
        {
            await DisplayAlert("Invalid Input", "Please enter a valid patient ID.", "OK");
        }
    }

    private async Task LoadLabTests(int patientId)
    {
        var allImages = await _LabTestRepo.GetAllAsync();
        var filtered = allImages.Where(x => x.PatientId == patientId);

        labsofthepatient.Clear();
        foreach (var lab in filtered)
        {
            labsofthepatient.Add(new LabTestViewModel
            {
                ImageSource = LoadImage(lab.ImageData)
            });
        }
    }

    private ImageSource LoadImage(byte[] imageData)
    {
        if (imageData == null || imageData.Length == 0) return null;
        return ImageSource.FromStream(() => new MemoryStream(imageData));
    }
}

public class LabTestViewModel
{
    public ImageSource ImageSource { get; set; }
}

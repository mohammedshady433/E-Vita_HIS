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

        //var services = ((App)Application.Current)._serviceProvider;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {

    }
}

public class LabTestViewModel
{
    public ImageSource ImageSource { get; set; }
}
namespace E_Vita.Views;

public partial class Patient_data : ContentPage
{
    int count = 0;

    public Patient_data()
    {
        InitializeComponent();
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;

        //if (count == 1)
        //    CounterBtn.Text = $"Clicked {count} time";
        //else
        //  CounterBtn.Text = $"Clicked {count} times";

        // SemanticScreenReader.Announce(CounterBtn.Text);
    }
}



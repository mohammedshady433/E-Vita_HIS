using E_Vita.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Vita.Views
{
    public partial class Prescription : ContentPage
    {
        private readonly Disease _diseaseVar;
        private List<Disease.NodeWithMeta> _nodesWithMeta;

        private readonly Drug _drugvar;
        private List<Drug> _druglist;
        //this store the selected drugs to use it with the pharma app
        private List<Drug> _selectedDrugs;

        public Prescription()
        {
            InitializeComponent();
            _diseaseVar = new Disease();
            SuggestionsList.SelectionChanged += SuggestionsList_SelectionChanged;

            _drugvar = new Drug();
            _selectedDrugs = new List<Drug>();
            SuggestionsListofDrugs.SelectionChanged += SuggestionsList_SelectionChangedforDrugs;

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadDataAsync();
            await LoadDrugDataAsync();
        }

        private async Task LoadDataAsync()
        {
            _nodesWithMeta = await _diseaseVar.LoadDataWithMetaAsync();
            if (_nodesWithMeta == null || !_nodesWithMeta.Any())
            {
                await DisplayAlert("Error", "Failed to load disease data. Please check the JSON file.", "OK");
            }
            else
            {
                SuggestionsList.ItemsSource = _nodesWithMeta;
            }
        }

        private void SearchEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is Entry entry)
            {
                var keyword = e.NewTextValue?.ToLowerInvariant() ?? "";
                var filtered = _nodesWithMeta
                    .Where(s => s.Node.Label != null && s.Node.Label.ToLowerInvariant().Contains(keyword))
                    .ToList();

                // Find the corresponding CollectionView (based on naming or tagging)
                var collectionView = entry == SearchEntry ? SuggestionsList : FindCollectionViewForEntry(entry);
                if (collectionView != null)
                {
                    collectionView.ItemsSource = filtered;
                    collectionView.IsVisible = filtered.Any();
                }
            }
        }

        private CollectionView FindCollectionViewForEntry(Entry entry)
        {
            // For the main search entries
            if (entry == SearchEntry)
                return SuggestionsList;
            if (entry == SearchEntryforDrugs)
                return SuggestionsListofDrugs;

            // Check in DiseaseNotesContainer
            var index = DiseaseNotesContainer.Children.IndexOf(entry);
            if (index >= 0 && index + 1 < DiseaseNotesContainer.Children.Count)
            {
                var nextChild = DiseaseNotesContainer.Children[index + 1];
                if (nextChild is CollectionView collectionView)
                {
                    return collectionView;
                }
            }

            // Check in DrugNotesContainer
            index = DrugNotesContainer.Children.IndexOf(entry);
            if (index >= 0 && index + 1 < DrugNotesContainer.Children.Count)
            {
                var nextChild = DrugNotesContainer.Children[index + 1];
                if (nextChild is CollectionView collectionView)
                {
                    return collectionView;
                }
            }

            return null;
        }


        private void OnSuggestionTapped(object sender, EventArgs e)
        {
            if (sender is Label label && label.BindingContext is Disease.NodeWithMeta nodeWithMeta)
            {
                // Find the Entry associated with the CollectionView
                var collectionView = label.FindAncestor<CollectionView>();
                var entry = FindEntryForCollectionView(collectionView);
                if (entry != null)
                {
                    entry.Text = nodeWithMeta.Node.Label;
                    collectionView.IsVisible = false;
                }

                try
                {
                    DisplayAlert("Remember", $"Description: {nodeWithMeta.Meta.Definition.Value}", "I know");
                }
                catch (Exception ex)
                {
                    DisplayAlert("Sorry!", $"Unfortunately the description of {nodeWithMeta.Node.Label} isn't available", "I know");
                }
            }
        }

        private Entry FindEntryForCollectionView(CollectionView collectionView)
        {
            // For the main collection views
            if (collectionView == SuggestionsList)
                return SearchEntry;
            if (collectionView == SuggestionsListofDrugs)
                return SearchEntryforDrugs;

            // Check in DiseaseNotesContainer
            var index = DiseaseNotesContainer.Children.IndexOf(collectionView);
            if (index > 0 && DiseaseNotesContainer.Children[index - 1] is Entry diseaseEntry)
            {
                return diseaseEntry;
            }

            // Check in DrugNotesContainer
            index = DrugNotesContainer.Children.IndexOf(collectionView);
            if (index > 0 && DrugNotesContainer.Children[index - 1] is Entry drugEntry)
            {
                return drugEntry;
            }

            return null;
        }


        private void SuggestionsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Disease.NodeWithMeta selectedNode)
            {
                SearchEntry.Text = selectedNode.Node.Label;
                SuggestionsList.IsVisible = false;
            }
        }


        private void OnAddDiseaseNoteClicked(object sender, EventArgs e)
        {
            // Create a new Entry for the disease note
            var newDiseaseEntry = new Entry
            {
                Placeholder = "Search disease by name...",
                WidthRequest = 300,
                Margin = new Thickness(5),
                BackgroundColor = Color.FromArgb("#FFECECE4"),
                TextColor = Colors.Black
            };

            // Attach TextChanged event for search functionality
            newDiseaseEntry.TextChanged += SearchEntry_TextChanged;

            // Create a new CollectionView
            var newCollectionView = new CollectionView
            {
                IsVisible = false,
                SelectionMode = SelectionMode.Single,
                WidthRequest = 300,
                HeightRequest = 200,
                Margin = new Thickness(5)
            };

            // Define the ItemTemplate for the CollectionView
            newCollectionView.ItemTemplate = new DataTemplate(() =>
            {
                var label = new Label
                {
                    Padding = new Thickness(10),
                    BackgroundColor = Color.FromArgb("#EEE"),
                    TextColor = Colors.Black,
                    Margin = new Thickness(5)
                };
                label.SetBinding(Label.TextProperty, "Node.Label");
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += OnSuggestionTapped;
                label.GestureRecognizers.Add(tapGestureRecognizer);
                return label;
            });

            // Add the new Entry and CollectionView to the container
            DiseaseNotesContainer.Children.Add(newDiseaseEntry);
            DiseaseNotesContainer.Children.Add(newCollectionView);
        }

        /// Drug search functionality
        //////////////////////////////////////
        private async Task LoadDrugDataAsync()
        {
            try
            {
                _druglist = await _drugvar.LoadDrugs();
                if (_druglist == null || !_druglist.Any())
                {
                    await DisplayAlert("Error", "Failed to load drug data. Please check the JSON file.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load drug data: {ex.Message}", "OK");
            }
        }

        private async void SearchEntry_TextChangedforDrugs(object sender, TextChangedEventArgs e)
        {
            if (sender is Entry entry)
            {
                var keyword = e.NewTextValue?.ToLowerInvariant() ?? "";
                var filtered = _druglist
                    .Where(s => s.Tradename != null && s.Tradename.ToLowerInvariant().Contains(keyword))
                    .ToList();

                var collectionView = entry == SearchEntryforDrugs ? SuggestionsListofDrugs : FindCollectionViewForEntry(entry);
                if (collectionView != null)
                {
                    collectionView.ItemsSource = filtered;
                    collectionView.IsVisible = filtered.Any();
                }
            }
        }

        private async void OnSuggestionTappedforDrug(object sender, EventArgs e)
        {
            if (sender is Label label && label.BindingContext is Drug drug)
            {
                var collectionView = label.FindAncestor<CollectionView>();
                var entry = FindEntryForCollectionView(collectionView);
                if (entry != null)
                {
                    entry.Text = drug.Tradename; // Update the specific entry that triggered the search
                    collectionView.IsVisible = false;
                    _selectedDrugs.Add(drug);
                }

                try
                {
                    await DisplayAlert("Remember", $"Description: {drug.Pharmacology}\nPrice: {drug.new_price}", "I know");
                }
                catch (Exception)
                {
                    await DisplayAlert("Sorry!", "Error", "OK");
                }
            }
        }

        private void SuggestionsList_SelectionChangedforDrugs(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Drug tradname)
            {
                // Find the correct entry field
                var collectionView = sender as CollectionView;
                var entry = FindEntryForCollectionView(collectionView);

                if (entry != null)
                {
                    entry.Text = tradname.Tradename;
                    collectionView.IsVisible = false;
                    _selectedDrugs.Add(tradname);
                }
            }
        }

        private void OnAddDrugNoteClicked(object sender, EventArgs e)
        {
            var newDrugEntry = new Entry
            {
                Placeholder = "Search drugs by name...",
                WidthRequest = 300,
                Margin = new Thickness(5),
                BackgroundColor = Color.FromArgb("#FFECECE4"),
                TextColor = Colors.Black
            };

            newDrugEntry.TextChanged += SearchEntry_TextChangedforDrugs;

            var newCollectionView = new CollectionView
            {
                IsVisible = false,
                SelectionMode = SelectionMode.Single,
                WidthRequest = 300,
                HeightRequest = 200,
                Margin = new Thickness(5)
            };

            newCollectionView.ItemTemplate = new DataTemplate(() =>
            {
                var label = new Label
                {
                    Padding = new Thickness(10),
                    BackgroundColor = Color.FromArgb("#EEE"),
                    TextColor = Colors.Black,
                    Margin = new Thickness(5)
                };
                label.SetBinding(Label.TextProperty, "Tradename");
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += OnSuggestionTappedforDrug;
                label.GestureRecognizers.Add(tapGestureRecognizer);
                return label;
            });

            DrugNotesContainer.Children.Add(newDrugEntry);
            DrugNotesContainer.Children.Add(newCollectionView);
        }





        ///////////////////////////////////
        private void ShowSurgeriesCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            checkboxentry.IsVisible = e.Value;
        }

        private void ShowFamilyHistoryCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            checkboxentry2.IsVisible = e.Value;
        }
    }

    // Extension method to find ancestor in the visual tree
    public static class VisualElementExtensions
    {
        public static T FindAncestor<T>(this VisualElement element) where T : VisualElement
        {
            while (element != null)
            {
                if (element is T ancestor)
                {
                    return ancestor;
                }
                element = element.Parent as VisualElement;
            }
            return null;
        }
    }
}
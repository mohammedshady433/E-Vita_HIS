using E_Vita.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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


        private readonly LabTest _labtestvar;
        private List<LabTest> _labtestList;
        // this store the selected lab tests to use it with the lab app
        private List<LabTest> _selectedLabTests;


        private readonly RadiologyTests _radiologyTestsVar;
        private List<RadiologyTests> _radiologyTestsList;
        // this store the selected radiology tests to use it with the lab app
        private List<RadiologyTests> _selectedRadiologyTests;

        public Prescription()
        {
            InitializeComponent();
            _diseaseVar = new Disease();
            SuggestionsList.SelectionChanged += SuggestionsList_SelectionChanged;

            _drugvar = new Drug();
            _selectedDrugs = new List<Drug>();
            SuggestionsListofDrugs.SelectionChanged += SuggestionsList_SelectionChangedforDrugs;

            _labtestvar = new LabTest();
            _selectedLabTests = new List<LabTest>();
            SuggestionsListofLabTests.SelectionChanged += SuggestionsList_SelectionChangedofLabTests;

            _radiologyTestsVar = new RadiologyTests();
            _selectedRadiologyTests = new List<RadiologyTests>();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadDataAsync();
            await LoadDrugDataAsync();
            await loadlabdata();
            await loadradiologydata();
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
            if (entry == searchentrylabTests)
                return SuggestionsListofLabTests;

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

            // Check in LabTestContainer
            index = labteststackpanel.Children.IndexOf(entry);
            if (index >= 0 && index + 1 < labteststackpanel.Children.Count)
            {
                var nextChild = labteststackpanel.Children[index + 1];
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
            if (collectionView == SuggestionsListofLabTests)
                return searchentrylabTests;

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

            // Check in LabTestContainer
            index = labteststackpanel.Children.IndexOf(collectionView);
            if (index > 0 && labteststackpanel.Children[index - 1] is Entry labEntry)
            {
                return labEntry;
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
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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

        //labtests*******************************************************************************************************************************************
        private async Task loadlabdata()
        {
            _labtestList = await _labtestvar.LoadLabTests();
            if (_labtestList == null || !_labtestList.Any())
            {
                await DisplayAlert("Error", "Failed to load Lab test data. Please check the JSON file.", "OK");
            }
            else
            {
                SuggestionsListofLabTests.ItemsSource = _labtestList;
            }
        }
        private void addlabbtn(object sender, EventArgs e)
        {
            var newlabtestentry = new Entry
            {
                Placeholder = "enter the test name",
                WidthRequest = 300,
                Margin = new Thickness(10),
                BackgroundColor = Color.FromArgb("#FFCECE4"),
                TextColor = Colors.White
            };

            newlabtestentry.TextChanged += SearchEntry_TextChangedforlabs;

            var newCollectionView = new CollectionView
            {
                IsVisible = false,
                SelectionMode = SelectionMode.Single,
                WidthRequest = 300,
                HeightRequest = 200,
                Margin = new Thickness(10)
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
                label.SetBinding(Label.TextProperty, "TestName");
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += OnSuggestionTappedofLabTest;
                label.GestureRecognizers.Add(tapGestureRecognizer);
                return label;
            });
            labteststackpanel.Children.Add(newlabtestentry);
            labteststackpanel.Children.Add(newCollectionView);
        }

        private void SearchEntry_TextChangedforlabs(object sender, TextChangedEventArgs e)
        {
            if (sender is Entry entry)
            {
                var keyword = e.NewTextValue?.ToLowerInvariant() ?? "";
                var filtered = _labtestList
                    .Where(s => s.TestName != null && s.TestName.ToLowerInvariant().Contains(keyword))
                    .ToList();

                var collectionView = entry == searchentrylabTests ? SuggestionsListofLabTests : FindCollectionViewForEntry(entry);
                if (collectionView != null)
                {
                    collectionView.ItemsSource = filtered;
                    collectionView.IsVisible = filtered.Any();
                }
            }
        }

        private async void OnSuggestionTappedofLabTest(object sender, TappedEventArgs e)
        {
            if (sender is Label label && label.BindingContext is LabTest lbtest)
            {
                var collectionView = label.FindAncestor<CollectionView>();
                var entry = FindEntryForCollectionView(collectionView);
                if (entry != null)
                {
                    entry.Text = lbtest.TestName; // Update the specific entry that triggered the search
                    collectionView.IsVisible = false;
                    _selectedLabTests.Add(lbtest);
                }
            }
        }

        private void SuggestionsList_SelectionChangedofLabTests(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is LabTest testname)
            {
                // Find the correct entry field
                var collectionView = sender as CollectionView;
                var entry = FindEntryForCollectionView(collectionView);
                if (entry != null)
                {
                    entry.Text = testname.TestName;
                    collectionView.IsVisible = false;
                    _selectedLabTests.Add(testname);
                }
            }
        }



        //RadiologyTests *******************************************************************************************************************************************

        private async Task loadradiologydata()
        {
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync("Radiology_Tests_JSON.json");
                using var reader = new StreamReader(stream);
                string json = await reader.ReadToEndAsync();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var radiologyData = JsonSerializer.Deserialize<RadiologyTests>(json, options);

                if (radiologyData != null)
                {
                    _radiologyTestsList = new List<RadiologyTests> { radiologyData };
                }
                else
                {
                    await DisplayAlert("Error", "Failed to parse radiology test data.", "OK");
                    _radiologyTestsList = new List<RadiologyTests>();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load radiology test data: {ex.Message}", "OK");
                _radiologyTestsList = new List<RadiologyTests>();
            }
        }


        private void SearchEntry_TextChangedforRadio(object sender, TextChangedEventArgs e)
        {
            if (sender is Entry entry)
            {
                var keyword = e.NewTextValue?.ToLowerInvariant() ?? "";

                // Create a flattened list of all radiology tests with their categories
                if (_radiologyTestsList != null && _radiologyTestsList.Any())
                {
                    var flattenedTests = new List<(string Category, string TestName)>();

                    if (_radiologyTestsList[0].Radiography != null)
                        foreach (var test in _radiologyTestsList[0].Radiography)
                            flattenedTests.Add(("Radiography", test));

                    if (_radiologyTestsList[0].ComputedTomography != null)
                        foreach (var test in _radiologyTestsList[0].ComputedTomography)
                            flattenedTests.Add(("Computed Tomography", test));

                    if (_radiologyTestsList[0].MagneticResonanceImaging != null)
                        foreach (var test in _radiologyTestsList[0].MagneticResonanceImaging)
                            flattenedTests.Add(("Magnetic Resonance Imaging", test));

                    if (_radiologyTestsList[0].Ultrasound != null)
                        foreach (var test in _radiologyTestsList[0].Ultrasound)
                            flattenedTests.Add(("Ultrasound", test));

                    if (_radiologyTestsList[0].NuclearMedicine != null)
                        foreach (var test in _radiologyTestsList[0].NuclearMedicine)
                            flattenedTests.Add(("Nuclear Medicine", test));

                    if (_radiologyTestsList[0].Mammography != null)
                        foreach (var test in _radiologyTestsList[0].Mammography)
                            flattenedTests.Add(("Mammography", test));

                    if (_radiologyTestsList[0].Fluoroscopy != null)
                        foreach (var test in _radiologyTestsList[0].Fluoroscopy)
                            flattenedTests.Add(("Fluoroscopy", test));

                    if (_radiologyTestsList[0].InterventionalRadiology != null)
                        foreach (var test in _radiologyTestsList[0].InterventionalRadiology)
                            flattenedTests.Add(("Interventional Radiology", test));

                    if (_radiologyTestsList[0].BoneDensitometry != null)
                        foreach (var test in _radiologyTestsList[0].BoneDensitometry)
                            flattenedTests.Add(("Bone Densitometry", test));

                    // Filter based on keyword
                    var filtered = flattenedTests
                        .Where(t => t.TestName.ToLowerInvariant().Contains(keyword))
                        .ToList();

                    // Create RadiologyTestViewModel objects for display
                    var viewModels = filtered.Select(t => new RadiologyTestViewModel
                    {
                        Category = t.Category,
                        TestName = t.TestName
                    }).ToList();

                    // Update the collection view
                    var collectionView = entry == RadiologyTests ? SuggestionsListofRadio : FindCollectionViewForRadio(entry);
                    if (collectionView != null)
                    {
                        collectionView.ItemsSource = viewModels;
                        collectionView.IsVisible = viewModels.Any();
                    }
                }
            }
        }

        private CollectionView FindCollectionViewForRadio(Entry entry)
        {
            // For the main radiology search entry
            if (entry == RadiologyTests)
                return SuggestionsListofRadio;

            // Check in RadiologStackpanel
            var index = radiologyStackpanel.Children.IndexOf(entry);
            if (index >= 0 && index + 1 < radiologyStackpanel.Children.Count)
            {
                var nextChild = radiologyStackpanel.Children[index + 1];
                if (nextChild is CollectionView collectionView)
                {
                    return collectionView;
                }
            }

            return null;
        }

        private void addRaidologybtn(object sender, EventArgs e)
        {
            var newRadioEntry = new Entry
            {
                Placeholder = "Enter radiology test name",
                WidthRequest = 300,
                Margin = new Thickness(10),
                BackgroundColor = Color.FromArgb("#FFCECE4"),
                TextColor = Colors.White
            };

            newRadioEntry.TextChanged += SearchEntry_TextChangedforRadio;

            var newCollectionView = new CollectionView
            {
                IsVisible = false,
                SelectionMode = SelectionMode.Single,
                WidthRequest = 300,
                HeightRequest = 200,
                Margin = new Thickness(10)
            };

            newCollectionView.ItemTemplate = new DataTemplate(() =>
            {
                var stackLayout = new StackLayout { Orientation = StackOrientation.Vertical };

                var categoryLabel = new Label
                {
                    Padding = new Thickness(10, 5, 10, 0),
                    BackgroundColor = Color.FromArgb("#333"),
                    TextColor = Colors.White,
                    FontSize = 12,
                    FontAttributes = FontAttributes.Italic
                };
                categoryLabel.SetBinding(Label.TextProperty, "Category");

                var testLabel = new Label
                {
                    Padding = new Thickness(10, 0, 10, 5),
                    BackgroundColor = Color.FromArgb("#EEE"),
                    TextColor = Colors.Black,
                    FontAttributes = FontAttributes.Bold,
                    Margin = new Thickness(0, 0, 0, 5)
                };
                testLabel.SetBinding(Label.TextProperty, "TestName");

                stackLayout.Children.Add(categoryLabel);
                stackLayout.Children.Add(testLabel);

                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += OnSuggestionTappedofRadio;
                stackLayout.GestureRecognizers.Add(tapGestureRecognizer);

                return stackLayout;
            });

            // Create details frame that will be displayed after selection
            var detailsFrame = new Frame
            {
                IsVisible = false,
                Margin = new Thickness(10),
                Padding = new Thickness(10),
                BackgroundColor = Color.FromArgb("#333333"),
                BorderColor = Color.FromArgb("#666"),
                CornerRadius = 5
            };

            var detailsStack = new VerticalStackLayout();

            var categoryLabel = new Label
            {
                Text = "Category:",
                TextColor = Colors.White,
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(0, 0, 0, 5)
            };

            var categoryValue = new Label
            {
                TextColor = Colors.LightGray,
                Margin = new Thickness(10, 0, 0, 10)
            };

            var testNameLabel = new Label
            {
                Text = "Test Name:",
                TextColor = Colors.White,
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(0, 0, 0, 5)
            };

            var testNameValue = new Label
            {
                TextColor = Colors.LightGray,
                Margin = new Thickness(10, 0, 0, 10)
            };

            var notesLabel = new Label
            {
                Text = "Notes:",
                TextColor = Colors.White,
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(0, 0, 0, 5)
            };

            var notesEditor = new Editor
            {
                Placeholder = "Enter any notes or special instructions...",
                BackgroundColor = Color.FromArgb("#222"),
                TextColor = Colors.White,
                HeightRequest = 100,
                Margin = new Thickness(0, 0, 0, 10)
            };

            var urgencyLabel = new Label
            {
                Text = "Urgency:",
                TextColor = Colors.White,
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(0, 0, 0, 5)
            };

            var urgencyOptions = new HorizontalStackLayout
            {
                Margin = new Thickness(10, 0, 0, 10)
            };

            var normalRadio = new RadioButton { Content = "Normal", TextColor = Colors.White, GroupName = "urgency" + Guid.NewGuid().ToString() };
            var urgentRadio = new RadioButton { Content = "Urgent", TextColor = Colors.White, GroupName = normalRadio.GroupName };
            var statRadio = new RadioButton { Content = "STAT", TextColor = Colors.White, GroupName = normalRadio.GroupName };

            urgencyOptions.Children.Add(normalRadio);
            urgencyOptions.Children.Add(urgentRadio);
            urgencyOptions.Children.Add(statRadio);

            // Add all components to the details stack
            detailsStack.Children.Add(categoryLabel);
            detailsStack.Children.Add(categoryValue);
            detailsStack.Children.Add(testNameLabel);
            detailsStack.Children.Add(testNameValue);
            detailsStack.Children.Add(notesLabel);
            detailsStack.Children.Add(notesEditor);
            detailsStack.Children.Add(urgencyLabel);
            detailsStack.Children.Add(urgencyOptions);

            // Add the details stack to the frame
            detailsFrame.Content = detailsStack;

            // Add the components to the radiology stack panel
            radiologyStackpanel.Children.Add(newRadioEntry);
            radiologyStackpanel.Children.Add(newCollectionView);
            radiologyStackpanel.Children.Add(detailsFrame);
        }

        private void OnSuggestionTappedofRadio(object sender, TappedEventArgs e)
        {
            try
            {
                // First, ensure we're working with the right UI element type and data
                if (sender is StackLayout stackLayout && stackLayout.BindingContext is RadiologyTestViewModel radiologyTest)
                {
                    // Get the parent CollectionView
                    var collectionView = stackLayout.FindAncestor<CollectionView>();
                    if (collectionView == null)
                    {
                        Console.WriteLine("CollectionView not found");
                        return;
                    }

                    // Find the associated entry using our specialized method for radiology
                    var entry = FindEntryForCollectionViewRadio(collectionView);
                    if (entry == null)
                    {
                        Console.WriteLine("Entry not found for CollectionView");
                        return;
                    }

                    // Update entry and hide suggestions
                    entry.Text = radiologyTest.TestName;
                    collectionView.IsVisible = false;

                    // Find the parent container to locate the details frame
                    var parentStackPanel = entry.Parent as StackLayout;
                    if (parentStackPanel == null)
                    {
                        parentStackPanel = entry.Parent as StackLayout; 
                    }

                    if (parentStackPanel != null)
                    {
                        // Get the index of entry in parent container
                        int entryIndex = -1;
                        for (int i = 0; i < parentStackPanel.Children.Count; i++)
                        {
                            if (parentStackPanel.Children[i] == entry)
                            {
                                entryIndex = i;
                                break;
                            }
                        }

                        // Find the frame (which should follow the CollectionView)
                        if (entryIndex >= 0 && entryIndex + 2 < parentStackPanel.Children.Count)
                        {
                            var frame = parentStackPanel.Children[entryIndex + 2] as Frame;
                            if (frame != null)
                            {
                                // Update the frame content
                                if (frame.Content is VerticalStackLayout detailsStack)
                                {
                                    ((Label)detailsStack.Children[1]).Text = radiologyTest.Category;
                                    ((Label)detailsStack.Children[3]).Text = radiologyTest.TestName;
                                    frame.IsVisible = true;
                                }
                            }
                        }
                    }

                    // Add to selected radiology tests
                    var newTest = new RadiologyTests();
                    // You might want to add a custom method to your RadiologyTests class
                    // to store the selected test and category
                    _selectedRadiologyTests.Add(newTest);
                }
                else if (sender is Label label && label.BindingContext is RadiologyTestViewModel viewModel)
                {
                    // Handle the case when it's a label
                    var collectionView = label.FindAncestor<CollectionView>();
                    var entry = FindEntryForCollectionViewRadio(collectionView);
                    if (entry != null)
                    {
                        entry.Text = viewModel.TestName;
                        collectionView.IsVisible = false;

                        // Optionally handle details frame similar to above

                        _selectedRadiologyTests.Add(new RadiologyTests());
                    }
                }
            }
            catch (Exception ex)
            {
                // Add debugging info and handle the exception
                Console.WriteLine($"Error in OnSuggestionTappedofRadio: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        // Specialized method to find entry for radiology CollectionView
        private Entry FindEntryForCollectionViewRadio(CollectionView collectionView)
        {
            // For the main radiology collection view
            if (collectionView == SuggestionsListofRadio)
                return RadiologyTests;

            // For dynamically added collection views in radiologyStackpanel
            var parent = collectionView.Parent as VerticalStackLayout;
            if (parent != null && parent == radiologyStackpanel)
            {
                var index = radiologyStackpanel.Children.IndexOf(collectionView);
                if (index > 0 && radiologyStackpanel.Children[index - 1] is Entry entry)
                {
                    return entry;
                }
            }

            // Check if the CollectionView is inside another container
            foreach (var child in radiologyStackpanel.Children)
            {
                if (child is VerticalStackLayout stack)
                {
                    var index = stack.Children.IndexOf(collectionView);
                    if (index > 0 && stack.Children[index - 1] is Entry entry)
                    {
                        return entry;
                    }
                }
            }

            return null;
        }


        // Helper class for displaying radiology tests in the UI
        public class RadiologyTestViewModel
        {
            public string Category { get; set; }
            public string TestName { get; set; }
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
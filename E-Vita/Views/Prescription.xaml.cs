using E_Vita.Services;
using E_Vita.ViewModels;
using E_Vita_APIs.Models;
using Syncfusion.Maui.Graphics.Internals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static E_Vita.Views.Prescription;

namespace E_Vita.Views
{
    public partial class Prescription : ContentPage
    {
        public ObservableCollection<Appointment> AppointmentsObservablecollection { get; set; } = new ObservableCollection<Appointment>();
        private readonly Disease _diseaseVar;
        int _patientid;
        int _currentDoctorId;
        private readonly MedicationServices medicationServices = new MedicationServices();
        private readonly PatientServices _patientService = new PatientServices();
        private readonly PrescriptionService _prescriptionService = new PrescriptionService();
        private List<Disease.NodeWithMeta> _nodesWithMeta;

        private readonly Drug _drugvar;
        private List<Drug> _druglist;
        //this store the selected drugs to use it with the pharma app
        private List<Drug> _selectedDrugs;
        //this will be used for the medication model to send the data to the DB
        private List<Medication> _medicationsWillGotoDB = new List<Medication>();

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


            if (Preferences.ContainsKey("SelectedPatientId"))
            {
                _patientid = Preferences.Get("SelectedPatientId", 0);
            }
            // Get doctor ID from preferences (set during login in MainPage.xaml.cs)
            if (Preferences.ContainsKey("CurrentDoctorId"))
            {
                _currentDoctorId = Preferences.Get("CurrentDoctorId", 0);
            }

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
            getpatientbyIDandfetchthedata();
            showprevPrescriptions();
        }

        private List<string> GetAllDosages()
        {
            var dosages = new List<string>();

            // 1. Add the main dosage field from the XAML (if filled)
            var mainDosage = DoseEntry.Text?.Trim();
            if (!string.IsNullOrEmpty(mainDosage))
            {
                var firstPart = mainDosage.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0];
                dosages.Add(firstPart);
            }

            // 2. Add all dynamically generated dosage fields
            foreach (var child in DrugNotesContainer.Children)
            {
                if (child is VerticalStackLayout verticalLayout && verticalLayout.Children.Count > 0)
                {
                    if (verticalLayout.Children[0] is HorizontalStackLayout horizontalStack && horizontalStack.Children.Count >= 2)
                    {
                        var dosageEntry = horizontalStack.Children[1] as Entry;
                        if (dosageEntry != null)
                        {
                            var dosageText = dosageEntry.Text?.Trim();
                            if (!string.IsNullOrEmpty(dosageText))
                            {
                                var firstPart = dosageText.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0];
                                dosages.Add(firstPart);
                            }
                        }
                    }
                }
            }

            return dosages;
        }

        private List<string> GetAllLabTestEntries()
        {
            var labTestEntries = new List<string>();

            // Add the main entry if filled
            var mainLabTest = searchentrylabTests.Text?.Trim();
            if (!string.IsNullOrEmpty(mainLabTest))
                labTestEntries.Add(mainLabTest);

            // Add dynamically added entries
            foreach (var child in labteststackpanel.Children)
            {
                if (child is Entry entry)
                {
                    var text = entry.Text?.Trim();
                    if (!string.IsNullOrEmpty(text))
                        labTestEntries.Add(text);
                }
            }
            return labTestEntries;
        }
        private List<string> GetAllRadiologyEntries()
        {
            var radiologyEntries = new List<string>();

            // Add the main entry if filled
            var mainRadiology = RadiologyTests.Text?.Trim();
            if (!string.IsNullOrEmpty(mainRadiology))
                radiologyEntries.Add(mainRadiology);

            // Add dynamically added entries
            foreach (var child in radiologyStackpanel.Children)
            {
                if (child is Entry entry)
                {
                    var text = entry.Text?.Trim();
                    if (!string.IsNullOrEmpty(text))
                        radiologyEntries.Add(text);
                }
            }
            return radiologyEntries;
        }

        private List<string> GetAllDiseaseEntries()
        {
            var diseaseEntries = new List<string>();

            // Add the main entry if filled
            var mainDisease = SearchEntry.Text?.Trim();
            if (!string.IsNullOrEmpty(mainDisease))
                diseaseEntries.Add(mainDisease);

            // Add dynamically added entries
            foreach (var child in DiseaseNotesContainer.Children)
            {
                if (child is Entry entry)
                {
                    var text = entry.Text?.Trim();
                    if (!string.IsNullOrEmpty(text))
                        diseaseEntries.Add(text);
                }
            }
            return diseaseEntries;
        }

        private List<(string DrugName, string Dosage)> GetAllDrugEntries()
        {
            var drugEntries = new List<(string DrugName, string Dosage)>();

            foreach (var child in DrugNotesContainer.Children)
            {
                if (child is VerticalStackLayout verticalLayout && verticalLayout.Children.Count > 0)
                {
                    // The first child is a HorizontalStackLayout with the entries
                    if (verticalLayout.Children[0] is HorizontalStackLayout horizontalStack && horizontalStack.Children.Count >= 2)
                    {
                        var drugEntry = horizontalStack.Children[0] as Entry;
                        var dosageEntry = horizontalStack.Children[1] as Entry;

                        if (drugEntry != null && dosageEntry != null)
                        {
                            var drugName = drugEntry.Text?.Trim();
                            var dosage = dosageEntry.Text?.Trim();

                            if (!string.IsNullOrEmpty(drugName) || !string.IsNullOrEmpty(dosage))
                            {
                                drugEntries.Add((drugName, dosage));
                            }
                        }
                    }
                }
            }

            return drugEntries;
        }

        private async void getpatientbyIDandfetchthedata()
        {
            //search for the patient by the id using the services of it then return it on patient var
            var patient = await _patientService.GetPatientsAsync(_patientid);
            NameLabel.Text = patient.Name;
            NationalityLabel.Text = patient.Nationality;
            var today = DateTime.Now;
            var age = today.Year - patient.DateOfBirth.Year;
            Ageentry.Text = age.ToString();
            IdLabel.Text = patient.ID.ToString();
            if (patient.Gender == Gender.Male)
            {
                FemaleRadioButton.IsChecked = true;

            }
            else
            {
                MaleRadioButton.IsChecked = true;
            }

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
            // Drug search Entry
            var newDrugEntry = new Entry
            {
                Placeholder = "Search drugs by name...",
                WidthRequest = 200,
                BackgroundColor = Color.FromArgb("#FFECECE4"),
                TextColor = Colors.Black
            };

            // Suggestions CollectionView for this entry
            var newCollectionView = new CollectionView
            {
                IsVisible = false,
                SelectionMode = SelectionMode.Single,
                WidthRequest = 200,
                HeightRequest = 200,
                Margin = new Thickness(0, 0, 0, 5)
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
                tapGestureRecognizer.Tapped += (s, e) =>
                {
                    if (label.BindingContext is Drug drug)
                    {
                        newDrugEntry.Text = drug.Tradename;
                        newCollectionView.IsVisible = false;
                        _selectedDrugs.Add(drug);
                        DisplayAlert("Remember", $"Description: {drug.Pharmacology}\nPrice: {drug.new_price}", "I know");

                    }
                };
                label.GestureRecognizers.Add(tapGestureRecognizer);
                return label;
            });

            newDrugEntry.TextChanged += (s, e) =>
            {
                var keyword = e.NewTextValue?.ToLowerInvariant() ?? "";
                var filtered = _druglist
                    .Where(d => d.Tradename != null && d.Tradename.ToLowerInvariant().Contains(keyword))
                    .ToList();
                newCollectionView.ItemsSource = filtered;
                newCollectionView.IsVisible = filtered.Any();
            };

            // Medication info Entry (e.g., dosage)
            var medicationInfoEntry = new Entry
            {
                Placeholder = "Dosage, e.g. 500mg 2x/day",
                WidthRequest = 180,
                BackgroundColor = Color.FromArgb("#FFECECE4"),
                TextColor = Colors.Black
            };

            // Layout: DrugEntry (top), Suggestions (under it), DoseEntry (under both)
            var verticalLayout = new VerticalStackLayout
            {
                Spacing = 0,
                Margin = new Thickness(0, 0, 0, 0)
            };
            var HorizontalStack = new HorizontalStackLayout
            {
                Spacing = 0,
                Margin = new Thickness(100, 0, 0, 0)
            };
            HorizontalStack.Children.Add(newDrugEntry);
            HorizontalStack.Children.Add(medicationInfoEntry);
            verticalLayout.Children.Add(HorizontalStack);
            verticalLayout.Children.Add(newCollectionView);


            DrugNotesContainer.Children.Add(verticalLayout);
        }

        private void GettheDrugstothemedicationlist()
        {
            var Dosages = GetAllDosages();
             foreach(var Dosage in Dosages)
            {
                var drug = _selectedDrugs.FirstOrDefault();
                Medication medication = new Medication();
                medication.MedID = drug.Id;
                medication.ActiveIngrediant = drug.ActiveIngredient;
                medication.Dose = Dosage;
                medication.Medication_name = drug.Tradename;
                medication.Time = TimeOnly.FromTimeSpan(DateTime.Now.TimeOfDay);
                medication.PatientId = _patientid;
                medication.PractitionerID = _currentDoctorId;
                _medicationsWillGotoDB.Add(medication);
            }
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

        private void ShowReserveCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            checkboxentry3.IsVisible = e.Value;
        }

        private async void saveData(object sender, EventArgs e)
        {

            await DisplayAlert("Wait", "Prescription will take time!", "OK");

            GettheDrugstothemedicationlist();
            
            // Save the selected drugs, lab tests, and radiology tests to the database
            foreach (var medication in _medicationsWillGotoDB)
            {
                //send the medication data to the E-Vita API the next line is the line is related to my app the rest for loop is related to the pharmacy application
                await medicationServices.AddMedication(medication);
                
                //send the patient data to the pharmacy API
                //PharmaApp.services.PatientService patientService = new PharmaApp.services.PatientService();
                //var patient = await patientService.GetByIdAsync(_patientid);
                //if (patient == null)
                //{
                //    PatientServices patientserv = new PatientServices();
                //    var patTOsend = await patientserv.GetPatientsAsync(_patientid);
                //    Pharmacy_ASP_API.Models.Entities.Patient patient1 = new Pharmacy_ASP_API.Models.Entities.Patient
                //    {
                //        PatientId = patTOsend.ID.ToString(),
                //        PatientName = patTOsend.Name,
                //        PhoneNo = patTOsend.Phone,
                //        Address = patTOsend.Address,
                //        DateOfBirth = patTOsend.DateOfBirth,
                //        Gender = (Pharmacy_ASP_API.Models.Entities.Gender)patTOsend.Gender
                //    };
                //    await patientService.AddAsync(patient1);
                //}

                //send the medication data (( medication Request )) to the pharmacy API
                PractitionerServices practitionerServices = new PractitionerServices();
                var practitioner = await practitionerServices.GetPractitionerByIdAsync(_currentDoctorId);

                PharmaApp.Services.MedicationRequestService medicationServicesPharm = new PharmaApp.Services.MedicationRequestService();
                int uniqueId = (int)(DateTime.Now.Ticks & 0x7FFFFFFF);

                Pharmacy_ASP_API.Models.Entities.MedicationRequest medthatWillGoToPharmacy = new Pharmacy_ASP_API.Models.Entities.MedicationRequest
                {
                   authoredTime = DateTime.Now.ToString(),
                   DoseInstruction = medication.Dose,
                   MedicationId = medication.MedID,
                   DrOutBed =practitioner.Name,
                   DrInBed = "null",
                   Status = "active",
                   RequestId = uniqueId.ToString()
                };
                await medicationServicesPharm.CreateMedicationRequestAsync(medthatWillGoToPharmacy);
            }

            E_Vita_APIs.Models.Prescription prescription = new E_Vita_APIs.Models.Prescription();
            prescription.ReasonForVisit = ReasonForVisitEntry.Text;
            prescription.Surgery = checkboxentry.IsVisible ? checkboxentry.Text : null;
            prescription.Reserve = checkboxentry3.IsVisible ? true : false;
            prescription.familyHistory = checkboxentry2.IsVisible ? checkboxentry2.Text : null;
            List<string> diseases = GetAllDiseaseEntries();
            string diseasesString = string.Join(", ", diseases);
            prescription.Diseases = diseasesString;
            List<string> labTests = GetAllLabTestEntries();
            string labTestsString = string.Join(", ", labTests);
            prescription.LabTest = labTestsString;
            List<string> radiologyTests = GetAllRadiologyEntries();
            string radiologyTestsString = string.Join(", ", radiologyTests);
            prescription.RadiologyTest = radiologyTestsString;
            prescription.PatientId = _patientid;
            prescription.PractitionerID = _currentDoctorId;
            prescription.Examination = Examination.Text;
            prescription.patientcomplaint = complaint.Text;

            await _prescriptionService.AddPrescriptionAsync(prescription);           
            await DisplayAlert("Success", "Prescription saved successfully!", "OK");
        }

        private async void showprevPrescriptions()
        {
            // Await the task to get the list of prescriptions
            var _allPrescriptions = await _prescriptionService.GetPrescriptionsAsync();

            // Search for the prescriptions that have our patient ID
            var _patientPrescriptions = _allPrescriptions.Where(p => p.PatientId == _patientid).ToList();
            var prescriptions = new List<PrescriptionBindingContextClass>();

            foreach (var prescription in _patientPrescriptions)
            {
                // Create a new PrescriptionBindingContextClass object
                var bindingContext = new PrescriptionBindingContextClass
                {
                    ReasonForVisit = prescription.ReasonForVisit,
                    Diseases = prescription.Diseases,
                    LabTest = prescription.LabTest,
                    RadiologyTest = prescription.RadiologyTest,
                    Surgery = prescription.Surgery ?? string.Empty, // Handle null
                    familyHistory = prescription.familyHistory ?? string.Empty, // Handle null
                    practitionerID = prescription.PractitionerID.ToString()
                };

                // Add the binding context to the list
                prescriptions.Add(bindingContext);
            }

            // Set the ItemsSource of the DataGrid
            ScheduleDataGrid.ItemsSource = prescriptions;
        }

        internal class PrescriptionBindingContextClass
        {
            public string ReasonForVisit { set; get; }
            public string Diseases { set; get; }
            public string LabTest { set; get; }
            public string RadiologyTest { set; get; }
            public string Surgery { set; get; }
            public string familyHistory { set; get; }
            public string practitionerID { set; get; }
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
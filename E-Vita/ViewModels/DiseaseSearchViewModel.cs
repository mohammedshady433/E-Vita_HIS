using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Vita.ViewModels
{
    public class DiseaseSearchViewModel : INotifyPropertyChanged
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string SNOMED_API_URL = "https://browser.ihtsdotools.org/snowstorm/snomed-ct/v2/concepts?term=";

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                    SearchDiseasesAsync();
                }
            }
        }

        public ObservableCollection<string> DiseaseSuggestions { get; } = new ObservableCollection<string>();

        private async void SearchDiseasesAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchText) || SearchText.Length < 3)
                return;

            try
            {
                string requestUrl = $"{SNOMED_API_URL}{SearchText}&active=true&limit=10";
                string response = await _httpClient.GetStringAsync(requestUrl);

                var result = JsonSerializer.Deserialize<SnomedResponse>(response);
                DiseaseSuggestions.Clear();

                foreach (var item in result.items)
                {
                    DiseaseSuggestions.Add($"{item.term} ({item.conceptId})");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching diseases: {ex.Message}");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        // لتحليل JSON
        public class SnomedResponse
        {
            public List<SnomedItem> items { get; set; }
        }

        public class SnomedItem
        {
            public string term { get; set; }
            public string conceptId { get; set; }
        }
    }
}
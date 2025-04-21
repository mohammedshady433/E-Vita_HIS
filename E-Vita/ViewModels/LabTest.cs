using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Vita.ViewModels
{
    public class LabTest
    {
        [JsonPropertyName("test_name")]
        public string TestName { get; set; }

        [JsonPropertyName("tested_components")]
        public List<string> TestedComponents { get; set; }

        [JsonPropertyName("normal_range")]
        public object NormalRange { get; set; } // Use object to handle both dictionary and string

        public async Task<List<LabTest>> LoadLabTests()
        {
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync("labtests.json");
                using var reader = new StreamReader(stream);
                string json = await reader.ReadToEndAsync();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var labTests = JsonSerializer.Deserialize<List<LabTest>>(json, options);
                return labTests ?? new List<LabTest>();
            }
            catch (Exception ex)
            {
                // Log the error for debugging
                Console.WriteLine($"Error loading lab tests: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw; // Re-throw to see the error in the calling method
            }
        }

        // Helper method to get normal range as string or dictionary
        public string GetNormalRangeAsString()
        {
            if (NormalRange is string str)
                return str;
            if (NormalRange is Dictionary<string, string> dict)
                return string.Join("; ", dict.Select(kv => $"{kv.Key}: {kv.Value}"));
            return string.Empty;
        }
    }
}
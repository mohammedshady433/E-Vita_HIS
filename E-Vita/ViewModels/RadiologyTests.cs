using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Vita.ViewModels
{
    public class RadiologyTests
    {
        public List<string> Radiography { get; set; }
        public List<string> ComputedTomography { get; set; }
        public List<string> MagneticResonanceImaging { get; set; }
        public List<string> Ultrasound { get; set; }
        public List<string> NuclearMedicine { get; set; }
        public List<string> Mammography { get; set; }
        public List<string> Fluoroscopy { get; set; }
        public List<string> InterventionalRadiology { get; set; }
        public List<string> BoneDensitometry { get; set; }

       public class RadiologyWrapper
        {
        public List<RadiologyTests> Radioo { get; set; }
        }
        
        public async Task<List<RadiologyTests>> LoadRadiologyTests()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("Radiology_Tests_JSON.json");
            using var reader = new StreamReader(stream);
            string json = await reader.ReadToEndAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var wrapper = System.Text.Json.JsonSerializer.Deserialize<RadiologyWrapper>(json, options);
            return wrapper?.Radioo ?? new List<RadiologyTests>(); // Ensures a non-null list is returned
        }
    }


}

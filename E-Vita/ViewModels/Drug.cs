using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Vita.ViewModels
{
    public class Drug
    {
        public string ActiveIngredient { get; set; }
        public string Company { get; set; }
        public string Created { get; set; }
        public string Form { get; set; }
        public string Group { get; set; }
        public string Id { get; set; }
        public string new_price { get; set; }
        public string Pharmacology { get; set; }
        public string Route { get; set; }
        public string Tradename { get; set; }
        public string Updated { get; set; }



        public class DrugWrapper
        {
            public List<Drug> Drugs { get; set; }
        }

        public async Task<List<Drug>> LoadDrugs()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("medications_New_prices_up_to_03-08-2024.json");
            using var reader = new StreamReader(stream);
            string json = await reader.ReadToEndAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var wrapper = System.Text.Json.JsonSerializer.Deserialize<DrugWrapper>(json, options);
            return wrapper?.Drugs ?? new List<Drug>(); // Ensures a non-null list is returned
        }


        // Method to retrieve a drug by tradename
        //public Drug GetDrugByTradename(string tradename)
        //{
        //    if (Data == null || Data.Drugs == null)
        //    {
        //        throw new InvalidOperationException("Drug data is not loaded. Call LoadDrugs() first.");
        //    }

        //    // Search for the drug by tradename
        //    return Data.Drugs.FirstOrDefault(d => d.Tradename.Equals(tradename, StringComparison.OrdinalIgnoreCase));
        //}
    }



}

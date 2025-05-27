using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pharmacy_ASP_API.Models.Entities;


namespace PharmaApp.services
{
    public class ReportService : BaseApiService
    {
        private const string Endpoint = "Report";

        public Task<List<Report>> GetAllAsync()
        {
            return GetAsync<List<Report>>(Endpoint);
        }

        public Task<Report> GetByIdAsync(int id)
        {
            return GetAsync<Report>($"{Endpoint}/{id}");
        }

        public Task<bool> AddAsync(Report report)
        {
            return PostAsync(Endpoint, report);
        }

        public Task<bool> UpdateAsync(int id, Report report)
        {
            return PutAsync($"{Endpoint}/{id}", report);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return DeleteAsync($"{Endpoint}/{id}");
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Vita_APIs.Models;
namespace E_Vita.Services
{
    internal class ScreentimeArticleServices : BaseApiService
    {
        public ScreentimeArticleServices() : base() { }
        private const string Endpoint = "ScreentimeArticle";
        // Get all screentime articles
        public async Task<List<ScreentimeArticle>> GetAllAsync()
        {
            return await GetAsync<List<ScreentimeArticle>>(Endpoint);
        }
        // Get screentime article by ID
        public async Task<ScreentimeArticle> GetByIdAsync(int id)
        {
            return await GetAsync<ScreentimeArticle>($"{Endpoint}/{id}");
        }
        // Create new screentime article
        public async Task<bool> CreateAsync(ScreentimeArticle screentimeArticle)
        {
            return await PostAsync(Endpoint, screentimeArticle);
        }
        // Update existing screentime article
        public async Task<bool> UpdateAsync(int id, ScreentimeArticle screentimeArticle)
        {
            return await PostAsync($"{Endpoint}/{id}", screentimeArticle);
        }
        // Delete screentime article
        public async Task<bool> DeleteAsync(int id)
        {
            return await PostAsync($"{Endpoint}/Delete/{id}", new { Id = id });
        }
    }
}

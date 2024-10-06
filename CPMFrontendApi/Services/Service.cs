using SharedModels;
using System.Text;
using System.Text.Json;

namespace CPMFrontendApi.Services
{
    public class Service : IService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public Service(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetFinanceProcessesDataAsync()
        {
            var httpClient = _httpClientFactory.CreateClient();

            var reporting = httpClient.GetStringAsync("https://localhost:7147/api/ReportingApi");
            var budgeting = httpClient.GetStringAsync("https://localhost:7290/api/BudgetApi");

            await Task.WhenAll(reporting, budgeting);

            var response = new
            {
                ReportingResponse = await reporting,
                BudgetingResponse = await budgeting
            };

            return JsonSerializer.Serialize(response);
        }

        public async Task<string> PostFinanceProcessesDataAsync(CombinedModel model)
        {
            var reporting = await CallReporting(model.ReportData);
            var budgeting = await CallBudgeting(model.BudgetData);

            var response = new
            {
                ReportingResponse = reporting,
                BudgetingResponse = budgeting
            };

            return JsonSerializer.Serialize(response);
        }

        public async Task<string> CallReporting(string reportData)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var reportingContent = new StringContent(JsonSerializer.Serialize(new Reporting { ReportData = reportData }), Encoding.UTF8, "application/json");
           
            var reporting = await httpClient.PostAsync("https://localhost:7147/api/ReportingApi", reportingContent);

            if (reporting.IsSuccessStatusCode)
            {
                return await reporting.Content.ReadAsStringAsync();
            }

            throw new HttpRequestException($"Failed to call Reporting: {reporting.StatusCode}");
        }

        public async Task<string> CallBudgeting(string budget)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var budgetingContent = new StringContent(JsonSerializer.Serialize(new Budget { BudgetData = budget }), Encoding.UTF8, "application/json");

            var budgeting = await httpClient.PostAsync("https://localhost:7290/api/BudgetApi", budgetingContent);

            if (budgeting.IsSuccessStatusCode)
            {
                return await budgeting.Content.ReadAsStringAsync();
            }

            throw new HttpRequestException($"Failed to call Budgeting: {budgeting.StatusCode}");
        }
    }

    public class CombinedModel
    {
        public string ReportData { get; set; }
        public string BudgetData { get; set; }
    }
}

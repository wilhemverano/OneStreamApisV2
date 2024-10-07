using SharedModels;
using System.Text;
using System.Text.Json;

namespace CPMFrontendApi.Services
{
    public class Service : IService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<Service> _logger;

        public Service(IHttpClientFactory httpClientFactory, ILogger<Service> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<string> GetFinanceProcessesDataAsync()
        {
            try
            {
                _logger.LogInformation("Entering GetFinanceProcessesDataAsync");
                

                _logger.LogInformation("Calling reporting api");
                var reporting = GetReportingData();

                _logger.LogInformation("Calling budgeting api");
                var budgeting = GetBudgetingData();

                await Task.WhenAll(reporting, budgeting);

                var response = new
                {
                    ReportingResponse = await reporting,
                    BudgetingResponse = await budgeting
                };

                return JsonSerializer.Serialize(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on GetFinanceProcessesDataAsync");
                throw;
            }            
        }

        public async Task<string> GetReportingData()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var reporting = await httpClient.GetStringAsync("https://localhost:7147/api/ReportingApi");
                return reporting;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred when getting reporting data");
            }
            return null;
        }

        public async Task<string> GetBudgetingData()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var budgeting = await httpClient.GetStringAsync("https://localhost:7290/api/BudgetApi");
                return budgeting;
            }            
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred when getting budget data");
            }
            return null;
        }

        public async Task<string> PostFinanceProcessesDataAsync(CombinedModel model)
        {
            try
            {
                _logger.LogInformation($"Entering PostFinanceProcessesDataAsync with repor data: {model.ReportData} and budget data: {model.BudgetData}");
                var reporting = await CallReporting(model.ReportData);
                var budgeting = await CallBudgeting(model.BudgetData);

                var response = new
                {
                    ReportingResponse = reporting,
                    BudgetingResponse = budgeting
                };

                return JsonSerializer.Serialize(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on PostFinanceProcessesDataAsync");
                throw;
            }            
        }

        public async Task<string> CallReporting(string reportData)
        {
            try
            {
                _logger.LogInformation($"Entering CallReporting with data: {reportData}");
                var httpClient = _httpClientFactory.CreateClient();

                var reportingContent = new StringContent(JsonSerializer.Serialize(new Reporting { ReportData = reportData }), Encoding.UTF8, "application/json");

                var reporting = await httpClient.PostAsync("https://localhost:7147/api/ReportingApi", reportingContent);

                if (reporting.IsSuccessStatusCode)
                {
                    return await reporting.Content.ReadAsStringAsync();
                }

                throw new HttpRequestException($"Failed to call Reporting: {reporting.StatusCode} with data: {reportData}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on CallReporting");
            }
            return null;
        }

        public async Task<string> CallBudgeting(string budget)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when calling budgeting api");
            }
            return null;
            
        }
    }

    public class CombinedModel
    {
        public string ReportData { get; set; }
        public string BudgetData { get; set; }
    }
}

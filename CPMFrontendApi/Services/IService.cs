namespace CPMFrontendApi.Services
{
    public interface IService
    {
        Task<string> GetFinanceProcessesDataAsync();
        Task<string> PostFinanceProcessesDataAsync(CombinedModel value);
    }
}
using MachineFailures.Domain;
using static MachineFailures.Blazor.Components.Pages.Search;

namespace MachineFailures.Blazor.Components.Services;

public class ApiService
{
    private readonly HttpClient _http;

    public ApiService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("api");
    }

    public async Task<List<Machine>> GetMachines()
    {
        return await _http.GetFromJsonAsync<List<Machine>>("api/machines");
    }

    public async Task AddMachine(Machine machine)
    {
        var response = await _http.PostAsJsonAsync("api/machines", machine);
        var text = await response.Content.ReadAsStringAsync();
        Console.WriteLine(text);
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<Failure>> GetFailures()
    {
        return await _http.GetFromJsonAsync<List<Failure>>("api/failures");
    }

    public async Task AddFailure(Failure failure)
    {
        var response = await _http.PostAsJsonAsync("api/failures", failure);
        var text = await response.Content.ReadAsStringAsync();
        Console.WriteLine(text);
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<Category>> GetCategories()
    {
        return await _http.GetFromJsonAsync<List<Category>>("api/categories");
    }

    public async Task<List<Failure>> GetFailuresForMachine(int machineId)
    {
        return await _http.GetFromJsonAsync<List<Failure>>($"api/machines/{machineId}/failures");
    }

    public async Task DeleteFailure(int failureId)
    {
        await _http.DeleteAsync($"api/failures/{failureId}");
    }

    public async Task UpdateFailure(Failure failure)
    {
        await _http.PutAsJsonAsync($"api/failures/{failure.Id}", failure);
    }

    public async Task<List<SearchResult>> SearchMachines(string? name, DateTime? dateFrom, DateTime? dateTo, int categoryId)
    {
        var queryParts = new List<string>();

        if (!string.IsNullOrWhiteSpace(name))
            queryParts.Add($"name={Uri.EscapeDataString(name)}");

        if (categoryId > 0)
            queryParts.Add($"categoryId={categoryId}");

        if (dateFrom.HasValue)
            queryParts.Add($"from={Uri.EscapeDataString(dateFrom.Value.ToString("o"))}");

        if (dateTo.HasValue)
            queryParts.Add($"to={Uri.EscapeDataString(dateTo.Value.ToString("o"))}");

        var url = "api/search" + (queryParts.Count > 0 ? "?" + string.Join("&", queryParts) : string.Empty);

        var results = await _http.GetFromJsonAsync<List<SearchResult>>(url);
        return results ?? new List<SearchResult>();
    }

}
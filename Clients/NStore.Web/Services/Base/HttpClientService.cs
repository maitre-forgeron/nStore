using Newtonsoft.Json;
using System.Text;

namespace NStore.Web.Services.Base;

public class HttpClientService
{
    public HttpClient _http;

    public HttpClientService(HttpClient http)
    {
        _http = http;
    }

    public async Task<T> GetAsync<T>(string url)
    {
        var response = await _http.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<T>(content);

        return result;
    }

    public async Task<(T Result, H HeaderData)> GetAsync<T, H>(string url, string headerName)
    {
        var response = await _http.GetAsync(url);
        response.EnsureSuccessStatusCode();

        string headerData = "";

        if (response.Headers.TryGetValues(headerName, out IEnumerable<string> headerValues))
        {
            headerData = headerValues.FirstOrDefault();
        }

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<T>(content);

        H? headerAsObject = default;

        if (!string.IsNullOrWhiteSpace(headerData))
        {
            headerAsObject = JsonConvert.DeserializeObject<H>(headerData);
        }

        return (result, headerAsObject);
    }

    public async Task<TResponse> PostAsync<TResponse>(string url, object data)
    {
        var json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _http.PostAsync(url, content);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<TResponse>(responseContent);

        return result;
    }

    public async Task<TResponse> PutAsync<TResponse>(string url, object data)
    {
        var json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _http.PutAsync(url, content);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<TResponse>(responseContent);

        return result;
    }

    public new async Task<bool> DeleteAsync(string url)
    {
        var response = await _http.DeleteAsync(url);
        response.EnsureSuccessStatusCode();

        return true;
    }
}

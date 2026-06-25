using System.Text.Json;

namespace StockMarketApi.Services.Serializers;

public class ResponseSerializer
{
    /// <summary>
    /// Maps Massive API response body to TickerInfo DTO
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    public static async Task<Dictionary<string, object>?> SerializeToDict(HttpResponseMessage response)
    {
        string responseString = await response.Content.ReadAsStringAsync();
        Dictionary<string, object>? responseDict = 
            JsonSerializer.Deserialize<Dictionary<string, object>>(responseString);

        if (responseDict == null)
        {
            throw new Exception("Upstream Response call to Massive API did not contain a response");
        }
        
        return responseDict;
    }
}
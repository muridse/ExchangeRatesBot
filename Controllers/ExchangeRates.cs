using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace ExchangeRates
{
    public static class ExchangeRates
    {
        static private Currencies? _currencies { get; set; }
        static ExchangeRates() 
        {
            
        }
        static public async Task UpdateRates()
        {
            var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "https://www.cbr-xml-daily.ru/daily_json.js");

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode(); // Throw an exception if error

            var jsonString = await response.Content.ReadAsStringAsync();

            _currencies = JsonSerializer.Deserialize<Currencies>(jsonString);

            Console.WriteLine("Обновление курсов валют выполнено!");
        }
        static public async Task AutoUpdateRates(int refreshRateInHours) 
        {
            while (true) 
            {
                await UpdateRates();
                await Task.Delay(TimeSpan.FromHours(refreshRateInHours));
                
            }
        }
        static public Currencies GetRates() => _currencies;
        
    }

}

    
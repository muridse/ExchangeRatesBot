using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace ExchangeRates
{
    public static class CryptoRates
    {
        static private BTC? btc { get; set; }
        static private double btcPrice { get; set; }
        static private ETH? eth { get; set; }
        static private double ethPrice { get; set; }
        static private USDT? usdt { get; set; }
        static private double usdtPrice { get; set; }
        static CryptoRates()
        {

        }
        static public async Task UpdateRates()
        {
            var client = new HttpClient();
            await UpdateBTC(client);
            await UpdateETH(client);
            await UpdateUSDT(client);

            Console.WriteLine("Обновление курсов крипты выполнено!");
        }
        static private async Task UpdateBTC(HttpClient client) 
        {
            var requestBTC = new HttpRequestMessage(HttpMethod.Get, "https://api.exmo.com/v1/trades/?pair=BTC_USDT");
            var responseBTC = await client.SendAsync(requestBTC);
            responseBTC.EnsureSuccessStatusCode(); // Throw an exception if error
            var jsonStringBTC = await responseBTC.Content.ReadAsStringAsync();
            btc = JsonSerializer.Deserialize<BTC>(jsonStringBTC);
            btcPrice = CalculateAvgPrice(btc);

        }
        static private async Task UpdateETH(HttpClient client)
        {

            var requestETH = new HttpRequestMessage(HttpMethod.Get, "https://api.exmo.com/v1/trades/?pair=ETH_USDT");
            var responseETH = await client.SendAsync(requestETH);
            responseETH.EnsureSuccessStatusCode(); // Throw an exception if error
            var jsonStringETH = await responseETH.Content.ReadAsStringAsync();
            eth = JsonSerializer.Deserialize<ETH>(jsonStringETH);
            ethPrice = CalculateAvgPrice(eth);
        }
        static private async Task UpdateUSDT(HttpClient client)
        {
            var requestUSDT = new HttpRequestMessage(HttpMethod.Get, "https://api.exmo.com/v1/trades/?pair=USDT_USD");
            var responseUSDT = await client.SendAsync(requestUSDT);
            responseUSDT.EnsureSuccessStatusCode(); // Throw an exception if error
            var jsonStringUSDT = await responseUSDT.Content.ReadAsStringAsync();
            usdt = JsonSerializer.Deserialize<USDT>(jsonStringUSDT);
            usdtPrice = CalculateAvgPrice(usdt);
        }
        static private double CalculateAvgPrice(dynamic cryptovalute) 
        {


            double sumPrice = 0;
            if (cryptovalute is BTC)
            {
                foreach (var item in cryptovalute.BTC_USDT)
                {
                    sumPrice += double.Parse(item.price, CultureInfo.InvariantCulture);
                }
                return sumPrice / cryptovalute.BTC_USDT.Length;
            }
            if (cryptovalute is ETH)
            {
                foreach (var item in cryptovalute.ETH_USDT)
                {
                    sumPrice += double.Parse(item.price, CultureInfo.InvariantCulture);
                }
                return sumPrice / cryptovalute.ETH_USDT.Length;
            }
            if (cryptovalute is USDT)
            {
                foreach (var item in cryptovalute.USDT_USD)
                {
                    sumPrice += double.Parse(item.price, CultureInfo.InvariantCulture);
                }
                return sumPrice / cryptovalute.USDT_USD.Length;
            }
            return -1;
        }
        static public async Task AutoUpdateRates(int refreshRateInHours)
        {
            while (true)
            {
                await UpdateRates();
                await Task.Delay(TimeSpan.FromHours(refreshRateInHours));
                
            }
        }
        static public double GetBtcPrice() => btcPrice;
        static public double GetEthPrice() => ethPrice;
        static public double GetUsdtPrice() => usdtPrice;
    }

}


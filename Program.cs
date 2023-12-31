namespace ExchangeRates
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            var periodInHoursExchange = 24;
            var periodInHoursCrypto = 1;
            await Task.Run(() =>
            {
                CryptoRates.AutoUpdateRates(periodInHoursCrypto);
                ExchangeRates.AutoUpdateRates(periodInHoursExchange);
            });


            new TelegramBot("TOKEN");
            app.Run();
        }
    }
}
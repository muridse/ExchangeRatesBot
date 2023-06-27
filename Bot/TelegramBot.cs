using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExchangeRates
{
    public class TelegramBot
    {
        private string _token {get; set;}
        private TelegramBotClient botClient {get; set;}

        public TelegramBot(string token) 
        {
            _token = token;
            botClient = new TelegramBotClient(_token);
            Start();
        }
        public async Task Start() 
        {
            using CancellationTokenSource cts = new();
            botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token
            );
        }


        // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
        };

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            //await Task.Run(() => exchangeRates.AutoUpdateRates(periodInHoursExchange));

            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Message is not { } message)
                return;
            // Only process text messages
            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;

            if (messageText == "/start")
                await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Что бы узнать курсы валют, напишите боту 'курсы''",
                cancellationToken: cancellationToken);

            if (messageText == "update")
                await ExchangeRates.UpdateRates();
            if (messageText == "updateCrypto")
                await CryptoRates.UpdateRates();

            if (messageText.ToLower() == "курсы") 
            {

                var messageList = new List<string>();
                    messageList.Add($"Данные Московской Биржи на {ExchangeRates.GetRates().Date.Day}." +
                        $"{ExchangeRates.GetRates().Date.Month}.{ExchangeRates.GetRates().Date.Year}");
                    messageList.Add($"🟢 {ExchangeRates.GetRates().Valute.USD.Name}: " +
                        $"{Math.Round(ExchangeRates.GetRates().Valute.USD.Value / ExchangeRates.GetRates().Valute.USD.Nominal, 2)} руб.");
                    messageList.Add($"🟢 {ExchangeRates.GetRates().Valute.EUR.Name}: " +
                        $"{Math.Round(ExchangeRates.GetRates().Valute.EUR.Value / ExchangeRates.GetRates().Valute.EUR.Nominal, 2)} руб.");
                    messageList.Add($"🟢 {ExchangeRates.GetRates().Valute.KZT.Name}: " +
                        $"{Math.Round(ExchangeRates.GetRates().Valute.KZT.Value / ExchangeRates.GetRates().Valute.KZT.Nominal, 2)} руб. " +
                        $"(1 руб. = {Math.Round(1 /(ExchangeRates.GetRates().Valute.KZT.Value / ExchangeRates.GetRates().Valute.KZT.Nominal), 2)} тнг.)");
                    messageList.Add($"\nДанные биржи EXMO за последний час");
                    messageList.Add($"🔵 Bitcoin: {Math.Round(CryptoRates.GetBtcPrice(),2)} USDT");
                    messageList.Add($"🔵 Ethereum: {Math.Round(CryptoRates.GetEthPrice(), 2)} USDT");
                    messageList.Add($"🔵 Tether USDT: {Math.Round(CryptoRates.GetUsdtPrice(), 2)} USD");
                await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: String.Join("\n", messageList),
                cancellationToken: cancellationToken);      
            }
                

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

        }

        Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}

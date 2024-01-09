namespace HotelChief.Application.Services
{
    using HotelChief.Application.IServices;
    using Telegram.Bot;
    using static HotelChief.Application.Services.RoomCleaningService;

    public class TelegramBotService : ITelegramBotService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly long _telegramRoomId;

        public TelegramBotService(ITelegramBotClient botClient, long telegramRoomId)
        {
            _botClient = botClient;
            _telegramRoomId = telegramRoomId;
        }

        public async Task SendTextMessageAsync(string message)
        {
            await _botClient.SendTextMessageAsync(_telegramRoomId, message);
        }
    }
}

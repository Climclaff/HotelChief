namespace HotelChief.Application.IServices
{
    public interface ITelegramBotService
    {
         Task SendTextMessageAsync(string message);
    }
}

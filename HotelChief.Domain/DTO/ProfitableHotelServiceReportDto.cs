namespace HotelChief.Core.DTO
{
    public class ProfitableHotelServiceReportDto
    {
        public int HotelServiceId { get; set; }

        public double TotalRevenue { get; set; }

        public int TotalOrders { get; set; }
    }
}

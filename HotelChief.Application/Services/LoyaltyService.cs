namespace HotelChief.Application.Services
{
    using System.Threading.Tasks;
    using HotelChief.Core.Entities.Abstract;
    using HotelChief.Core.Interfaces;
    using HotelChief.Core.Interfaces.IServices;
    using HotelChief.Infrastructure.EFEntities;

    public class LoyaltyService<T> : ILoyaltyService<T>
        where T : Booking
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoyaltyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AssignLoyaltyPoints(T paymentActivity, int userId)
        {
            double loyaltyPointsEarned = paymentActivity.Amount * 0.1;
            var user = await GetBusinessUser(userId);
            if (user == null)
            {
                return;
            }

            user.LoyaltyPoints ??= 0;

            user.LoyaltyPoints += loyaltyPointsEarned;
            _unitOfWork.GetRepository<Guest>().Update(user);
        }

        public async Task<T?> ApplyDiscount(T paymentActivity, int userId)
        {
            if (paymentActivity.IsDiscounted)
            {
                return null;
            }

            var user = await GetBusinessUser(userId);
            if (user == null)
            {
                return null;
            }

            if (user.LoyaltyPoints >= 0)
            {
                double discountLimit = paymentActivity.Amount * 0.2;
                if (user.LoyaltyPoints >= discountLimit)
                {
                    paymentActivity.Amount -= discountLimit;
                    user.LoyaltyPoints -= discountLimit;
                }
                else
                {
                    paymentActivity.Amount -= user.LoyaltyPoints.Value;
                    user.LoyaltyPoints = 0;
                }

                paymentActivity.IsDiscounted = true;
                _unitOfWork.GetRepository<Guest>().Update(user);
                return paymentActivity;
            }

            return null;
        }

        public async Task Commit()
        {
            await _unitOfWork.Commit();
        }

        private async Task<Guest?> GetBusinessUser(int userId)
        {
            var user = (await _unitOfWork.GetRepository<Guest>().Get(x => x.Id == userId)).FirstOrDefault();
            if (user == null)
            {
                return null;
            }

            return user;
        }
    }
}

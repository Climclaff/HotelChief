namespace HotelChief.Infrastructure.Repositories
{
    using HotelChief.Core.Entities;
    using HotelChief.Core.Interfaces.IRepositories;
    using HotelChief.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}

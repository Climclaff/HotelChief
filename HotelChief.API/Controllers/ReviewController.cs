namespace HotelChief.API.Controllers
{
    using System.Security.Claims;
    using AutoMapper;
    using HotelChief.API.Hubs;
    using HotelChief.API.ViewModels;
    using HotelChief.API.ViewModels.DTO;
    using HotelChief.Core.Entities;
    using HotelChief.Core.Entities.Identity;
    using HotelChief.Core.Interfaces.IServices;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Localization;

    public class ReviewController : Controller
    {
        private readonly UserManager<Infrastructure.EFEntities.Guest> _userManager;
        private readonly IReviewService _reviewService;
        private readonly IStringLocalizer<ReviewController> _localizer;
        private readonly IMapper _mapper;
        private readonly IHubContext<ReviewHub> _hubContext;

        public ReviewController(
        UserManager<Infrastructure.EFEntities.Guest> userManager,
        IReviewService reviewService,
        IMapper mapper,
        IStringLocalizer<ReviewController> localizer,
        IHubContext<ReviewHub> hubContext)
        {
            _userManager = userManager;
            _reviewService = reviewService;
            _mapper = mapper;
            _localizer = localizer;
            _hubContext = hubContext;
        }

        public async Task<ActionResult> Index()
        {
            var reviews = await _reviewService.GetReviewsAsync();
            var uniqueGuestIds = reviews.Select(r => r.GuestId).Distinct().ToList();
            var users = await _userManager.Users.Where(u => uniqueGuestIds.Contains(u.Id)).ToListAsync();
            var reviewsWithUsers = MapUsersToDTO(reviews, users);
            ReviewViewModel model = new ReviewViewModel();
            model.Reviews = reviewsWithUsers;

            if (model.Reviews != null)
            {
                int i = 0;
                bool[] commentOwnership = new bool[reviews.Count()];
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (var review in model.Reviews)
                {
                    bool isCommentOwner = IsCommentOwner(Convert.ToInt32(userId), review.GuestId);
                    if (isCommentOwner)
                    {
                        commentOwnership[i] = true;
                    }

                    i++;
                }

                TempData["commentOwnership"] = commentOwnership;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddReview(ReviewViewModel review)
        {
            if (review.Comment == null)
            {
                TempData["ErrorMessage"] = _localizer["Empty review"].ToString();
                return RedirectToAction(nameof(Index));
            }

            if (review.Comment.Count() > 500)
            {
                TempData["ErrorMessage"] = _localizer["Message too big"].ToString();
                return RedirectToAction(nameof(Index));
            }

            if (review.Rating < 0 || review.Rating > 5)
            {
                TempData["ErrorMessage"] = _localizer["Select Rating"].ToString();
                return RedirectToAction(nameof(Index));
            }

            review.Timestamp = DateTime.UtcNow;
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                review.GuestId = Convert.ToInt32(userId);
                if (ModelState.IsValid)
                {
                    await _reviewService.AddReviewAsync(_mapper.Map<ReviewViewModel, Review>(review));
                    await _reviewService.Commit();
                    TempData["SuccessMessage"] = _localizer["Review shared"].ToString();
                    return RedirectToAction(nameof(Index));
                }
            }

            TempData["ErrorMessage"] = _localizer["Something went wrong"].ToString();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(IFormCollection collection)
        {
            try
            {
                string val = collection["commentId"];
                if (val != null)
                {
                    int id = Convert.ToInt32(val);
                    var user = await _userManager.GetUserAsync(HttpContext.User);
                    if (user != null)
                    {
                        IList<Claim> claims = await _userManager.GetClaimsAsync(user);
                        Review review = await _reviewService.GetReviewByIdAsync(id);

                        bool isCommentOwner = IsCommentOwner(user.Id, review.GuestId);
                        bool isAdmin = false;

                        for (int i = 0; i < claims.Count; i++)
                        {
                            if (claims[i].Type.ToString() == "IsAdmin" && claims[i].Value.ToString() == "true")
                            {
                                isAdmin = true;
                            }
                        }

                        if (isCommentOwner || isAdmin)
                        {
                            await _reviewService.DeleteReviewAsync(review);
                            await _reviewService.Commit();
                            TempData["SuccessMessage"] = _localizer["Review deleted"].ToString();
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }

                TempData["ErrorMessage"] = _localizer["Cant delete other reviews"].ToString();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["ErrorMessage"] = _localizer["Something went wrong"].ToString();
                return View();
            }
        }

        [HttpPost]
        public async Task Upvote(int reviewId)
        {
            var review = await _reviewService.GetReviewByIdAsync(reviewId);
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                var updatedUpvotes = await _reviewService.UpvoteReviewAsync(reviewId, Convert.ToInt32(userId));

                review = await _reviewService.GetReviewByIdAsync(reviewId);
                await NotifyClientsOfVotes(reviewId, updatedUpvotes, review.Downvotes);
            }
        }

        [HttpPost]
        public async Task Downvote(int reviewId)
        {
            var review = await _reviewService.GetReviewByIdAsync(reviewId);
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                var updatedDownvotes = await _reviewService.DownvoteReviewAsync(reviewId, Convert.ToInt32(userId));

                review = await _reviewService.GetReviewByIdAsync(reviewId);
                await NotifyClientsOfVotes(reviewId, review.Upvotes, updatedDownvotes);
            }
        }

        private bool IsCommentOwner(int id, int reviewGuestId)
        {
            if (id == reviewGuestId)
            {
                return true;
            }

            return false;
        }

        private async Task NotifyClientsOfVotes(int reviewId, int upvotes, int downvotes)
        {
            await _hubContext.Clients.All.SendAsync("UpdateVotes", reviewId, upvotes, downvotes);
        }

        private List<ReviewDTO> MapUsersToDTO(IEnumerable<Review> reviews, List<Infrastructure.EFEntities.Guest> users)
        {
            return reviews
                .Select(review => new
                {
                    Review = review,
                    User = users.FirstOrDefault(u => u.Id == review.GuestId),
                })
                .Select(pair => new ReviewDTO
                {
                    ReviewId = pair.Review.ReviewId,
                    GuestId = pair.Review.GuestId,
                    GuestEmail = pair.User?.Email,
                    Rating = pair.Review.Rating,
                    Comment = pair.Review.Comment,
                    Upvotes = pair.Review.Upvotes,
                    Downvotes = pair.Review.Downvotes,
                    Timestamp = pair.Review.Timestamp,
                })
                .ToList();
        }
    }
}

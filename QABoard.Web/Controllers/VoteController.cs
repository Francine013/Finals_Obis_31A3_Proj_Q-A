using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QABoard.Infrastructure.Entities;
using QABoard.Services.Interfaces;

namespace QABoard.Web.Controllers
{
    [Authorize]
    public class VoteController : Controller
    {
        private readonly IVoteService _voteService;
        private readonly UserManager<ApplicationUser> _userManager;

        public VoteController(IVoteService voteService,
            UserManager<ApplicationUser> userManager)
        {
            _voteService = voteService;
            _userManager = userManager;
        }

        [HttpPost]
        public IActionResult Upvote(int answerId, int questionId)
        {
            var userId = _userManager.GetUserId(User);
            var hasVoted = _voteService.HasUserVoted(answerId, userId);

            if (hasVoted)
            {
                _voteService.RemoveVote(answerId, userId);
            }
            else
            {
                _voteService.Upvote(answerId, userId);
            }

            return RedirectToAction("Details", "Question", new { id = questionId });
        }
    }
}
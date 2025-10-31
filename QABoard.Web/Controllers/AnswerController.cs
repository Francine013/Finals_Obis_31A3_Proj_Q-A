using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QABoard.Infrastructure.Entities;
using QABoard.Services.Interfaces;
using QABoard.Web.Models;

namespace QABoard.Web.Controllers
{
    [Authorize]
    public class AnswerController : Controller
    {
        private readonly IAnswerService _answerService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AnswerController(IAnswerService answerService,
            UserManager<ApplicationUser> userManager)
        {
            _answerService = answerService;
            _userManager = userManager;
        }

        [HttpPost]
        public IActionResult Create(AnswerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var answer = new Answer
                {
                    Content = model.Content,
                    QuestionId = model.QuestionId,
                    UserId = _userManager.GetUserId(User),
                    CreatedAt = DateTime.Now
                };

                _answerService.Create(answer);
                return RedirectToAction("Details", "Question", new { id = model.QuestionId });
            }

            return RedirectToAction("Details", "Question", new { id = model.QuestionId });
        }

        [HttpPost]
        public IActionResult Delete(int id, int questionId)
        {
            var answer = _answerService.GetById(id);
            if (answer == null)
            {
                return NotFound();
            }

            var currentUserId = _userManager.GetUserId(User);
            if (answer.UserId != currentUserId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            _answerService.Delete(id);
            return RedirectToAction("Details", "Question", new { id = questionId });
        }
    }
}
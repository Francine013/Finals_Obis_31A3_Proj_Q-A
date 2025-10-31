using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QABoard.Infrastructure.Data;
using QABoard.Infrastructure.Entities;
using QABoard.Services.Interfaces;

namespace QABoard.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IQuestionService _questionService;
        private readonly IAnswerService _answerService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(
            AppDbContext context,
            IQuestionService questionService,
            IAnswerService answerService,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _questionService = questionService;
            _answerService = answerService;
            _userManager = userManager;
        }

        public IActionResult Dashboard()
        {
            ViewBag.TotalQuestions = _context.Questions.Count();
            ViewBag.TotalAnswers = _context.Answers.Count();
            ViewBag.TotalUsers = _context.Users.Count();
            ViewBag.TotalVotes = _context.Votes.Count();

            return View();
        }

        public IActionResult Users()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Users");
        }

        public IActionResult Questions()
        {
            var questions = _questionService.GetAll();
            return View(questions);
        }

        [HttpPost]
        public IActionResult DeleteQuestion(int id)
        {
            _questionService.Delete(id);
            return RedirectToAction("Questions");
        }
    }
}
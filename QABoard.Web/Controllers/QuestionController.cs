using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QABoard.Infrastructure.Entities;
using QABoard.Services.Interfaces;
using QABoard.Web.Models;

namespace QABoard.Web.Controllers
{
    [Authorize]
    public class QuestionController : Controller
    {
        private readonly IQuestionService questionService;
        private readonly UserManager<ApplicationUser> userManager;

        public QuestionController(IQuestionService questionService, UserManager<ApplicationUser> userManager)
        {
            this.questionService = questionService;
            this.userManager = userManager;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var questions = questionService.GetAll();
            var viewModels = questions.Select(q => new QuestionViewModel
            {
                Id = q.Id,
                Title = q.Title,
                Content = q.Content,
                UserId = q.UserId,
                UserName = q.User.FullName,
                CreatedAt = q.CreatedAt,
                AnswerCount = q.Answers.Count
            }).ToList();

            return View(viewModels);
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var question = questionService.GetById(id);
            if (question == null)
            {
                return NotFound();
            }

            var currentUserId = userManager.GetUserId(User);

            var viewModel = new QuestionViewModel
            {
                Id = question.Id,
                Title = question.Title,
                Content = question.Content,
                UserId = question.UserId,
                UserName = question.User.FullName,
                CreatedAt = question.CreatedAt,
                AnswerCount = question.Answers.Count
            };

            ViewBag.Answers = question.Answers.Select(a => new AnswerViewModel
            {
                Id = a.Id,
                Content = a.Content,
                UserId = a.UserId,
                UserName = a.User.FullName,
                CreatedAt = a.CreatedAt,
                VoteCount = a.Votes.Count,
                HasVoted = a.Votes.Any(v => v.UserId == currentUserId)
            }).OrderByDescending(a => a.VoteCount).ToList();

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(QuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var question = new Question
                {
                    Title = model.Title,
                    Content = model.Content,
                    UserId = userManager.GetUserId(User),
                    CreatedAt = DateTime.Now
                };

                questionService.Create(question);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var question = questionService.GetById(id);
            if (question == null)
            {
                return NotFound();
            }

            var currentUserId = userManager.GetUserId(User);
            if (question.UserId != currentUserId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var viewModel = new QuestionViewModel
            {
                Id = question.Id,
                Title = question.Title,
                Content = question.Content
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(QuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var question = questionService.GetById(model.Id);
                if (question == null)
                {
                    return NotFound();
                }

                var currentUserId = userManager.GetUserId(User);
                if (question.UserId != currentUserId && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                question.Title = model.Title;
                question.Content = model.Content;
                questionService.Update(question);

                return RedirectToAction("Details", new { id = model.Id });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var question = questionService.GetById(id);
            if (question == null)
            {
                return NotFound();
            }

            var currentUserId = userManager.GetUserId(User);
            if (question.UserId != currentUserId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            questionService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quiz.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizWeb.Controllers
{
    [Authorize]
    public class QuizController : Controller
    {
        private readonly IQuizService quizService;
        private readonly IUserAnswerService userAnswerService;

        public QuizController(IQuizService quizService, IUserAnswerService userAnswerService)
        {
            this.quizService = quizService;
            this.userAnswerService = userAnswerService;
        }
        public IActionResult Test(int quizId)
        {
            this.quizService.StartQuiz(this.User?.Identity?.Name, quizId);
            var viewModel = this.quizService.GetQuizById(quizId);
            return this.View(viewModel);
        }
        public IActionResult Submit(int quizId)
        {
            foreach (var item in this.Request.Form)
            {
                var questionId = int.Parse(item.Key.Replace("q_", string.Empty));
                var answerId = int.Parse(item.Value);
                this.userAnswerService.AddUserAnswer(this.User.Identity.Name, questionId, answerId);
            }
            return this.RedirectToAction("Results", new { quizId });
        }
        public IActionResult Results(int quizId)
        {
            var points = this.userAnswerService.UserResult(this.User.Identity.Name, quizId);
            return this.View(points);
        }
    }
}

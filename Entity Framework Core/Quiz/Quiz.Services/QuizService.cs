using System;
using System.Linq;
using Quiz.Data;
using Quiz.Models;
using Quiz.Services.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Quiz.Services
{
    public class QuizService : IQuizService
    {
        private readonly ApplicationDbContext applicationDbContext;
        public QuizService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public int Add(string title)
        {
            var quiz = new Quiz.Models.Quiz
            {
                Title = title
            };
            this.applicationDbContext.Quizes.Add(quiz);
            this.applicationDbContext.SaveChanges();
            return quiz.Id;
        }
      public QuizViewModel GetQuizById(int quizId)
        {
            var quiz = this.applicationDbContext.Quizes.Include(x=>x.Questions).ThenInclude(x=>x.Answers).FirstOrDefault(x => x.Id == quizId);
            var quizViewModel = new QuizViewModel
            {
                Id=quiz.Id,
                Title = quiz.Title,
                QUestions = quiz.Questions.OrderBy(r=>Guid.NewGuid()).Select(x => new QuestionViewModel
                {
                    Id=x.Id,
                    Title = x.Title,
                    Answers = x.Answers.OrderBy(r => Guid.NewGuid()).Select(a => new AnswerViewModel
                    {
                        Title = a.Title,
                        Id = a.Id
                    })
                })
            };
            return quizViewModel;
        }

        public IEnumerable<UserQuizViewModel> GetQuizesByUsername(string userName)
        {
            var quizes = applicationDbContext.Quizes.Select(x => new UserQuizViewModel
            {
                Id = x.Id,
                Title = x.Title,
            }).ToList();
            foreach (var quiz in quizes)
            {
                var questionsCount = applicationDbContext.UsersAnswers.Count(UserAnswer => UserAnswer.IdentityUser.UserName == userName && UserAnswer.Question.QuizId == quiz.Id);
                if (questionsCount==0)
                {
                    quiz.Status = QuizStatus.NotStarted;
                    continue;
                }
                var answeredQuestions = applicationDbContext.UsersAnswers.Count(UserAnswer => UserAnswer.IdentityUser.UserName == userName && UserAnswer.Question.QuizId == quiz.Id
                && UserAnswer.AnswerId.HasValue);
                if (answeredQuestions==questionsCount)
                {
                    quiz.Status = QuizStatus.Finished;
                }
                else
                {
                    quiz.Status = QuizStatus.InProgress;
                }
            }
            return quizes;
        }

        public void StartQuiz(string username, int quizId)
        {
            if (applicationDbContext.UsersAnswers.Any(x=>x.IdentityUser.UserName==username && x.Question.QuizId == quizId))
            {
                return;
            }
            var userId = this.applicationDbContext.Users.Where(x => x.UserName == username).Select(x => x.Id).FirstOrDefault();
            var questions = applicationDbContext.Questions.Where(x => x.QuizId == quizId).Select(x => new { x.Id }).ToList();
            foreach (var question in questions)
            {
                applicationDbContext.UsersAnswers.Add(new UserAnswer
                {
                    AnswerId = null,
                    IdentityUserId = userId,
                    QuestionId = question.Id,
                });
            }
            applicationDbContext.SaveChanges();
        }
    }
}

using System;
using System.Linq;
using Quiz.Data;
using Quiz.Models;
using Quiz.Services.Models;
using Microsoft.EntityFrameworkCore;

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
                QUestions = quiz.Questions.Select(x => new QuestionViewModel
                {
                    Id=x.Id,
                    Title = x.Title,
                    Answers = x.Answers.Select(a => new AnswerViewModel
                    {
                        Title = a.Title,
                        Id = a.Id
                    })
                })
            };
            return quizViewModel;
        }
    }
}

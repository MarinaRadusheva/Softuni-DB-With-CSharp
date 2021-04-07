using System;
using Quiz.Data;
using Quiz.Models;

namespace Quiz.Services
{
    public class QuizService : IQuizService
    {
        private readonly ApplicationDbContext applicationDbContext;
        public QuizService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public void Add(string title)
        {
            var quiz = new Models.Quiz
            {
                Title = title
            };
            this.applicationDbContext.Quizes.Add(quiz);
            this.applicationDbContext.SaveChanges();
        }
    }
}

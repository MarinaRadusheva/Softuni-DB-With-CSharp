using Quiz.Data;
using Quiz.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quiz.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly ApplicationDbContext applicationDbContext;
        public QuestionService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public int Add(string title, int quizId)
        {
            var question = new Question
            {
                Title = title,
                QuizId = quizId
            };
            this.applicationDbContext.Questions.Add(question);
            this.applicationDbContext.SaveChanges();
            return question.Id;
        }
    }
}

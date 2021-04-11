using Quiz.Data;
using Quiz.Models;
using Quiz.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Quiz.Services
{
    public class UserAnswerService : IUserAnswerService
    {
        private readonly ApplicationDbContext applicationDbContext;
        public UserAnswerService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public void AddUserAnswer(string userId, int questionId, int answerId)
        {
            var userAnswer = new UserAnswer
            {
                IdentityUserId = userId,
                QuestionId=questionId,
                AnswerId = answerId
            };
            this.applicationDbContext.UsersAnswers.Add(userAnswer);
            this.applicationDbContext.SaveChanges();
        }
        public void BulkAddUserAnswer(QuizInputModel quizInputModel)
        {
            var userAnswers = new List<UserAnswer>();
            foreach (var item in quizInputModel.Questions)
            {
                var userAnswer = new UserAnswer
                {
                    IdentityUserId = quizInputModel.UserId,
                    AnswerId=item.AnswerId,
                };
                userAnswers.Add(userAnswer);
            }
            this.applicationDbContext.UsersAnswers.AddRange(userAnswers);
            this.applicationDbContext.SaveChanges();
        }
        public int UserResult(string userId, int quizId)
        {
            var totalPoints = this.applicationDbContext.UsersAnswers
                .Where(x => x.IdentityUserId == userId && x.Question.QuizId == quizId).Sum(x => x.Answer.Points);
            return totalPoints;
            //var quiz = this.applicationDbContext.Quizes.Include(x=>x.Questions).ThenInclude(x=>x.Answers).FirstOrDefault(x => x.Id == quizId);
            //var userAnswers = this.applicationDbContext.UsersAnswers.Where(x => x.IdentityUserId == userId && x.QuizId == quizId).ToList();
            //var totalPoints = 0;
            //foreach (var userAnswer in userAnswers)
            //{
            //    totalPoints += quiz.Questions.FirstOrDefault(x => x.Id == userAnswer.QuestionId).Answers.FirstOrDefault(x => x.Id == userAnswer.AnswerId).Points;
            //}
            //return totalPoints;
        }
    }
}

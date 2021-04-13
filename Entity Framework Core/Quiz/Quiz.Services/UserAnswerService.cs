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
        public void AddUserAnswer(string userName, int questionId, int answerId)
        {
            var userId = this.applicationDbContext.Users.Where(x => x.UserName == userName).Select(x => x.Id).FirstOrDefault();
            var userAnswer = this.applicationDbContext.UsersAnswers.FirstOrDefault(x => x.IdentityUserId == userId && x.QuestionId == questionId);
            userAnswer.AnswerId = answerId;
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
        public int UserResult(string userName, int quizId)
        {
            var userId = this.applicationDbContext.Users.Where(x => x.UserName == userName).Select(x => x.Id).FirstOrDefault();
            var totalPoints = this.applicationDbContext.UsersAnswers
                .Where(x => x.IdentityUserId == userId && x.Question.QuizId == quizId).Sum(x => x.Answer.Points);
            return totalPoints;
            
        }
    }
}

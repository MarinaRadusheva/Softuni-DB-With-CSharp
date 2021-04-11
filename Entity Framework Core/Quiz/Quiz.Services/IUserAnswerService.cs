using Quiz.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quiz.Services
{
    public interface IUserAnswerService
    {
        void AddUserAnswer(string userId, int questionId, int answerId);
        void BulkAddUserAnswer(QuizInputModel quizInputModel);
        int UserResult(string userId, int quizId);
    }
}

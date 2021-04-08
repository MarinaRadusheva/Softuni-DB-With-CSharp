using Quiz.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quiz.Services
{
    public interface IQuizService
    {
        void Add(string title);
        QuizViewModel GetQuizById(int quizId);
    }
}

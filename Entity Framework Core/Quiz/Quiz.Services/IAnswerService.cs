using System;
using System.Collections.Generic;
using System.Text;

namespace Quiz.Services
{
    public interface IAnswerService
    {
        int Add(string title, int points, bool isCorrect, int questionId);
    }
}

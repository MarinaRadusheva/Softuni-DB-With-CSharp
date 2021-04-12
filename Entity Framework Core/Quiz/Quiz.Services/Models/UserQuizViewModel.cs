using System;
using System.Collections.Generic;
using System.Text;

namespace Quiz.Services.Models
{
    public class UserQuizViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public QuizStatus Status { get; set; }
    }
    public enum QuizStatus
    {
        NotStarted = 1,
        InProgress =2,
        Finished = 3,
    }
}

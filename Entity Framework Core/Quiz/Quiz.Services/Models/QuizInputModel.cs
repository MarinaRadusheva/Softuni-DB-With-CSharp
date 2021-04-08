using System;
using System.Collections.Generic;
using System.Text;

namespace Quiz.Services.Models
{
    public class QuizInputModel
    {
        public string UserId { get; set; }
        public int QuizId { get; set; }
        public ICollection<QuestionInputModel> Questions { get; set; }
    }
}

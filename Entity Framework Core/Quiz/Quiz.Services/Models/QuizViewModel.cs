using System;
using System.Collections.Generic;
using System.Text;

namespace Quiz.Services.Models
{
    public class QuizViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<QuestionViewModel> QUestions { get; set; }
    }
}

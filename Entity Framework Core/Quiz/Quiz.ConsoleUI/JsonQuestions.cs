using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.ConsoleUI
{
    public class JsonQuestions
    {
            public string Question { get; set; }
            public JsonAnswer[] Answers { get; set; }
    }
}

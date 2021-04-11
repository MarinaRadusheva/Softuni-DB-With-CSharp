using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Quiz.Data;
using Quiz.Services;
using System;
using System.Collections.Generic;
using System.IO;

namespace Quiz.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var json = File.ReadAllText("EF-Core-Quiz.json");
            var questions = JsonConvert.DeserializeObject<IEnumerable<JsonQuestions>>(json);
            var quizService = serviceProvider.GetService<IQuizService>();
            var questionService = serviceProvider.GetService<IQuestionService>();
            var answerService = serviceProvider.GetService<IAnswerService>();
            var quizId = quizService.Add("EF Core");
            foreach (var question in questions)
            {
                var questionId = questionService.Add(question.Question, quizId);
                foreach (var answer in question.Answers)
                {
                    answerService.Add(answer.Answer, answer.Correct ? 1 : 0, answer.Correct, questionId);
                }
            }
            //var quizService = serviceProvider.GetService<IQuizService>();
            //quizService.Add("C# DB");
            //var questionService = serviceProvider.GetService<IQuestionService>();
            //questionService.Add("What if Entity Framework Core?", 1);
            //var answerService = serviceProvider.GetService<IAnswerService>();
            //answerService.Add("It is ORM", 5, true, 1);
            //answerService.Add("It is micro ORM", 0, false, 1);
            //var userAnswerService = serviceProvider.GetService<IUserAnswerService>();
            //userAnswerService.AddUserAnswer("595476fc-ba98-4963-ac76-681951d8611c", 1, 1, 1);
            //var userResult = userAnswerService.UserResult("595476fc-ba98-4963-ac76-681951d8611c", 1);
            //Console.WriteLine(userResult);
            //var quiz = quizService.GetQuizById(1);
            //Console.WriteLine(quiz.Title);
            //foreach (var item in quiz.QUestions)
            //{
            //    Console.WriteLine(item.Title);
            //    foreach (var answer in item.Answers)
            //    {
            //        Console.WriteLine(answer.Title);
            //    }
            //}

        }
        private static void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddTransient<IQuizService, QuizService>();
            services.AddTransient<IQuestionService, QuestionService>();
            services.AddTransient<IAnswerService, AnswerService>();
            services.AddTransient<IUserAnswerService, UserAnswerService>();
        }
    }
}

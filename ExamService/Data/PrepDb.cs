using Microsoft.EntityFrameworkCore;
using ExamService.Models;

namespace ExamService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {

            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<DBContext>(), true);
            }
        }

        private static void SeedData(DBContext context, bool isProd)
        {
            if(isProd)
            {
                Console.WriteLine("---> Attemp to apply migration...");
                try
                {
                    //context.Database.EnsureCreated();
                    context.Database.Migrate();
                }
                catch (Exception ex) 
                {
                    
                    Console.WriteLine($"---> Fail to run migration: {ex.Message}");
                }
            }
            if(!context.Topics.Any())
            {
                Console.WriteLine("---> Seeding data ...");

              
                    Topic englishTopic = new Topic(){Name = "English"};
                    Topic mathTopic = new Topic(){Name = "Math"};

                    context.Topics.AddRange(
                        englishTopic,
                        mathTopic
                    );


                    Exam examA = new Exam(){
                        Name = "firstTestExam",
                        AuthorEmail = "anhanh1253@gmail.com",
                        Topic = englishTopic
                    };

                    context.Exams.Add(examA);

                    Question questionA = new Question(){Content = "Question A", Exam = examA};
                    Question questionB = new Question(){Content = "Question B", Exam = examA};

                    context.Questions.AddRange(questionA, questionB);

                    Option optionA = new Option(){ Content = "option A", IsCorrect = true, Question = questionA };
                    Option optionB = new Option(){ Content = "option B", Question = questionA };
                    Option optionC = new Option(){ Content = "option C", IsCorrect = true, Question = questionB };
                    Option optionD = new Option(){ Content = "option D", Question = questionB };
                    
                    context.Options.AddRange(optionA, optionB, optionC, optionD);

                    
                if(!context.Users.Any())
                {
                    User user = new User(){ ExternalId = 1, Email = "anhanh1253@gmail.com"};
                    context.Users.Add(user);
                }

                if(!context.Attemps.Any() && context.Exams.Any() && context.Users.Any())
                {
                    Exam exam = context.Exams.First();
                    User user = context.Users.First();
                    Console.WriteLine(exam.Name + " " + user.Email);
                    Attemp attemp = new Attemp(){ TotalScore = 10, Exam = exam, User = user};
                    context.Attemps.Add(attemp);
                }

               

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("---> We already have data");
            }
        }
    }
}
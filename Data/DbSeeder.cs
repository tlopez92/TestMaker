using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace TestMakerFreeWeb.Data
{
    public class DbSeeder
    {
        public DbSeeder()
        {
        }

        #region Public Methods
        public static void Seed(ApplicationDbContext dbContext)
        {
            // create default users (if there are none)
            if (!dbContext.Users.Any())
                CreateUsers(dbContext);

            // Create default Quizzes (if there are none) together with their set of Q&A
            if (!dbContext.Quizzes.Any())
                CreateQuizzes(dbContext);
        }
        #endregion

        #region Seed Methods
        private static void CreateUsers(ApplicationDbContext dbContext)
        {
            // local variables
            DateTime createdDate = new DateTime(2019, 02, 19, 12, 00, 00);
            DateTime lastModifiedDate = DateTime.Now;

            // Create the Admin ApplicationUser account (if it doesn't exist already)

            var user_Admin = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Admin",
                Email = "admin@testmakerfree.com",
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            };

            // Insert the admin user into the database
            dbContext.Users.Add(user_Admin);

#if DEBUG
            // create some sample registerd user accounts (if they don't exist already)

            var user_John = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "John",
                Email = "john@testmakerfree.com",
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            };

            var user_Bob = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Bob",
                Email = "bob@testermakerfree.com",
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            };

            var user_Mike = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Mike",
                Email = "mike@testmakerfree.com",
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            };

            // Insert sample registered users into the db
            dbContext.Users.AddRange(user_John, user_Bob, user_Mike);
#endif
            dbContext.SaveChanges();
        }

        private static void CreateQuizzes(ApplicationDbContext dbContext)
        {
            // local variables
            DateTime createdDate = new DateTime(2019, 02, 19, 12, 00, 00);
            DateTime lastModifiedDate = DateTime.Now;

            // retrive the admin user, which we'll use as default author.
            var authorId = dbContext.Users
                .Where(u => u.UserName == "Admin")
                .FirstOrDefault()
                .Id;

#if DEBUG
            // create 47 sample quizzes with auto-generated data including questions, answers & results
            var num = 47;
            for(int i = 1; i <= num; i++)
            {
                CreateSampleQuiz(
                dbContext,
                i,
                authorId,
                num - 1,
                3,
                3,
                3,
                createdDate.AddDays(-num));
            }
#endif
            // create 3 more quizzes with better descriptive data
            EntityEntry<Quiz> e1 = dbContext.Quizzes.Add(new Quiz()
            {
                UserId = authorId,
                Title = "Are you more Light or Dark side of the Force?",
                Description = "Star Wars Personality Test",
                Text = @"Choose wisely you must, young padawan: " +
                        "this test will prove if your will is strong enough " +
                        "to adhere to the principles of the light side of the Force " +
                        "or if you're fated to embrace the dark side. " +
                        "No you want to become a true JEDI, you can't possibly miss this!",
                ViewCount = 2343,
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            });

            EntityEntry<Quiz> e2 = dbContext.Quizzes.Add(new Quiz()
            {
                UserId = authorId,
                Title = "GenX, GenY or GenZ?",
                Description = "Find out what decade most represents you",
                Text = @"Do you feel comfortable in your generation? " +
                        "What year should you have been born in?" +
                        "Here's a bunch of questions that will help you to find out!",
                ViewCount = 4180,
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            });

            EntityEntry<Quiz> e3 = dbContext.Quizzes.Add(new Quiz()
            {
                UserId = authorId,
                Title = "Which Shingeki No Kyojin character are you?",
                Description = "Attack On Tital Personality Test",
                Text = @"Do you relentlessly seek revenge like Eren? " +
                        "Are you willing to put your life on the stake to protect your friends like Mikasa? " +
                        "Would you trust your fighting skills like Levi " +
                        "or rely on your strategies and tactics like Arwin? " +
                        "Unveil your true self with this Attack On Titan Personality Test!",
                ViewCount = 5203,
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            });

            // persist the changes on the Database
            dbContext.SaveChanges();
        }
        #endregion

        #region Utility Methods
        private static void CreateSampleQuiz(
        ApplicationDbContext dbContext,
        int num,
        string authorId,
        int viewCount,
        int numberOfQuestions,
        int numberOfAnswersPerQuestion,
        int numberOfResults,
        DateTime createdDate)
        {
            var quiz = new Quiz()
            {
                UserId = authorId,
                Title = String.Format("Quiz {0} Title", num),
                Description = String.Format("This is a sample description for quiz {0}.", num),
                Text = "This is a sample quiz created by the DbSeeder class for testing purposes. " +
                        "All the questions, answers & results are auto-generated as well.",
                ViewCount = viewCount,
                CreatedDate = createdDate,
                LastModifiedDate = createdDate
            };
            dbContext.Quizzes.Add(quiz);
            dbContext.SaveChanges();

            for(int i = 0; i < numberOfQuestions; i++)
            {
                var question = new Question()
                {
                    QuizId = quiz.Id,
                    Text = "This is a sample question created by the DbSeeder class for testing purposes. " +
                            "All the child answers are auto-generated as well.",
                    CreatedDate = createdDate,
                    LastModifiedDate = createdDate
                };
                dbContext.Questions.Add(question);
                dbContext.SaveChanges();

                for(int i2 = 0; i2 < numberOfAnswersPerQuestion; i2++)
                {
                    var e2 = dbContext.Answers.Add(new Answer()
                    {
                        QuestionId = question.Id,
                        Text = "This is a sample answer created by the DbSeeder class for testing purposes. ",
                        Value = i2,
                        CreatedDate = createdDate,
                        LastModifiedDate = createdDate
                    });
                }
            }

            for(int i = 0; i < numberOfResults; i++)
            {
                dbContext.Results.Add(new Result()
                {
                    QuizId = quiz.Id,
                    Text = "This is a sample result created by the DbSeeder class for testing purposes. ",
                    MinValue = 0,
                    MaxValue = numberOfAnswersPerQuestion * 2,
                    CreatedDate = createdDate,
                    LastModifiedDate = createdDate
                });
            }
            dbContext.SaveChanges();
        }
        #endregion
    }
}

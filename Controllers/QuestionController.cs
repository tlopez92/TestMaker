using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMakerFreeWebApp.ViewModels;
using System.Collections.Generic;
using TestMakerFreeWeb.Data;
using System.Linq;
using Mapster;
using TestMakerFreeWeb.Controllers;

namespace TestMakerFreeWebApp.Controllers
{
    [Route("api/[controller]")]
    public class QuestionController : BaseApiController
    {
        #region Private Fields
        #endregion

        #region Constructor
        public QuestionController(ApplicationDbContext context) : base(context)
        {
        }
        #endregion

        #region RESTful conventions methods
        /// <summary>
        /// Retrieves the Question with the given {id}
        /// </summary>
        /// <param name="id">The ID of an existing Question</param>
        /// <returns>the Question with the given {id}</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var question = this.dbContext.Questions.Where(i => i.Id == id).FirstOrDefault();

            // handle requests asking for non-existing questions
            if (question == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Question ID {0} has not been found", id)
                });
            }

            return new JsonResult(question.Adapt<QuestionViewModel>(), jsonSettings);
        }

        /// <summary>
        /// Adds a new Question to the Database
        /// </summary>
        /// <param name="model">The QuestionViewModel containing the data to insert</param>
        [HttpPut]
        public IActionResult Put([FromBody]QuestionViewModel model)
        {
            // return a generic HTTP Status 500 (server error)
            // if the client payload is invalid
            if (model == null) 
                return new StatusCodeResult(500);

            // map the ViewModel to the Model
            var question = model.Adapt<Question>();

            // override those properties
            // that should be set from the server-side only
            question.QuizId = model.QuizId;
            question.Text = model.Text;
            question.Notes = model.Notes;

            // properties set from server-side
            question.CreatedDate = DateTime.Now;
            question.LastModifiedDate = question.CreatedDate;

            // add the new question
            this.dbContext.Questions.Add(question);
            // persist the changes to the db
            this.dbContext.SaveChanges();

            // return the newly-created Question to the client.
            return new JsonResult(question.Adapt<QuestionViewModel>(), jsonSettings);
        }

        /// <summary>
        /// Edit the Question with the given {id}
        /// </summary>
        /// <param name="model">The QuestionViewModel containing the data to update</param>
        [HttpPost]
        public IActionResult Post([FromBody]QuestionViewModel model)
        {
            // return generic 500 
            // if client payload is invalid
            if (model == null)
                return new StatusCodeResult(500);

            // retrieve question to edit
            var question = this.dbContext.Questions.Where(q => q.Id == model.Id).FirstOrDefault();

            // handle request asking for non-existing questions
            if (question == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Question ID {0} has not been found", model.Id)
                });
            }

            // handle the update
            // by manually assigning the properties we want to accept from the request
            question.QuizId = model.QuizId;
            question.Text = model.Text;
            question.Notes = model.Notes;

            // properties set from the server-side
            question.LastModifiedDate = DateTime.Now;

            // persist changes to db
            this.dbContext.SaveChanges();

            // return the updated Quiz to the client.
            return new JsonResult(question.Adapt<QuestionViewModel>(), jsonSettings);
        }


        /// <summary>
        /// Deletes the Question with the given {id} from the Database
        /// </summary>
        /// <param name="id">The ID of an existing Question</param>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // retrive the question from th db
            var question = this.dbContext.Questions.Where(q => q.Id == id).FirstOrDefault();

            // handle requests asking for non-existing questions
            if (question == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Question ID {0} has not been found", id)
                });
            }

            // remove the quiz from the dbcontext
            this.dbContext.Questions.Remove(question);
            // persist changes to the db
            this.dbContext.SaveChanges();

            // return an HTTP Status 200 (OK).
            return new OkResult();
        }
        #endregion

        // GET api/question/all
        [HttpGet("All/{quizId}")]
        public IActionResult All(int quizId)
        {
            var questions = this.dbContext.Questions.Where(q => q.QuizId == quizId).ToArray();

            return new JsonResult(questions.Adapt<QuestionViewModel[]>(), jsonSettings);
        }
    }
}


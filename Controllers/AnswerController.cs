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
    public class AnswerController : BaseApiController
    {
        #region private fields
        #endregion

        #region Constructor
        public AnswerController(ApplicationDbContext context) : base(context)
        {
        }
        #endregion

        #region RESTful conventions methods
        /// <summary>
        /// Retrieves the Answer with the given {id}
        /// </summary>
        /// <param name="id">The ID of an existing Answer</param>
        /// <returns>the Answer with the given {id}</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var answer = this.dbContext.Answers.Where(a => a.Id == id).FirstOrDefault();

            if (answer == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Answer ID {0} has not been found", id)
                });
            }

            return new JsonResult(answer.Adapt<AnswerViewModel>(), jsonSettings);
        }

        /// <summary>
        /// Adds a new Answer to the Database
        /// </summary>
        /// <param name="model">The AnswerViewModel containing the data to insert</param>
        [HttpPut]
        public IActionResult Put([FromBody]AnswerViewModel model)
        {
            // return a generic HTTP Status 500 (server error)
            // if the client payload is invalid
            if (model == null)
                return new StatusCodeResult(500);

            // map the ViewModel to the Model
            var answer = model.Adapt<Answer>();

            // override those properties
            // that should be set from the server-side only
            answer.QuestionId = model.QuestionId;
            answer.Text = model.Text;
            answer.Notes = model.Notes;

            // properties set from server-side
            answer.CreatedDate = DateTime.Now;
            answer.LastModifiedDate = answer.CreatedDate;

            // add the new question
            this.dbContext.Answers.Add(answer);
            // persist the changes to the db
            this.dbContext.SaveChanges();

            // return the newly-created Question to the client.
            return new JsonResult(answer.Adapt<AnswerViewModel>(), jsonSettings);
        }

        /// <summary>
        /// Edit the Answer with the given {id}
        /// </summary>
        /// <param name="model">The AnswerViewModel containing the data to update</param>
        [HttpPost]
        public IActionResult Post([FromBody]AnswerViewModel model)
        {

            // return generic 500 
            // if client payload is invalid
            if (model == null)
                return new StatusCodeResult(500);

            // retrieve question to edit
            var answer = this.dbContext.Answers.Where(q => q.Id == model.Id).FirstOrDefault();

            // handle request asking for non-existing questions
            if (answer == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Answer ID {0} has not been found", model.Id)
                });
            }

            // handle the update
            // by manually assigning the properties we want to accept from the request
            answer.QuestionId = model.QuestionId;
            answer.Text = model.Text;
            answer.Value = model.Value;
            answer.Notes = model.Notes;

            // properties set from the server-side
            answer.LastModifiedDate = DateTime.Now;

            // persist changes to db
            this.dbContext.SaveChanges();

            // return the updated answer to the client.
            return new JsonResult(answer.Adapt<AnswerViewModel>(), jsonSettings);
        }

        /// <summary>
        /// Deletes the Answer with the given {id} from the Database
        /// </summary>
        /// <param name="id">The ID of an existing Answer</param>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // retrive the question from th db
            var answer = this.dbContext.Answers.Where(q => q.Id == id).FirstOrDefault();

            // handle requests asking for non-existing questions
            if (answer == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Answer ID {0} has not been found", id)
                });
            }

            // remove the answer from the dbcontext
            this.dbContext.Answers.Remove(answer);
            // persist changes to the db
            this.dbContext.SaveChanges();

            // return an HTTP Status 200 (OK).
            return new OkResult();
        }
        #endregion

        // GET api/answer/all
        [HttpGet("All/{questionId}")]
        public IActionResult All(int questionId)
        {
            var answers = this.dbContext.Answers.Where(q => q.QuestionId == questionId).ToArray();

            return new JsonResult(
            answers.Adapt<AnswerViewModel[]>(), jsonSettings);
        }
    }
}

